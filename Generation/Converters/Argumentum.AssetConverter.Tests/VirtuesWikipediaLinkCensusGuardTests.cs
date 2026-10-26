using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Organe de prévention dérivé du recensement (#458 grain 3, dispatch ai-01 c.6090687351 ;
    /// mesure fondatrice : PR #1851 / <c>docs/corpus/458-v23-g40-virtues-wikipedia-deadlinks-2026-10-10.md</c>).
    /// Le 10/10/2026, le recensement a mesuré sur master **246 cellules <c>link_*</c> mortes** (204 URL
    /// distinctes, dont 193 n'ont <b>jamais existé</b> — fabrication à la génération, pas du link rot).
    /// #1851 vide les 240 cellules et réécrit les 6 restantes ; cette garde verrouille la porte derrière :
    /// <b>toute valeur <c>link_*</c> wikipedia du corpus doit figurer dans la carte committée
    /// <c>Assets/VirtuesLinkCensus/virtues-wikipedia-link-census.json</c> avec un verdict</b> (779 entrées :
    /// 774 au recensement + 5 réécritures #1851), et le budget de cellules mortes brûle vers 0.
    /// </summary>
    /// <remarks>
    /// <para><b>Clés = chaînes brutes</b> du CSV, comparaison <see cref="StringComparer.Ordinal"/>, aucun
    /// normaliseur. Conséquence assumée (fail-closed) : une URL ré-encodée (p. ex. <c>%C3%A9</c> vs
    /// <c>é</c> — les deux formes coexistent dans le corpus mesuré) compte comme une nouvelle clé ⇒ la
    /// garde passe au rouge ⇒ il faut la sonder avant de la committer. Un normaliseur qui ramènerait le
    /// motif cherché à autre chose rendrait le détecteur muet par construction.</para>
    /// <para><b>Périmètre = règle d'hôte</b> : URI absolue dont l'hôte est <c>wikipedia.org</c> ou finit
    /// par <c>.wikipedia.org</c>. Les 31 valeurs non-wikipedia du corpus (cairn.info, halshs, LSE blogs…)
    /// sont hors périmètre, déclaré ; le miroir wayback de #1851 (hôte <c>web.archive.org</c>) l'est
    /// <b>par construction</b> — une règle par sous-chaîne l'aurait attrapé à tort.</para>
    /// <para><b>Budget 0 = burn-down refermé</b> (« une exclusion doit mourir avec sa raison ») : la garde
    /// fut introduite à 246 (état master, #1851 non mergé) pour rester verte dans les deux ordres de merge ;
    /// #1851 étant mergé (10/10/2026, `059f219f`), le grain de suivi referme le budget à <b>0</b> — toute
    /// cellule <c>link_*</c> wikipedia morte qui revient fait maintenant rouge directement.</para>
    /// <para><b>Planchers + sentinelles</b> contre le vert vacue : un lecteur d'en-tête cassé ou une carte
    /// non chargée rend 0 — les planchers (distincts / cellules / entrées de carte) l'attrapent, et les
    /// sentinelles exigent qu'une URL vivante et une morte connues résolvent avec leur verdict attendu
    /// (un instrument incapable de voir le cas positif n'a aucun compte à rendre).</para>
    /// <para><b>Régénération</b> : re-sonder selon la discipline du recensement (redirects suivis,
    /// 3 retries, témoin <c>fr:Sophisme</c> toutes les 40 URL, fail-closed — jamais « tout mort » sur
    /// un incident réseau), puis régénérer la carte depuis la mesure. Ne jamais éditer la carte à la main.</para>
    /// </remarks>
    public class VirtuesWikipediaLinkCensusGuardTests
    {
        /// <summary>Sentinelle vivante : article stable du corpus, sonde au 200 au recensement.</summary>
        private const string AliveSentinel = "https://fr.wikipedia.org/wiki/Syllogisme";

        /// <summary>Sentinelle morte : mesurée 404, classe « n'a jamais existé » (#1851 §6-7).</summary>
        private const string DeadSentinel = "https://fr.wikipedia.org/wiki/Raisonnement_inductif";

        /// <summary>
        /// Budget de cellules mortes. 0 depuis le merge de #1851 (10/10/2026, grain de suivi) : les 246
        /// cellules mortes mesurées au recensement ont été vidées (240) ou réécrites (6). Introduit à 246
        /// pour rester vert dans les deux ordres de merge — refermé dès #1851 sur master.
        /// </summary>
        private const int DeadCellBudget = 0;

        /// <summary>Plancher anti-vacue : états mesurés 774 distincts (master) / 575 (post-#1851).</summary>
        private const int DistinctUrlFloor = 500;

        /// <summary>Plancher anti-vacue : états mesurés 1 139 cellules (master) / 898 (post-#1851).</summary>
        private const int WikiCellFloor = 800;

        /// <summary>Plancher anti-vacue : la carte committée porte 779 entrées.</summary>
        private const int MapEntryFloor = 700;

        private static string VirtuesCsv => Path.Combine(
            TestRepoRoot.Find(), "Cards", "Fallacies", "Argumentum Virtues - Taxonomy.csv");

        private static string CensusMapPath => Path.Combine(
            AppContext.BaseDirectory, "Assets", "VirtuesLinkCensus", "virtues-wikipedia-link-census.json");

        [Fact]
        public void EveryWikipediaLinkOfTheVirtuesCorpusIsCoveredByTheCensusMap()
        {
            var verdicts = LoadCensusVerdicts();
            var cells = LoadLinkCells();

            var inScope = cells.Where(c => IsWikipediaHostUrl(c.Value)).ToList();

            // Planchers : une périmètre muet (lecteur cassé, règle d'hôte faussée) rend ~0.
            var distinctUrls = inScope.Select(c => c.Value).Distinct(StringComparer.Ordinal).Count();
            distinctUrls.Should().BeGreaterThanOrEqualTo(DistinctUrlFloor,
                because: "un compte proche de zéro signifie que la lecture du CSV ou la règle de périmètre est cassée ; " +
                         $"états mesurés : 774 URL distinctes sur master, 575 après #1851 — lu : {distinctUrls}");
            inScope.Count.Should().BeGreaterThanOrEqualTo(WikiCellFloor,
                because: $"états mesurés : 1 139 cellules wikipedia sur master, 898 après #1851 — lu : {inScope.Count}");

            // Présence : toute URL wikipedia du corpus doit avoir un verdict dans la carte.
            var missing = inScope.Where(c => !verdicts.ContainsKey(c.Value)).ToList();
            missing.Should().BeEmpty(
                because: "toute valeur link_* wikipedia du corpus doit figurer dans la carte du recensement " +
                         "(chaîne brute, comparaison ordinale) — une URL absente n'a jamais été sondée. " +
                         $"Manquantes : {FormatCells(missing.Take(5))}{(missing.Count > 5 ? " …" : "")}");

            // Burn-down : les cellules mortes ne peuvent que décroître, jamais revenir.
            var deadCells = inScope.Where(c => verdicts.TryGetValue(c.Value, out var verdict) && verdict == "dead").ToList();
            deadCells.Count.Should().BeLessThanOrEqualTo(DeadCellBudget,
                because: $"burn-down refermé à 0 depuis le merge de #1851 (246 cellules mortes vidées ou réécrites) " +
                         $"— lu : {deadCells.Count}. " +
                         $"Exemples : {FormatCells(deadCells.Take(3))}{(deadCells.Count > 3 ? " …" : "")}");
        }

        [Fact]
        public void CensusMap_SentinelsResolveWithTheirExpectedVerdicts()
        {
            var verdicts = LoadCensusVerdicts();

            verdicts.Count.Should().BeGreaterThanOrEqualTo(MapEntryFloor,
                because: $"la carte committée porte 779 entrées (774 recensement + 5 réécritures #1851) — lue : {verdicts.Count} ; " +
                         "une carte non chargée ou vidée par un éditeur doit échouer ici, pas compter pour vert");

            verdicts.Should().ContainKey(AliveSentinel).WhoseValue.Should().Be("alive",
                because: "sentinelle vivante : la carte doit résoudre le cas positif, sinon son « mort » ne prouve rien");
            verdicts.Should().ContainKey(DeadSentinel).WhoseValue.Should().Be("dead",
                because: "sentinelle morte : une carte où plus rien n'est mort n'est plus une mesure mais une édiction");
        }

        /// <summary>
        /// Charge les verdicts de la carte du recensement. Clés brutes, comparaison
        /// <see cref="StringComparer.Ordinal"/> — aucun normaliseur (voir les remarques de la classe).
        /// </summary>
        private static IReadOnlyDictionary<string, string> LoadCensusVerdicts()
        {
            File.Exists(CensusMapPath).Should().BeTrue(
                because: $"la carte du recensement doit être copiée vers la sortie (glob csproj 'Assets\\**\\*' " +
                         $"en CopyToOutputDirectory) — attendue à '{CensusMapPath}'");

            using var document = JsonDocument.Parse(File.ReadAllBytes(CensusMapPath));
            var urls = document.RootElement.GetProperty("urls");
            var verdicts = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var property in urls.EnumerateObject())
            {
                var verdict = property.Value.GetProperty("verdict").GetString();
                verdict.Should().NotBeNull(because: $"l'entrée '{property.Name}' de la carte doit porter un verdict");
                verdicts[property.Name] = verdict!;
            }

            return verdicts;
        }

        /// <summary>
        /// Toutes les cellules <c>link*</c> non vides du CSV Virtues, découvertes depuis l'en-tête
        /// (une colonne <c>link_*</c> ajoutée demain entre automatiquement dans le périmètre de la garde).
        /// </summary>
        private static List<(string Pk, string Column, string Value)> LoadLinkCells()
        {
            var csv = new HarvestCardIdsCsv(VirtuesCsv);
            var linkColumns = csv.ReadHeader()
                .Where(h => h == "link" || h.StartsWith("link_", StringComparison.Ordinal))
                .ToList();
            linkColumns.Should().NotBeEmpty(because: "le CSV Virtues doit porter ses colonnes link_*");

            var pks = csv.LoadColumn("pk");
            var cells = new List<(string, string, string)>();
            foreach (var column in linkColumns)
            {
                var values = csv.LoadColumn(column);
                values.Count.Should().Be(pks.Count,
                    because: $"la colonne '{column}' doit couvrir les mêmes rangées que pk");
                for (var i = 0; i < values.Count; i++)
                {
                    if (values[i].Length > 0)
                    {
                        cells.Add((pks[i], column, values[i]));
                    }
                }
            }

            return cells;
        }

        /// <summary>Règle de périmètre par hôte — la même que celle qui a construit la carte.</summary>
        private static bool IsWikipediaHostUrl(string value) =>
            Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Host == "wikipedia.org" || uri.Host.EndsWith(".wikipedia.org", StringComparison.Ordinal));

        private static string FormatCells(IEnumerable<(string Pk, string Column, string Value)> cells) =>
            string.Join(", ", cells.Select(c => $"pk {c.Pk}/{c.Column}: {c.Value}"));
    }
}
