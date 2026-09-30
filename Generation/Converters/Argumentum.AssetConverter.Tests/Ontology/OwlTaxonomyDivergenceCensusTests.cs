using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Argumentum.AssetConverter.Ontology;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using OWLSharp.Ontology;
using Xunit;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.Ontology
{
    /// <summary>
    /// Census #1666 — l'ecart entre les IRI que la taxonomie PRODUIT et les concepts que
    /// l'ontologie committée PORTE, nommé plutôt que silencieux.
    ///
    /// <para><b>Pourquoi cet organe existe.</b> <c>docs/ontology/argumentum.owl</c> est produit par une
    /// passe <c>--generate-owl</c> délibérée : un écart entre deux régénérations est donc <b>attendu</b>.
    /// Ce qui ne l'est pas, c'est qu'il soit <b>invisible</b> : rien ne distingue une dérive ordinaire
    /// d'un oubli, et #133 (publication, qui <b>fige les IRI</b>) peut partir à tout moment. Mesuré sur
    /// <c>f95c0b70</c> : 1313 IRI produites par le CSV, 1306 concepts portés par le fichier, 16 absents,
    /// 9 orphelins. <b>Réconcilié par la régénération #1681</b> (30/09) : 1313 produites / 1313 portées,
    /// <b>0 absentes, 0 orphelines</b> sur <c>c1491de9</c> — les plafonds descendent à 0 : l'écart devient
    /// un invariant d'égalité, et tout renommage anglais non suivi d'une régénération rougit nommément.</para>
    ///
    /// <para><b>Pourquoi aucun organe existant ne le voyait.</b>
    /// <c>OwlE2EGenerationValidationTests.LoadedOntology_CrossLinkAndAifCounts_MatchTheCorpusExactly</c>
    /// réconcilie bien CSV et OWL — mais sur des <i>comptes d'assertions par verbe</i>. Un concept
    /// <b>renommé</b> conserve ses liens : les comptes restent exacts, l'identité du concept a changé.
    /// Le test est structurellement aveugle au renommage, et <c>OwlIriFragmentValidityTests</c> ne lit
    /// que la grammaire des fragments. Celui-ci compare les <b>ensembles d'identités</b>.</para>
    ///
    /// <para><b>La divergence n'est pas un invariant — c'est un point de décision.</b> Les bornes sont
    /// donc <b>posées sur l'écart, jamais sur l'égalité</b>, dans le sens qui laisse la réparation
    /// passer au vert : des <b>planchers</b> sur les deux populations (anti-vacuité — un OWL tronqué
    /// ne peut pas rendre « 0 absent » sur 0 concept), et des <b>plafonds</b> sur l'écart lui-même.
    /// Une régénération qui réconcilie fait <b>baisser</b> l'écart : vert. Un renommage anglais qui
    /// n'est pas suivi d'une régénération le fait <b>monter</b> : rouge, et le message nomme les IRI.
    /// <b>Que faire si c'est rouge</b> : régénérer, ou recaler le plafond avec la date — jamais
    /// effacer la ligne.</para>
    /// </summary>
    [Collection(PublishedOntologyCollection.Name)]
    public class OwlTaxonomyDivergenceCensusTests
    {
        private readonly ITestOutputHelper _out;

        public OwlTaxonomyDivergenceCensusTests(ITestOutputHelper output) => _out = output;

        // Planchers : anti-vacuité. Un fichier illisible ou un glob cassé rend 0 des deux côtés,
        // et « 0 absent » serait alors vrai sans rien prouver.
        private const int ProducedFloor = 1300;
        private const int DeclaredFloor = 1290;

        // Plafonds : 16 et 9 à f95c0b70 (le constat #1666), descendus à 0 à la réconciliation
        // #1681 — l'OWL régénéré (c1491de9) porte exactement les 1313 identités produites.
        // Ils ne peuvent plus remonter : un renommage anglais sans régénération rougit.
        private const int AbsentCeiling = 0;
        private const int OrphanCeiling = 0;

        private static readonly Lazy<OwlAdapter> CommittedOntology = new(() =>
            OwlAdapter.FromFile(Path.Combine(TestRepoRoot.Find(), "docs", "ontology", "argumentum.owl")));

        /// <summary>
        /// Les lignes du CSV Fallacies, lues une seule fois : <c>PK</c> et <c>text_en</c> (chaîne vide
        /// si la cellule est vide). Les deux lectures de cet organe — l'ensemble produit et la table
        /// annoncée — passent par ici, pour qu'elles ne puissent pas diverger sur la façon de lire.
        /// </summary>
        private static IEnumerable<(int Pk, string TextEn)> Rows()
        {
            var csvPath = Path.Combine(TestRepoRoot.Find(),
                "Cards", "Fallacies", "Argumentum Fallacies - Taxonomy.csv");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null,
            };
            using var reader = new StringReader(File.ReadAllText(csvPath));
            using var csv = new CsvReader(reader, config);

            foreach (var record in csv.GetRecords<dynamic>())
            {
                var row = (IDictionary<string, object>)record;
                var rawPk = row.TryGetValue("PK", out var pkRaw) ? pkRaw?.ToString()?.Trim() : null;
                if (!int.TryParse(rawPk, NumberStyles.Integer, CultureInfo.InvariantCulture, out var pk))
                    continue;
                var textEn = row.TryGetValue("text_en", out var enRaw) ? enRaw?.ToString()?.Trim() : null;
                yield return (pk, textEn ?? string.Empty);
            }
        }

        /// <summary>
        /// Les IRI que la taxonomie produit : <see cref="OwlDocumentConfig.GetId"/> sur <c>text_en</c>,
        /// l'organe de production lui-même — jamais une réimplémentation, qui dériverait au premier
        /// changement de <c>GetId</c> (c'est exactement ce que #1651/#1622 point 1 modifient).
        /// </summary>
        private static HashSet<string> ProducedIds()
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var (_, textEn) in Rows())
            {
                if (string.IsNullOrEmpty(textEn))
                    continue;
                var id = OwlDocumentConfig.GetId(textEn);
                if (!string.IsNullOrEmpty(id))
                    ids.Add(id);
            }
            return ids;
        }

        /// <summary>
        /// <c>PK</c> → <c>text_en</c>. La clé de jointure est le <c>PK</c>, jamais le <c>path</c> :
        /// #1503 a mesuré que deux états du corpus ne se joignent pas par <c>path</c> — 153/169 de
        /// recouvrement entre `Archive/v3` et le corpus courant — et la table annoncée nomme des
        /// identités, donc elle se joint sur ce que la table nomme.
        /// </summary>
        private static Dictionary<int, string> EnglishTitlesByPk()
        {
            var titles = new Dictionary<int, string>();
            foreach (var (pk, textEn) in Rows())
                titles[pk] = textEn;
            return titles;
        }

        /// <summary>
        /// Les seize identités que la prochaine regénération publiera — la colonne « Nouvelle IRI »
        /// de la table annoncée à CoursIA (#1525 c.5892207390), <b>re-mesurée au merge de #1661</b>.
        ///
        /// <para><b>Pourquoi elles sont épinglées ici, alors que le census borne déjà l'écart.</b>
        /// Le census compte <i>combien</i> d'identités bougent ; il ne dit pas <i>lesquelles</i>. Or
        /// c'est la liste qui sort du dépôt — elle est annoncée à un consommateur extérieur
        /// (jsboige/CoursIA#4960) qui s'en sert pour réécrire des IRI. #1651 a montré le 2026-09-30
        /// ce que coûte une liste non vérifiée : la ligne PK 1368 annonçait <c>callingCards</c> alors
        /// que la chaîne d'alors mintait <c>callingcards</c> — un guillemet occupant la place de la
        /// lettre à mettre en majuscule. Une réplique de <c>GetId</c> ne suit pas la fonction
        /// qu'elle réplique ; ces épingles passent par <see cref="OwlDocumentConfig.GetId"/>.</para>
        ///
        /// <para><b>Le plafond du census ne peut pas voir l'erreur qu'elles attrapent.</b> Un
        /// renommage corrigé et un autre apparu le même jour laissent le compte à 16 : le nombre est
        /// juste, la liste est fausse. Seule la comparaison d'ensembles les sépare.</para>
        ///
        /// <para><b>Cycle de vie.</b> Les épingles portent sur le titre du CSV, qu'une régénération
        /// ne touche pas (<c>--generate-owl</c> lit la taxonomie, il ne la réécrit pas). Elles ne
        /// rougissent que si quelqu'un renomme un de ces seize titres anglais — c'est-à-dire
        /// exactement quand une annonce neuve est due. Le second membre du test, lui, porte sur
        /// l'écart : une régénération le réconcilie et rend l'inclusion <b>vacue, pas fausse</b>.</para>
        /// </summary>
        private static readonly (int Pk, string Iri)[] AnnouncedIdentities =
        {
            (95, "cafeteriaChristianity"),
            (101, "drinkingTheKoolAid/PeerPressure"),
            (178, "argumentByQuestion"),
            (182, "falseAlternative"),
            (390, "contrastFraming"),
            (667, "imprecision"),
            (690, "inappropriateOperation"),
            (696, "faultyReasoning"),
            (829, "circularDefinition"),
            (944, "hook"),
            (1005, "backpedaling"),
            (1015, "appealToEffort"),
            (1312, "debateSabotage"),
            (1357, "shamArguments"),
            (1368, "callingCards"),
            (1386, "appealToLackOfAccomplishment"),
        };

        /// <summary>
        /// Les concepts que le fichier porte : les sujets des assertions <c>skos:prefLabel</c>,
        /// réduits au fragment d'IRI. C'est la définition que #1666 a mesurée (1306 sur
        /// <c>f95c0b70</c>), et <c>rdf:type</c> ne survit pas à l'aller-retour OWL2XML — donc
        /// <c>prefLabel</c> est le seul ancrage disponible, comme le note <c>OwlAdapter.GetConcepts</c>.
        /// </summary>
        private static HashSet<string> DeclaredFragments()
        {
            var fragments = new HashSet<string>(StringComparer.Ordinal);
            var prefLabel = SKOSVocabulary.PrefLabel.ToString();
            foreach (var subject in CommittedOntology.Value.GetOntology().AnnotationAxioms
                         .OfType<OWLAnnotationAssertion>()
                         .Where(a => a.AnnotationProperty.GetIRI().ToString() == prefLabel)
                         .Select(a => a.SubjectIRI?.ToString() ?? string.Empty)
                         .Where(s => !string.IsNullOrEmpty(s)))
            {
                var hash = subject.LastIndexOf('#');
                if (hash >= 0 && hash + 1 < subject.Length)
                    fragments.Add(subject.Substring(hash + 1));
            }
            return fragments;
        }

        private (HashSet<string> Produced, HashSet<string> Declared) Measure()
        {
            var produced = ProducedIds();
            var declared = DeclaredFragments();

            // Anti-vacuité, avant tout verdict : les deux populations doivent exister.
            produced.Should().HaveCountGreaterThanOrEqualTo(ProducedFloor,
                "le CSV Fallacies doit produire ses IRI. Un compte bas signifie que la lecture a échoué, " +
                "pas que l'écart a disparu.");
            declared.Should().HaveCountGreaterThanOrEqualTo(DeclaredFloor,
                "docs/ontology/argumentum.owl doit porter ses concepts. Un compte bas signifie que le " +
                "chargement a échoué, pas que la divergence est résolue.");

            return (produced, declared);
        }

        [Fact]
        public void TheDivergence_StaysWithinItsPinnedBounds()
        {
            var (produced, declared) = Measure();
            var absent = produced.Except(declared).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var orphans = declared.Except(produced).OrderBy(x => x, StringComparer.Ordinal).ToList();

            _out.WriteLine($"produites={produced.Count} portees={declared.Count} " +
                           $"absentes={absent.Count} orphelines={orphans.Count}");
            _out.WriteLine("ABSENTES  : " + string.Join(", ", absent));
            _out.WriteLine("ORPHELINES: " + string.Join(", ", orphans));

            absent.Should().HaveCountLessThanOrEqualTo(AbsentCeiling,
                $"une IRI que la taxonomie produit et que le fichier ne porte pas est un concept que la " +
                $"publication #133 servirait sous une identité qui n'existe plus. Plafond 0 depuis la " +
                $"réconciliation #1681 (16 absentes à f95c0b70, 0 sur c1491de9). Régénérer " +
                $"(--generate-owl), ou recaler ce plafond avec la date — jamais effacer la ligne. " +
                $"Absentes aujourd'hui ({absent.Count}) : {string.Join(", ", absent)}");

            orphans.Should().HaveCountLessThanOrEqualTo(OrphanCeiling,
                $"un concept que le fichier porte et qu'aucune ligne ne produit est une identité que la " +
                $"publication #133 figerait sans corpus derrière. Plafond 0 depuis la réconciliation " +
                $"#1681 (9 orphelines à f95c0b70, 0 sur c1491de9). Régénérer (--generate-owl), ou " +
                $"recaler ce plafond avec la date — jamais effacer la ligne. " +
                $"Orphelines aujourd'hui ({orphans.Count}) : {string.Join(", ", orphans)}");
        }

        /// <summary>
        /// Contrôle positif du census lui-même : il ne suffit pas que l'écart soit petit, il faut
        /// qu'il <b>décrive la bonne chose</b>. Deux formes sont épinglées par leur nom.
        ///
        /// <para><b>Un concept aligné est des deux côtés.</b> C'est la preuve que les deux lectures
        /// parlent du même espace : sans elle, deux lecteurs revenant vides rendraient un écart nul
        /// que le plafond accepterait.</para>
        ///
        /// <para><b>Un renommage tombe d'un côté et entre de l'autre, jamais les deux.</b> Les deux
        /// paires épinglées sont celles que #1666 a appariées par leur <c>prefLabel</c> français :
        /// « Faulty reasoning » contre « Faulty logics », et « Cafeteria Christianity » contre
        /// « Cafetaria Christianity » — la seconde documentant au passage la coquille du côté OWL.
        /// Le détail de ce qui est vrai dans <b>les deux états</b> est porté par
        /// <see cref="AssertRenamePair"/>.</para>
        /// </summary>
        [Fact]
        public void Renames_LandOnOppositeSides_AndAlignedConcepts_OnBoth()
        {
            var (produced, declared) = Measure();

            foreach (var aligned in new[] { "ableism", "absurd" })
            {
                produced.Should().Contain(aligned,
                    "« {0} » est produit par la taxonomie — il doit rester dans l'ensemble CSV.", aligned);
                declared.Should().Contain(aligned,
                    "« {0} » est porté par l'ontologie committée — il doit rester dans l'ensemble OWL. " +
                    "S'il manque ici alors qu'il est produit, le census a cessé de lire le fichier.", aligned);
            }

            AssertRenamePair(produced, declared, "faultyLogics", "faultyReasoning", "Faulty reasoning");
            AssertRenamePair(produced, declared, "cafetariaChristianity", "cafeteriaChristianity",
                "Cafeteria Christianity");
        }

        /// <summary>
        /// Épingle ce qui reste vrai d'une paire « nom d'hier / nom d'aujourd'hui » <b>dans les deux
        /// états</b> — avant et après la régénération qui réconciliera l'OWL (#1525).
        ///
        /// <para><b>Ce que la première version faisait de travers (revue ai-01, 2026-09-30).</b> Elle
        /// assertait que <c>declared</c> contient l'ancien nom et <b>pas</b> le nouveau — vrai le jour
        /// de la rédaction, faux <b>le jour de la réparation</b>. Un contrôle positif qui rougit sur
        /// la réparation qu'il attend contredit la doctrine des bornes du même fichier : une
        /// régénération doit faire passer l'organe au vert, jamais au rouge.</para>
        ///
        /// <para><b>Côté CSV, l'invariant est absolu</b> : la taxonomie produit le nom d'aujourd'hui
        /// et plus celui d'hier. Le corpus ne se réécrit pas tout seul, donc une régression ici en est
        /// une, avant comme après la régénération.</para>
        ///
        /// <para><b>Côté OWL, l'invariant est « exactement un »</b> : l'ancien avant la régénération,
        /// le nouveau après, <b>jamais les deux, jamais aucun</b>. C'est ce XOR qui porte le contrôle :
        /// « les deux » signale un concept <i>doublé</i> au lieu d'être renommé, « aucun » un concept
        /// <i>perdu</i> au lieu d'être renommé. Les deux états interdits sont vus mordre par
        /// <see cref="TheRenameInvariant_RejectsBothForbiddenStates"/>.</para>
        /// </summary>
        private static void AssertRenamePair(HashSet<string> produced, HashSet<string> declared,
            string oldName, string newName, string label)
        {
            produced.Should().Contain(newName,
                $"« {newName} » est le nom d'aujourd'hui de {label} — il doit rester dans l'ensemble CSV.");
            produced.Should().NotContain(oldName,
                $"« {oldName} » est le nom d'hier de {label} — le corpus ne le produit plus. S'il " +
                "réapparaît ici, la taxonomie a régressé, et c'est cette régression qu'il faut traiter.");

            var declaredOld = declared.Contains(oldName);
            var declaredNew = declared.Contains(newName);

            (declaredOld ^ declaredNew).Should().BeTrue(
                $"l'ontologie committée doit porter exactement un des deux noms de {label} : " +
                $"« {oldName} » avant la régénération de #1525, « {newName} » après. " +
                $"Lu : ancien={declaredOld}, nouveau={declaredNew}. " +
                "Les deux vrais = le concept a été doublé au lieu d'être renommé ; " +
                "les deux faux = il a été perdu au lieu d'être renommé. " +
                "Cette assertion ne rougit ni avant ni après la réparation : elle rougit sur une perte " +
                "ou sur un doublon.");
        }

        /// <summary>
        /// Témoin des deux états que <see cref="AssertRenamePair"/> doit refuser. Il ne suffit pas que
        /// l'invariant soit écrit : il faut l'avoir vu mordre, sinon rien ne dit que la branche est
        /// vivante.
        ///
        /// <para>Les quatre états sont alimentés à la main, sur des ensembles construits, plutôt que
        /// par quatre mutations du fichier de 5,9 Mo : le témoin reste dans le dépôt, la mutation non.
        /// Un second témoin, par mutation réelle des données, est mesuré dans le corps de PR.</para>
        /// </summary>
        [Fact]
        public void TheRenameInvariant_RejectsBothForbiddenStates()
        {
            var produced = new HashSet<string>(StringComparer.Ordinal) { "newName" };
            HashSet<string> Declared(params string[] names) =>
                new HashSet<string>(names, StringComparer.Ordinal);

            Action oldOnly = () => AssertRenamePair(produced, Declared("oldName"),
                "oldName", "newName", "la paire de témoin");
            oldOnly.Should().NotThrow(
                "l'état d'aujourd'hui est légitime : l'OWL porte encore le nom d'hier.");

            Action newOnly = () => AssertRenamePair(produced, Declared("newName"),
                "oldName", "newName", "la paire de témoin");
            newOnly.Should().NotThrow(
                "l'état réparé est légitime : l'OWL porte le nom d'aujourd'hui.");

            Action both = () => AssertRenamePair(produced, Declared("oldName", "newName"),
                "oldName", "newName", "la paire de témoin");
            both.Should().Throw<Exception>().WithMessage("*exactement un*",
                "déclarer les deux noms signale un doublon, pas un renommage.");

            Action neither = () => AssertRenamePair(produced, Declared("autreChose"),
                "oldName", "newName", "la paire de témoin");
            neither.Should().Throw<Exception>().WithMessage("*exactement un*",
                "ne déclarer aucun des deux noms signale une perte, pas un renommage.");
        }

        /// <summary>
        /// La table annoncée sort-elle du dépôt en disant vrai, et en disant <b>tout</b> ?
        ///
        /// <para><b>Membre 1 — les seize entrées sont-elles ce que l'émetteur minta ?</b> Chaque
        /// ligne passe par <see cref="OwlDocumentConfig.GetId"/> sur le <c>text_en</c> que le
        /// <c>PK</c> désigne. C'est la vérification que #1525 a dû faire à la main le 2026-09-30 :
        /// la table avait été calculée par une <b>réplique Python</b> de <c>GetId</c>, qui minta
        /// <c>callingcards</c> là où la fonction réelle, après #1651, minta <c>callingCards</c>.</para>
        ///
        /// <para><b>Membre 2 — la table nomme-t-elle toutes les IRI absentes ?</b> Celles que la
        /// taxonomie produit et que l'OWL ne porte pas doivent toutes figurer dans la table : un
        /// consommateur qui réécrit des IRI d'après elle n'a pas d'autre source. C'est une
        /// <b>inclusion</b>, jamais une égalité — une régénération fait tomber des deux côtés et
        /// laisse l'inclusion vraie. Le plafond du census, lui, ne voit pas un <b>échange</b> : un
        /// renommage corrigé et un autre apparu laissent le compte à 16 pendant que la liste
        /// annoncée devient fausse.</para>
        ///
        /// <para><b>Ce que ce test ne remplace pas.</b> Il ne prouve pas que l'annonce est
        /// <i>publiée</i> — l'annonce vit sur CoursIA#4960, hors de ce dépôt. Il prouve que ce que
        /// le dépôt peut vérifier est vrai au moment où il le vérifie.</para>
        /// </summary>
        [Fact]
        public void TheAnnouncedIdentities_AreWhatTheEmitterMints_AndEveryAbsentIriIsNamed()
        {
            var titleByPk = EnglishTitlesByPk();

            // Anti-vacuité : un PK disparu du corpus rendrait sa ligne verte par omission.
            var missing = AnnouncedIdentities.Where(a => !titleByPk.ContainsKey(a.Pk))
                .Select(a => a.Pk).OrderBy(pk => pk).ToList();
            missing.Should().BeEmpty(
                "les seize lignes annoncées doivent exister dans le CSV des Fallacies — la jointure " +
                "se fait par PK, l'identité que la table nomme. PK introuvables : " +
                string.Join(", ", missing));

            var diverging = new List<string>();
            foreach (var (pk, iri) in AnnouncedIdentities)
            {
                var title = titleByPk[pk];
                var minted = OwlDocumentConfig.GetId(title);
                if (!string.Equals(minted, iri, StringComparison.Ordinal))
                    diverging.Add($"PK {pk} « {title} » minta {minted}, la table annonce {iri}");
            }

            _out.WriteLine($"table annoncée : {AnnouncedIdentities.Length} entrées, "
                           + $"{AnnouncedIdentities.Length - diverging.Count} conformes à GetId");
            foreach (var line in diverging)
                _out.WriteLine("  DIVERGE : " + line);

            diverging.Should().BeEmpty(
                "chaque IRI annoncée doit être celle que GetId minta pour le titre que le PK désigne. " +
                "C'est la vérification que #1525 a faite à la main, après que la table calculée par " +
                "réplique Python eut annoncé callingcards là où GetId minta callingCards (#1651). " +
                "Divergentes : " + string.Join(" | ", diverging));

            var (produced, declared) = Measure();
            var announced = new HashSet<string>(
                AnnouncedIdentities.Select(a => a.Iri), StringComparer.Ordinal);
            var unnamed = produced.Except(declared)
                .Where(x => !announced.Contains(x))
                .OrderBy(x => x, StringComparer.Ordinal).ToList();

            _out.WriteLine($"absentes non nommées par la table : {unnamed.Count}");

            unnamed.Should().BeEmpty(
                "toute IRI que la taxonomie produit et que l'OWL ne porte pas doit figurer dans la " +
                "table annoncée : c'est la seule source du consommateur qui réécrit les IRI " +
                "(jsboige/CoursIA#4960). Celles que rien ne nomme : " + string.Join(", ", unnamed));
        }
    }
}