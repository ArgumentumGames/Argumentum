using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Dnn
{
    /// <summary>
    /// Organe mentions légales /en (pool ai-01 13/10, c.6134164246 grain 1) — le resx NEUTRE
    /// &lt;code&gt;GlobalResources.resx&lt;/code&gt; (fallback anglais, servi sous /en/Terms) doit porter
    /// le bloc d'identification légal (éditeur + SIREN + hébergeur) que le resx fr-FR porte
    /// depuis #1869. Constat fondateur (ai-01, 13/10) : /en/Terms servait le gabarit DNN anglais
    /// SANS éditeur, SIREN ni hébergeur — la version anglaise du site n'avait donc pas de
    /// mentions légales conformes (art. 6 LCEN).
    ///
    /// LE CONTRAT D'IDENTITÉ EST OCTET POUR OCTET : les trois chaînes (raison sociale + adresse
    /// de l'éditeur, SIREN, raison sociale + adresse de l'hébergeur) doivent être IDENTIQUES
    /// entre le resx neutre et le resx fr-FR — mêmes organismes, mêmes chiffres, jamais
    /// retapés. La prose autour est anglaise (LEGAL NOTICE / SITE IDENTIFICATION), le gabarit
    /// DNN existant (AGREEMENT BETWEEN USER AND…) est préservé tel quel : l'insertion est
    /// différentielle, pas un remplacement.
    ///
    /// VECTEUR DE RETOUR COUVERT : une mise à jour de paquet DNN réécrit le resx neutre avec
    /// le gabarit stock (c'est exactement l'état constaté sous /en/Terms) — la garde rougit
    /// nommément sur la chaîne d'identité disparue. Les mentions sont des données publiques
    /// (registre légal), pas des secrets.
    ///
    /// EXTENSION es-ES / pt-BR (pool ai-01 c.6180321584 item 1, 15/10) : les resx es-ES et pt-BR
    /// — committés par #1878 avec le gabarit DNN stock anglais, SANS mentions légales — portent
    /// le même bloc traduit (prose es/pt, identité octet pour octet). NB terminologique : « editor »
    /// EST le terme légal correct en espagnol et en portugais (LSSI-CE art. 2) — le faux-ami
    /// « editor = relecteur » n'existe que côté anglais (revue ai-01 c.6143944905).
    ///
    /// GRAIN SUITE fr (pool ai-01 c.6203990682 item 1, 16/10) : capitales accentuées
    /// (« L’ÉDITEUR » ×5), accord pluriel (« ne s’appliquent »), articles doublés ×2 (dont un à
    /// travers un repli de ligne), insécables avant ; : ! ? de prose (×11). Le détecteur ne voit
    /// que la prose (balises et entités consommées — le CSS inline n'est pas de la prose) et la
    /// chaîne d'identité « SIREN: » est exemptée : parité byte 4 resx (contrat plus fort).
    ///
    /// GRAIN CAPITALES fr (pool ai-01 c.6213886301 item 2, 16/10) : recensement dérivé de TOUS
    /// les tokens majuscules du bloc — 31 mots sans accent / 42 occurrences converties (AFFILIÉES,
    /// HÉBERGEUR ×4, SOCIÉTÉS ×3, ÉLÉMENT(S) ×5, PRÉCISION, FIABILITÉ, DÉLICTUELLE…) + 3
    /// prépositions « A » isolées → « À ». Exclusions LUES EN CONTEXTE, déclarées : « TEL » =
    /// adjectif (« d’un tel dommage »), EXACTITUDE/RESPONSABLE/CONCESSION… = aucun accent en
    /// français, MARTEL/PARIS/ARGUMENTUM = noms propres, SAS/SIREN = sigles. « INFORME » = participe
    /// passé → INFORMÉ (le contexte tranche : verbe « informe » sans accent, participe avec) ;
    /// SUITE 2 (17/10) : accord pluriel → INFORMÉS (verbe ONT), + RÉCLAMATION (balayage
    /// dictionnaire externe — 27 autres candidats lus en contexte = homographes exclus :
    /// OU conjonction ×12, MAIS, SUR, EXPLICITE adj., APPLIQUE verbe, MARQUES/MARCHANDE/
    /// USAGE/TIERCES/CADRE/TENUS noms, DE/DES/DU/LA/LES/NE mots-outils).
    /// Identité byte intouchée (état du grain LCEN #1876/#1883, avant E2).
    ///
    /// GRAIN E2 (pool #458 c.6456588555, décision (a)) : les valeurs TERMS ET PRIVACY du neutre,
    /// es-ES, pt-BR et ru-RU sont REMPLACÉES depuis le fr-FR (gabarit DNN retiré) — les 6
    /// épingles du gabarit qui vivaient ici (ancre AGREEMENT/ACUERDO/ACORDO, guillemets
    /// droits « Linked Sites » et ses traductions) partent avec lui, sans contre-épingle ;
    /// la garde dérivée du fr est <see cref="DnnLegalEnEsPtRuGuardTests"/>. Les épingles
    /// de glossaire LCEN ci-dessous (publisher / editado por / guillemets / nº) restent :
    /// la terminologie arbitrée a été imposée au traducteur.
    /// </summary>
    public class DnnLegalMentionsResxGuardTests
    {
        private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
        private const string FrenchResx = "DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx";
        private const string SpanishResx = "DNNPlatform/App_GlobalResources/GlobalResources.es-ES.resx";
        private const string PortugueseResx = "DNNPlatform/App_GlobalResources/GlobalResources.pt-BR.resx";
        private const string TermsKey = "MESSAGE_PORTAL_TERMS.Text";

        /// <summary>Les trois chaînes d'identité — jamais retapées, extraites du bloc fr-FR #1869.</summary>
        private static readonly string[] IdentityStrings =
        {
            "ARGUMENTUM SAS, 5 B RUE MARTEL, 75010 PARIS 10",
            "SIREN: 922020276",
            "MY INTELLIGENCE AGENCY, 5 B RUE MARTEL, 75010 PARIS 10",
        };

        private static string ReadTermsValue(string resxRelativePath)
        {
            var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
            File.Exists(path).Should().BeTrue($"le resx doit exister dans le checkout ({resxRelativePath})");

            var root = XDocument.Load(path).Root;
            root.Should().NotBeNull("un resx est toujours un XML à racine unique");

            var entry = root!.Elements("data")
                .FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, TermsKey, StringComparison.Ordinal));
            entry.Should().NotBeNull($"la clé {TermsKey} doit exister dans {resxRelativePath}");

            var value = entry!.Element("value")?.Value;
            value.Should().NotBeNullOrWhiteSpace($"la valeur de {TermsKey} ne doit pas être vide dans {resxRelativePath}");
            return value!;
        }

        private static string[] MissingIdentities(string termsValue) =>
            IdentityStrings.Where(s => !termsValue.Contains(s, StringComparison.Ordinal)).ToArray();

        [Fact]
        public void NeutralResx_TermsValue_CarriesTheLegalIdentityBlock()
        {
            var value = ReadTermsValue(NeutralResx);

            MissingIdentities(value).Should().BeEmpty(
                "le resx neutre (fallback /en) doit porter le bloc d'identification légal — " +
                "chaîne manquante = mention légale absente de la version anglaise du site " +
                "(ou paquet DNN ayant réécrit le resx avec le gabarit stock)");
        }

        [Fact]
        public void NeutralResx_LegalNoticeIsInEnglish_HeadedBySiteIdentification()
        {
            var value = ReadTermsValue(NeutralResx);

            value.Should().Contain("LEGAL NOTICE", "l'intitulé du bloc est anglais dans le resx neutre");
            value.Should().Contain("SITE IDENTIFICATION", "le sous-titre d'identification est anglais");
        }

        /// <summary>
        /// Terminologie juridique (revue ai-01 c.6143944905) : l'« éditeur » de la LCEN se dit
        /// <i>publisher</i> en anglais juridique — « edited by » / « Editor of the Site » sont
        /// des faux-amis (un relecteur, un correcteur d'épreuves) et ont été corrigés en
        /// « published by » / « Publisher of the Site ». La garde épingle la forme corrigée ET
        /// l'absence des faux-amis : le vecteur de retour couvert est une ré-édition future du
        /// bloc qui re-réintroduirait le faux-ami (les occurrences historiques sont uniques
        /// dans la valeur TERMS — vérifié avant l'écriture des NotContain).
        /// </summary>
        [Fact]
        public void NeutralResx_UsesPublisherTerminology_NotTheEditorFauxAmis()
        {
            var value = ReadTermsValue(NeutralResx);

            // Le resx replie la désignation sur deux lignes ("Publisher of the" / "Site") : le
            // newline survit au décodage XML mais se rend comme un ESPACE au navigateur. Les
            // assertions portent donc sur la vue servie (newlines normalisés en espaces) —
            // sinon un faux-ami réintroduit avec le même repli échapperait au NotContain.
            var served = value.Replace("\r", string.Empty).Replace("\n", " ");

            served.Should().Contain("is published by:",
                "l'« éditeur » de la LCEN se traduit publisher en anglais juridique " +
                "(revue ai-01 c.6143944905 sur PR #1876)");
            served.Should().Contain("“Publisher of the Site”",
                "la désignation contractuelle du bloc, forme corrigée, guillemets typographiques " +
                "anglais “ ” (grain typographie 15/10)");

            served.Should().NotContain("edited by",
                "faux-ami juridique corrigé : « edited by » = relu/corrigé par, pas « édité » " +
                "au sens de la LCEN");
            served.Should().NotContain("Editor of the Site",
                "faux-ami juridique corrigé : la désignation est « Publisher of the Site »");
        }

        [Fact]
        public void FrenchResx_TermsValue_CarriesTheSameIdentityStrings()
        {
            var value = ReadTermsValue(FrenchResx);

            MissingIdentities(value).Should().BeEmpty(
                "le resx fr-FR (source du bloc, #1869) doit porter les mêmes chaînes d'identité — " +
                "la parité octet pour octet est le contrat qui rend la version EN conforme");
        }

        [Fact]
        public void SpanishResx_TermsValue_CarriesTheLegalIdentityBlock()
        {
            var value = ReadTermsValue(SpanishResx);

            MissingIdentities(value).Should().BeEmpty(
                "le resx es-ES doit porter le bloc d'identification légal — l'activation es-ES du site " +
                "exige des mentions conformes, identité octet pour octet avec le fr-FR");
        }

        [Fact]
        public void SpanishResx_LegalNoticeIsInSpanish_WithCorrectLegalTerms()
        {
            var value = ReadTermsValue(SpanishResx);

            value.Should().Contain("AVISO LEGAL", "l'intitulé du bloc est espagnol dans le resx es-ES");
            value.Should().Contain("IDENTIFICACIÓN DEL SITIO", "le sous-titre d'identification est espagnol");
            value.Should().Contain("está editado por",
                "l'éditeur LCEN se dit editor en droit espagnol (LSSI-CE art. 2) — le terme N'EST PAS " +
                "un faux-ami dans cette langue, contrairement à l'anglais");
            value.Should().Contain("está alojado por", "l'hébergeur : alojado por");
            value.Should().Contain("«Editor del Sitio»",
                "la désignation contractuelle es-ES, guillemets espagnols « » sans espace " +
                "intérieure (grain typographie 15/10)");
            value.Should().Contain("«Proveedor de Alojamiento»",
                "la désignation de l'hébergeur es-ES, guillemets espagnols");
        }

        [Fact]
        public void PortugueseResx_TermsValue_CarriesTheLegalIdentityBlock()
        {
            var value = ReadTermsValue(PortugueseResx);

            MissingIdentities(value).Should().BeEmpty(
                "le resx pt-BR doit porter le bloc d'identification légal — l'activation pt-BR du site " +
                "exige des mentions conformes, identité octet pour octet avec le fr-FR");
        }

        [Fact]
        public void PortugueseResx_LegalNoticeIsInPortuguese_WithCorrectLegalTerms()
        {
            var value = ReadTermsValue(PortugueseResx);

            value.Should().Contain("AVISO LEGAL", "l'intitulé du bloc est portugais dans le resx pt-BR");
            value.Should().Contain("IDENTIFICAÇÃO DO SITE", "le sous-titre d'identification est portugais");
            value.Should().Contain("é editado por",
                "l'éditeur LCEN se rend editor do site en portugais — même note qu'en espagnol : le " +
                "terme n'est PAS un faux-ami dans cette langue");
            value.Should().Contain("é hospedado por", "l'hébergeur pt-BR : hospedado por");
            value.Should().Contain("“Editor do Site”",
                "la désignation contractuelle pt-BR, guillemets typographiques “ ” (grain typographie 15/10)");
            value.Should().Contain("“Provedor de Hospedagem”",
                "la désignation de l'hébergeur pt-BR, guillemets typographiques");
            value.Should().Contain("nº 2004-575",
                "le numéro de la loi en abrégé brésilien « nº » (grain typographie 15/10)");
            value.Should().NotContain("n.º",
                "« n.º » est la graphie pt-PT ; le site cible est pt-BR (#1095)");
        }

        /// <summary>
        /// GRAIN TYPOGRAPHIE (pool ai-01 c.6190391294 item 1, 15/10). Les quatre blocs portaient
        /// des guillemets droits <c>"..."</c> autour des désignations (« Argumentum », éditeur,
        /// hébergeur) et l'URL Légifrance embarquait un <c>jsessionid</c> figé — une URL de
        /// session dans un artefact committé est non canonique par construction. Le grain pose :
        /// fr « … » (guillemets + espace insécable intérieure), es «…» (sans espace,
        /// usage RAE), en/pt “…” (guillemets typographiques), pt « nº » au lieu de « n.º »,
        /// et l'URL canonique <c>https://www.legifrance.gouv.fr/jorf/id/JORFTEXT000000801164</c>
        /// (vérifiée par lecture du contenu : la page sert bien la Loi 2004-575 du 21 juin 2004 —
        /// un sondage HTTP python rend 403 uniformément, anti-bot : l'instrument était aveugle,
        /// le titre lu dans la page est la preuve). L'identité octet pour octet et le gabarit DNN
        /// (zone « AGREEMENT », guillemets droits compris) ne bougent PAS.
        /// </summary>
        [Fact]
        public void AllLanguages_LegifranceUrlIsCanonical_NeverCarriesASessionId()
        {
            var canonical = "https://www.legifrance.gouv.fr/jorf/id/JORFTEXT000000801164";
            foreach (var resx in new[] { NeutralResx, FrenchResx, SpanishResx, PortugueseResx })
            {
                var value = ReadTermsValue(resx);
                value.Should().Contain(canonical,
                    "{0} : le lien LCEN est l'URL canonique Légifrance (même identifiant JORFTEXT, " +
                    "version consolidée courante — le libellé du lien dit « en vigueur »)", resx);
                value.Should().NotContain("jsessionid",
                    "{0} : un identifiant de session figé dans un artefact committé ne peut pas " +
                    "rester — l'URL qu'il décorait est morte à la première expiration de session", resx);
            }
        }

        [Fact]
        public void FrenchResx_EditeurCarriesTheAccentInEveryOccurrence()
        {
            var value = ReadTermsValue(FrenchResx);
            var served = value.Replace("\r", string.Empty).Replace("\n", " ");

            served.Should().Contain("Éditeur du Site détient",
                "la prose courante porte l'accent (la désignation comme le texte)");
            // La désignation est épinglée sur la valeur BRUTE : elle est repliée sur deux lignes
            // dans le resx (« du » + newline + « Site », repli hérité du gabarit d'origine, comme
            // sa sœur anglaise) — la vue servie y montrerait un double espace, artefact du repli.
            // Les échappements   restent visibles en source : l'insécable est invisible.
            value.Should().Contain("« Éditeur du \nSite »",
                "la désignation contractuelle : guillemets français AVEC espaces insécables " +
                "intérieures (\\u00a0 en source, jamais un espace plein)");
            // Ordinal : « Editeur » (sans accent) ne doit plus exister — ni dans la désignation,
            // ni dans la prose. « Éditeur » (U+00C9) ne contient pas « Editeur » : le NotContain
            // ne peut pas passer au travers de l'accent.
            served.Should().NotContain("Editeur",
                "l'orthographe sans accent est un reste de gabarit : 29 occurrences converties");
        }

        /// <summary>
        /// GRAIN SUITE (pool ai-01 c.6203990682 item 1, 16/10) — typographie fr, deuxième passe.
        /// Le bloc portait encore : « L’EDITEUR » sans accent en capitales (×5, sections
        /// LIMITATION/EXCLUSION), un accord fautif (« conditions générales … ne s’applique »),
        /// deux articles doublés (« à la l’Éditeur », « contrôle de la \nl’Éditeur » — la seconde
        /// à travers un repli de ligne) et 11 ponctuations doubles de prose sans insécable
        /// (9 « ; » + 2 « : » — « ! » et « ? » : 0 occurrence mesurée).
        /// </summary>
        [Fact]
        public void FrenchResx_TypoSuite_CapitalsCarryTheAccent_Everywhere()
        {
            var value = ReadTermsValue(FrenchResx);

            // « É » (U+00C9) ≠ « E » en ordinal : le compteur ne peut pas confondre les formes.
            CountOf(value, "L’ÉDITEUR").Should().Be(5,
                "5 occurrences en capitales mesurées au grain (sections LIMITATION DE GARANTIE / " +
                "EXCLUSION) — toute divergence = un retour de gabarit ou une 6e section mal accentuée");
            value.Should().Contain("NI L’ÉDITEUR DU SITE, NI",
                "échantillon des sections capitales : l'accent survit à la mise en majuscules");
            value.Should().NotContain("L’EDITEUR",
                "la forme sans accent en capitales est un reste de gabarit : 5 occurrences converties");
        }

        [Fact]
        public void FrenchResx_TypoSuite_PluralAgreement_AndNoDoubledArticle()
        {
            var value = ReadTermsValue(FrenchResx);
            // Vue normalisée (repli de ligne → espace unique) : la seconde occurrence de l'article
            // doublé vivait À TRAVERS un \n (« de la \nl’Éditeur ») — invisible à un Contain brut.
            var normalized = System.Text.RegularExpressions.Regex.Replace(
                value.Replace("\r", string.Empty).Replace("\n", " "), "[ \t]+", " ");

            normalized.Should().Contain("ne s’appliquent qu’au site",
                "accord pluriel : le sujet est « les conditions générales d’utilisation »");
            value.Should().NotContain("s’applique qu’au",
                "l'accord fautif ne doit plus exister nulle part (ordinal : « s’appliquent » ne " +
                "contient pas « s’applique qu’au »)");
            normalized.Should().NotContain("la l’Éditeur",
                "article doublé corrigé ×2 — dont une occurrence à travers le repli de ligne, " +
                "que seule la vue normalisée voit");
            normalized.Should().Contain("contrôle de l’Éditeur du Site",
                "la phrase de la 2e occurrence, après suppression de « la » (repli conservé)");
        }

        /// <summary>
        /// En français, la ponctuation double (; : ! ?) est précédée d'une espace insécable —
        /// dans la PROSE seulement : le détecteur consomme les balises HTML (le CSS inline
        /// « border:1px solid black; » vit dans les attributs, pas dans la prose) et les entités
        /// (&amp;amp;nbsp; et consœurs). Les chaînes d'identité sont exemptées AVANT détection :
        /// « SIREN: 922020276 » est byte-parity dans les 4 resx (contrat #1869/#1883), plus fort
        /// que la convention typographique fr — imposer l'insécable fr aux blocs en/es/pt
        /// contredirait le grain #1886 (chaque langue suit SA typographie).
        /// </summary>
        [Fact]
        public void FrenchResx_TypoSuite_NbspBeforeProseDoublePunctuation()
        {
            var offenders = ProseDoublePunctuationWithoutNbsp(ReadTermsValue(FrenchResx));

            offenders.Should().BeEmpty(
                "11 ponctuations de prose corrigées au grain (9 « ; » + 2 « : » ; les 3 « : » " +
                "déjà insécables de #1886 et le SIREN exempté passent) — contrevenantes : " +
                $"{string.Join(" | ", offenders.Take(3))}{(offenders.Length > 3 ? " …" : "")}");
        }

        /// <summary>
        /// Contrôle inverse du détecteur typographique : il doit FLAGUER chaque cas positif
        /// synthétique (espace pleine avant ; — et les bras ! / ?, qui comptent 0 occurrence
        /// dans le corpus : un bras jamais témoins est un bras mort) et SAUTER tag CSS, entité,
        /// insécable déjà présente et chaîne d'identité. Sans ce témoin, un vert sur le fichier
        /// réel pourrait n'être que la cécité du détecteur.
        /// </summary>
        [Fact]
        public void Witness_TypoSuiteDetector_FlagsEverySyntheticPositive_AndSkipsTagsEntitiesIdentity()
        {
            const string synthetic =
                "<p style=\"border:1px solid black;\">mot ; suite</p>" +
                "<p>état\u00a0: déjà correct</p>" +
                "<p>entité&nbsp; conservée</p>" +
                "<p>SIREN: 922020276</p>" +
                "<p>cri ! question ?</p>";

            var offenders = ProseDoublePunctuationWithoutNbsp(synthetic);

            var joined = string.Join(" | ", offenders);
            offenders.Should().HaveCount(3,
                "exactement les 3 positifs : « mot ; », « cri ! », « question ? » — le « : » du CSS " +
                "est dans la balise, l'entité et l'insécable sont reconnus, le SIREN est exempté " +
                "comme chaîne d'identité (pas comme mot)");
            joined.Should().Contain("mot ;", "le cas positif canonique : espace pleine avant ;");
            joined.Should().Contain("cri !", "témoin du bras « ! » (0 occurrence corpus)");
            joined.Should().Contain("question ?", "témoin du bras « ? » (0 occurrence corpus)");
        }

        private static int CountOf(string haystack, string needle) =>
            System.Text.RegularExpressions.Regex.Matches(
                haystack, System.Text.RegularExpressions.Regex.Escape(needle)).Count;

        /// <summary>Compteur à frontières de mot (lettres latines + accentuées) — le grain
        /// capitales l'exige : « ÉLÉMENT » est préfixe de « ÉLÉMENTS », un compteur de
        /// sous-chaîne lirait 5 au lieu de 2 (mesuré : le premier passage a rougi là-dessus).</summary>
        private static int CountWord(string haystack, string word) =>
            System.Text.RegularExpressions.Regex.Matches(
                haystack,
                "(?<![A-Za-zÀ-ÿ])" + System.Text.RegularExpressions.Regex.Escape(word) + "(?![A-Za-zÀ-ÿ])").Count;

        /// <summary>
        /// Détecteur : liste les ponctuations doubles (; : ! ?) de PROSE non précédées d'une
        /// espace insécable (U+00A0). Balises &lt;…&gt; et entités &amp;…; consommées ; chaînes
        /// d'identité retirées avant classification (parité byte 4 resx).
        /// </summary>
        private static string[] ProseDoublePunctuationWithoutNbsp(string termsValue)
        {
            foreach (var id in IdentityStrings)
                termsValue = termsValue.Replace(id, string.Empty, StringComparison.Ordinal);

            var offenders = new System.Collections.Generic.List<string>();
            var i = 0;
            var inTag = false;
            char prev = '\0';
            while (i < termsValue.Length)
            {
                var c = termsValue[i];
                if (!inTag && c == '<') { inTag = true; i++; prev = '\0'; continue; }
                if (inTag)
                {
                    if (c == '>') inTag = false;
                    i++; continue;
                }
                if (c == '&')
                {
                    var m = System.Text.RegularExpressions.Regex.Match(
                        termsValue.Substring(i), "^&#?\\w+;");
                    if (m.Success) { i += m.Length; prev = '\0'; continue; }
                }
                if (";:!?".IndexOf(c) >= 0)
                {
                    if (prev != '\u00a0')
                        offenders.Add(Snippet(termsValue, i));
                    prev = c; i++; continue;
                }
                prev = c; i++;
            }
            return offenders.ToArray();
        }

        /// <summary>Contexte lisible autour d'une position (replis et insécables rendus visibles).</summary>
        private static string Snippet(string s, int pos)
        {
            var start = Math.Max(0, pos - 18);
            var len = Math.Min(30, s.Length - start);
            return s.Substring(start, len)
                .Replace("\r", string.Empty).Replace("\n", "\\n").Replace("\u00a0", "␣");
        }

        [Fact]
        public void NeutralResx_UsesEnglishCurlyQuotes()
        {
            var value = ReadTermsValue(NeutralResx);

            value.Should().Contain("“Argumentum”",
                "les citations du nom du site en anglais prennent les guillemets typographiques “ ”");
            value.Should().Contain("“Host”", "la désignation de l'hébergeur, guillemets typographiques");
        }

        [Fact]
        public void SpanishResx_UsesSpanishGuillemets()
        {
            var value = ReadTermsValue(SpanishResx);

            value.Should().Contain("«Argumentum»",
                "espagnol : guillemets « » SANS espace intérieure (usage RAE, contrairement au français)");
        }

        [Fact]
        public void PortugueseResx_UsesCurlyQuotes()
        {
            var value = ReadTermsValue(PortugueseResx);

            value.Should().Contain("“Argumentum”",
                "portugais brésilien : guillemets typographiques “ ” (usage brésilien courant)");
        }

        /// <summary>
        /// GRAIN CAPITALES (pool ai-01 c.6213886301 item 2, 16/10) — recensement dérivé du bloc
        /// entier : chaque mot majuscule dont l'orthographe française porte un accent doit
        /// l'avoir. 31 mots / 42 occurrences + 3 prépositions « A » isolées. SUITE 2 (17/10) :
        /// + RÉCLAMATION (l'oubli du classement manuel, attrapé par le balayage dictionnaire
        /// EXTERNE — le lexique tiré du resx seul ne contient pas « réclamation ») ; INFORMÉ
        /// singulier → 0 (l'occurrence est devenue INFORMÉS, accord porté par son propre fait).
        /// Les attendus sont
        /// les comptes MESURÉS post-grain dans la valeur TERMS (⚠️ le « DONNÉES » préexistant
        /// du fichier vit dans la clé License.Text, hors bloc — compté fichier-entier il
        /// ferait attendre 2 à tort ; l'invariant reste la disparition des formes sans accent).
        /// </summary>
        [Fact]
        public void FrenchResx_CapitalsSuite_EveryUppercaseWordCarriesItsAccent()
        {
            var value = ReadTermsValue(FrenchResx);

            foreach (var (bare, accented, expected) in UppercaseAccentMap)
            {
                CountWord(value, bare).Should().Be(0,
                    "{0} : la forme majuscule sans accent est un reste de gabarit — " +
                    "en français soigné, les capitales portent leurs accents", bare);
                CountWord(value, accented).Should().Be(expected,
                    "{0} : compte mesuré post-grain (préexistant + convertis) — toute divergence " +
                    "est un retour de gabarit ou une sur-conversion. Frontières de mot : " +
                    "ÉLÉMENT ⊂ ÉLÉMENTS, un compteur de sous-chaîne lirait 5 au lieu de 2", accented);
            }

            // Les 3 prépositions isolées : épinglées en phrase (le « A » isolé du français est
            // toujours la préposition « à », jamais l'article — les 3 contextes ont été lus).
            value.Should().Contain("LIMITÉE À L’ÉDITEUR",
                "préposition à en capitales : accord de licence");
            value.Should().Contain("RELATIF À LA VÉRIFICATION",
                "préposition à en capitales : consentement relatif");
            value.Should().Contain("À sa demande, vous",
                "la préposition ouvrant la clause d'indemnisation (nommée par le dispatch)");
        }

        /// <summary>
        /// SUITE 2 (17/10, ai-01 relit le diff de #1892) : accord du participe. Le bloc portait
        /// « L'ÉDITEUR DU SITE, OU L'HÉBERGEUR ONT ÉTÉ INFORMÉ » — singulier derrière le verbe
        /// pluriel ONT régi par le sujet coordonné. L'arbitrage aligne le participle :
        /// INFORMÉS. (Un dictionnaire ne voit pas cet accord : la carte ci-dessus attend
        /// INFORMÉ singulier = 0 — c'est CE fait qui porte la raison.)
        /// ⚠️ ancré sur « ÉTÉ INFORMÉS » : le fichier brut porte un saut de ligne entre
        /// ONT et ÉTÉ, un Contains("ONT ÉTÉ…") serait faux-négatif de fabrication.
        /// </summary>
        [Fact]
        public void FrenchResx_Informes_AgreesWithPluralVerb()
        {
            var value = ReadTermsValue(FrenchResx);

            value.Should().Contain("ÉTÉ INFORMÉS DE LA POSSIBILITÉ",
                "sujet coordonné + verbe pluriel (ONT) ⇒ participe pluriel INFORMÉS — " +
                "accord nommé par ai-01 au diff de #1892");
            CountWord(value, "INFORMÉ").Should().Be(0,
                "le singulier ne survit nulle part : sa seule occurrence est devenue INFORMÉS");
        }

        /// <summary>
        /// Contrôle inverse du grain capitales : la boucle de
        /// <see cref="FrenchResx_CapitalsSuite_EveryUppercaseWordCarriesItsAccent"/> doit VOIR
        /// une forme sans accent si elle revient — un vert sur le fichier réel ne prouve rien
        /// sans ce témoin (les asserts sont des Contain/CountOf ordinaires : ce témoin prouve
        /// que la carte couvre bien les mots qu'elle prétend couvrir).
        /// </summary>
        [Fact]
        public void Witness_CapitalsSuiteMap_FlagsEverySyntheticBareUppercase()
        {
            const string synthetic =
                "PAR L’HEBERGEUR ET LEURS SOCIETES MÈRES OU AFFILIEES, LA PRECISION ET LA " +
                "FIABILITE, ONT ETE INFORME, DONT BENEFICE ET DONNEES, PROCEDURE DE RECLAMATION";

            var seen = UppercaseAccentMap
                .Where(m => synthetic.Contains(m.Bare, StringComparison.Ordinal))
                .Select(m => m.Bare).ToArray();

            seen.Should().BeEquivalentTo(new[]
                { "HEBERGEUR", "SOCIETES", "AFFILIEES", "PRECISION", "FIABILITE", "ETE", "INFORME", "BENEFICE", "DONNEES", "PROCEDURE", "RECLAMATION" },
                "les 11 formes injectées doivent chacune être couvertes par la carte — une " +
                "absence ici = un mot que la garde ne saurait pas voir revenir. RECLAMATION : " +
                "l'oubli nommé par ai-01 au diff de #1892 (suite 2, 17/10)");
        }

        /// <summary>Carte mesurée du grain capitales : (forme sans accent, forme accentuée, compte post-grain).</summary>
        private static readonly (string Bare, string Accented, int Expected)[] UppercaseAccentMap =
        {
            ("ACCEDER", "ACCÉDER", 1), ("AFFILIEES", "AFFILIÉES", 3),
            ("BENEFICE", "BÉNÉFICE", 1), ("COMPETENT", "COMPÉTENT", 1),
            ("CONCEDANTS", "CONCÉDANTS", 1), ("CONFIDENTIALITE", "CONFIDENTIALITÉ", 1),
            ("CREER", "CRÉER", 1), ("DELICTUELLE", "DÉLICTUELLE", 1),
            ("DEPOSEES", "DÉPOSÉES", 1), ("DISPONIBILITE", "DISPONIBILITÉ", 1),
            ("DIVISIBILITE", "DIVISIBILITÉ", 1), ("DONNEES", "DONNÉES", 1),
            ("ELEMENT", "ÉLÉMENT", 2), ("ELEMENTS", "ÉLÉMENTS", 3),
            ("EMPLOYES", "EMPLOYÉS", 1), ("ETE", "ÉTÉ", 1),
            ("FIABILITE", "FIABILITÉ", 1), ("HEBERGEUR", "HÉBERGEUR", 4),
            ("INCAPACITE", "INCAPACITÉ", 1), ("INFORME", "INFORMÉ", 0),
            ("INTEGRALITE", "INTÉGRALITÉ", 1), ("LIMITEE", "LIMITÉE", 2),
            ("POSSIBILITE", "POSSIBILITÉ", 1), ("PRECISION", "PRÉCISION", 1),
            ("PROCEDURE", "PROCÉDURE", 1), ("PROPRIETE", "PROPRIÉTÉ", 1),
            ("RECLAMATION", "RÉCLAMATION", 1),
            ("RESILIATION", "RÉSILIATION", 1), ("RESPONSABILITE", "RESPONSABILITÉ", 1),
            ("SOCIETES", "SOCIÉTÉS", 3), ("VERIFICATION", "VÉRIFICATION", 1),
        };

        /// <summary>
        /// Contrôle inverse : l'instrument doit VOIR une absence. Un resx synthétique portant le
        /// gabarit DNN stock (état constaté sous /en/Terms avant le correctif) rapporte les TROIS
        /// chaînes manquantes — sans ce témoin, un vert sur le fichier réel pourrait n'être que
        /// la cécité du détecteur.
        /// </summary>
        [Fact]
        public void Witness_StockDnnTemplate_ReportsAllThreeIdentitiesMissing()
        {
            const string stockTemplate =
                "<root><data name=\"MESSAGE_PORTAL_TERMS.Text\" xml:space=\"preserve\">" +
                "<value>&lt;div align=\"left\"&gt;&lt;p&gt;&lt;span class=\"SubHead\"&gt;" +
                "AGREEMENT BETWEEN USER AND [Portal:PortalName]&lt;/span&gt;&lt;p&gt;" +
                "The [Portal:PortalName] Web Site is offered to you conditioned on your acceptance." +
                "&lt;/p&gt;&lt;/div&gt;</value></data></root>";

            var value = XDocument.Parse(stockTemplate).Root!.Elements("data")
                .First(d => d.Attribute("name")!.Value == TermsKey).Element("value")!.Value;

            MissingIdentities(value).Should().BeEquivalentTo(IdentityStrings,
                "le gabarit DNN stock ne porte AUCUNE chaîne d'identité — c'est l'état constaté " +
                "sous /en/Terms avant le correctif, et le détecteur doit le voir ainsi");
        }
    }
}
