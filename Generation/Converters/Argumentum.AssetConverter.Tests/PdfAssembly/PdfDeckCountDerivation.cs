using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>Derivation of one CardSet's contribution to a document (all values measured, none assumed).</summary>
    public sealed record CardSetPlan(
        string CardSetName, string DataSetName, int SourceRows, int Rscount,
        int Cards, int NbCopies, int Instances, bool HasBack);

    /// <summary>The fully derived expectation for one document: instances, expected pages, and a
    /// human-readable per-CardSet trace (the anti-"bare integer" provenance of every number).</summary>
    public sealed record DocumentPlan(
        string DocumentName, string Format, IReadOnlyList<CardSetPlan> CardSets,
        int CardInstances, int ExpectedPages, string Breakdown);

    /// <summary>
    /// #1187 PDF-level card-count organ — derivation engine. Computes the expected page count of a
    /// <c>CardSetDocumentConfig</c> from the three authored sources ONLY:
    /// <list type="number">
    /// <item><description>the <b>factory config</b> (<c>new AssetConverterConfig()</c> — the C#
    /// initializers ARE the single source of truth, <c>SkipConfigFile=true</c>): document format,
    /// per-CardSet <c>NbCopies</c>, back presence (<c>BackCardSetInfo != null</c>), page size,
    /// header, <c>NbColumns</c>, <c>NoBack</c>;</description></item>
    /// <item><description>the <b>source CSVs</b> via <see cref="HarvestCardIdsCsv"/> with the
    /// CardSet's own <c>CsvFilterField/CsvFilterValues</c> — the reading engine already pinned by
    /// <c>CardSetExpectedCardCountContractTests</c> (#1212);</description></item>
    /// <item><description>the CardPen <b>face template</b> for <c>rscount</c> — the grouping width
    /// the harvest itself uses (<c>expectedImageCount = ceil(rows / rscount)</c>).</description></item>
    /// </list>
    /// No expected page count is ever hard-coded: a fifth number for the same object is exactly
    /// what made #1187 drift. The Memo is fully covered by this chain — its "1 card" is
    /// ceil(176 taxonomy rows / rscount 200), and its document copies come from config
    /// (<c>NbCopies=7</c> in TarotCards today) — the divergence between "Memo=1" (CSV organ) and
    /// "Memo ×7" (matrix) becomes a visible derivation line instead of a silent disagreement.
    /// </summary>
    internal static class PdfDeckCountDerivation
    {
        // Mirrors PrintAndPlayDocument's private MmToPointsFactor (0.1 / 2.54 * 72).
        private const float MmToPoints = 0.1f / 2.54f * 72f;

        /// <summary>Derives the expected page count of <paramref name="doc"/> at the CURRENT
        /// authored state (config + CSV + templates under <paramref name="repoRoot"/>).</summary>
        public static DocumentPlan Derive(AssetConverterConfig config, CardSetDocumentConfig doc, string repoRoot)
        {
            var projectDir = Path.GetFullPath(Path.Combine(
                repoRoot, "Generation", "Converters", "Argumentum.AssetConverter"));
            // The config's relative paths (DebugFilePath, JsonFilePathDebug) are authored against
            // the pipeline's RUNTIME working directory — the build output
            // bin/Debug/net9.0-windows (the csproj's TargetFramework folder), from which six
            // ".." climb back to the repo root. Resolving them against the source project dir
            // would overshoot the repo — this reproduces the production resolution instead.
            var runtimeCwd = Path.Combine(projectDir, "bin", "Debug", "net9.0-windows");
            var cardSetsByName = config.WebBasedGeneratorConfig.CardSets.ToDictionary(c => c.Name, StringComparer.Ordinal);
            var dataSetsByName = config.DataSets.ToDictionary(d => d.Name, StringComparer.Ordinal);

            var plans = new List<CardSetPlan>();
            foreach (var docCardSet in doc.CardSets)
            {
                if (!cardSetsByName.TryGetValue(docCardSet.CardSetName, out var cardSet))
                    throw new InvalidOperationException(
                        $"PdfDeckCountDerivation: document '{doc.DocumentName}' references CardSet '{docCardSet.CardSetName}' which is not defined in WebBasedGeneratorConfig.CardSets.");
                var face = cardSet.FaceCardSetInfo
                    ?? throw new InvalidOperationException(
                        $"PdfDeckCountDerivation: CardSet '{cardSet.Name}' has no FaceCardSetInfo — cannot derive its card count.");
                if (!dataSetsByName.TryGetValue(face.DataSet, out var dataSet))
                    throw new InvalidOperationException(
                        $"PdfDeckCountDerivation: CardSet '{cardSet.Name}' references DataSet '{face.DataSet}' which is not defined.");
                if (string.IsNullOrEmpty(dataSet.DebugFilePath))
                    throw new InvalidOperationException(
                        $"PdfDeckCountDerivation: DataSet '{face.DataSet}' has no DebugFilePath — cannot resolve its CSV.");

                var csvPath = Path.GetFullPath(Path.Combine(runtimeCwd, dataSet.DebugFilePath));
                var csv = new HarvestCardIdsCsv(csvPath);
                // Count data rows through the same filter the harvest applies. The column read is
                // irrelevant (only its row count matters): the filter column when filtering, else
                // the first header column.
                var header = csv.ReadHeader();
                if (header.Count == 0)
                    throw new InvalidOperationException($"PdfDeckCountDerivation: '{csvPath}' has an empty header.");
                var countColumn = face.CsvFilterField ?? header[0];
                var rows = csv.LoadColumn(countColumn, face.CsvFilterField, face.CsvFilterValues?.ToList()).Count;

                // Effective grouping, mirroring the harvest EXACTLY: the template's
                // (rscount, rsstyle) pair, overridden by the config's RowsetNb ONLY when > 0
                // (HarvestManager.cs:379 — RowsetNb=0 on the Scenarii P&P faces means "use the
                // template", not "group 0"), then the card count through the harvest's OWN pure
                // formula (grouping applies iff rscount > 1 AND rsstyle ∈ {bunch, cycle, random}).
                var (templateRscount, rsstyle) = ReadTemplateGrouping(face.JsonFilePathDebug, runtimeCwd, cardSet.Name);
                var rscount = face.RowsetNb > 0 ? face.RowsetNb : templateRscount;
                var cards = HarvestManager.ComputeExpectedImageCount(rows, rscount, rsstyle);
                var instances = cards * docCardSet.NbCopies;

                // Back presence mirrors the production predicate (LocalizationConfig.cs:29,
                // CardSetInfo.GetCardSetDocument): BackCardSetInfo is NEVER null (its property
                // default is an empty CardSetInfo) — a card set has a back iff its back template
                // path is configured; an empty path yields a null back payload at harvest.
                var hasBack = !string.IsNullOrEmpty(cardSet.BackCardSetInfo?.JsonFilePathDebug);

                plans.Add(new CardSetPlan(
                    cardSet.Name, face.DataSet, rows, rscount, cards, docCardSet.NbCopies,
                    instances, hasBack));
            }

            var instanceHasBack = plans.SelectMany(p => Enumerable.Repeat(p.HasBack, p.Instances)).ToList();
            int expectedPages;
            string format;
            switch (doc.DocumentFormat)
            {
                case CardDocumentFormat.AlternateFaceAndBack:
                    format = "AlternateFaceAndBack";
                    expectedPages = PdfCardCountIntegrity.ExpectedPagesAlternateFaceAndBack(instanceHasBack);
                    break;
                case CardDocumentFormat.FacesOnly:
                    format = "FacesOnly";
                    expectedPages = PdfCardCountIntegrity.ExpectedPagesFacesOnly(instanceHasBack.Count);
                    break;
                case CardDocumentFormat.PrintAndPlay:
                    format = "PrintAndPlay";
                    // Mirror Compose: page size resolved by reflection over QuestPDF PageSizes from
                    // the doc's PageSize string (Compose does the same — the type is only known by
                    // name); card size from CardSets[0]; margin hardcoded 0 (pageMarginMm = 0 in
                    // Compose — doc.Padding feeds the inner grid, not the page geometry).
                    var (pageWidth, pageHeight) = GetPageSizePoints(doc.PageSize, doc.DocumentName);
                    var card = doc.CardSets[0].FrontCards;
                    expectedPages = PdfCardCountIntegrity.ExpectedPagesPrintAndPlay(
                        pageWidth, pageHeight,
                        (float)card.WidthMM * MmToPoints, (float)card.HeigthMM * MmToPoints,
                        totalMarginPoints: 0f,
                        hasHeader: !string.IsNullOrEmpty(doc.Header),
                        configuredNbColumns: doc.NbColumns,
                        instanceHasBack: instanceHasBack,
                        noBack: doc.NoBack);
                    break;
                default:
                    throw new NotSupportedException(
                        $"PdfDeckCountDerivation: format {doc.DocumentFormat} of '{doc.DocumentName}' emits one PDF per distinct back art; " +
                        "its page count depends on how back images group at harvest time, which this organ refuses to guess (#1187: no silent fifth number). " +
                        "Derive it explicitly or exclude the document, loudly.");
            }

            var breakdownLines = plans.Select(p =>
                $"{p.CardSetName}: {p.SourceRows} row(s) [DataSet {p.DataSetName}] / rscount {p.Rscount} → {p.Cards} card(s) × {p.NbCopies} copie(s) = {p.Instances} instance(s), " +
                (p.HasBack ? "avec dos" : "SANS dos") + ".");
            var breakdown = $"format {format}, {instanceHasBack.Count} instance(s): " + string.Join(" | ", breakdownLines);

            return new DocumentPlan(doc.DocumentName, format, plans, instanceHasBack.Count, expectedPages, breakdown);
        }

        /// <summary>Resolves a named QuestPDF page size to its (Width, Height) in points, the way
        /// <c>PrintAndPlayDocument.Compose</c> does (reflection over
        /// <c>QuestPDF.Helpers.PageSizes</c> by property name).</summary>
        private static (float Width, float Height) GetPageSizePoints(string pageSizeName, string documentName)
        {
            var property = typeof(QuestPDF.Helpers.PageSizes).GetProperty(pageSizeName)
                ?? throw new InvalidOperationException(
                    $"PdfDeckCountDerivation: PageSize '{pageSizeName}' of '{documentName}' is not a QuestPDF PageSizes member.");
            var value = property.GetValue(null)
                ?? throw new InvalidOperationException(
                    $"PdfDeckCountDerivation: QuestPDF.Helpers.PageSizes.{pageSizeName} returned null for '{documentName}'.");
            return (ReadSizeMember(value, "Width", pageSizeName), ReadSizeMember(value, "Height", pageSizeName));
        }

        /// <summary>Reads Width/Height from a QuestPDF page size — property OR public field
        /// (the PageSize struct of the pinned 2022.12 version exposes them as fields).</summary>
        private static float ReadSizeMember(object value, string member, string pageSizeName)
        {
            var type = value.GetType();
            var prop = type.GetProperty(member);
            if (prop != null) return Convert.ToSingle(prop.GetValue(value));
            var field = type.GetField(member);
            if (field != null) return Convert.ToSingle(field.GetValue(value));
            throw new InvalidOperationException(
                $"PdfDeckCountDerivation: page-size type of '{pageSizeName}' exposes no '{member}' property or field.");
        }

        /// <summary>Reads the (rscount, rsstyle) grouping pair from a CardPen template JSON.
        /// Parsed with <b>Utf8Json</b> — the parser production uses
        /// (<c>CardSetInfo.GetCardSetDocument</c>) — NOT System.Text.Json, whose strict reader
        /// rejects the raw control characters these templates embed inside their CSS strings
        /// (documented in <c>LanguageSpecificCssContractTests</c>). Fail-loud on a missing
        /// template: a guessed grouping width would silently miscount the whole CardSet.</summary>
        private static (int Rscount, string? Rsstyle) ReadTemplateGrouping(string? jsonFilePathDebug, string runtimeCwd, string cardSetName)
        {
            if (string.IsNullOrEmpty(jsonFilePathDebug))
                throw new InvalidOperationException(
                    $"PdfDeckCountDerivation: CardSet '{cardSetName}' has no JsonFilePathDebug — cannot read its rscount.");
            var path = Path.GetFullPath(Path.Combine(runtimeCwd, jsonFilePathDebug));
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"PdfDeckCountDerivation: face template of CardSet '{cardSetName}' not found at '{path}' — rscount is required to derive its card count.", path);
            var template = Utf8Json.JsonSerializer.Deserialize<CardSetDocument>(File.ReadAllBytes(path));
            return (template?.rscount ?? 0, template?.rsstyle);
        }
    }
}
