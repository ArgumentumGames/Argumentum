using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    /// <summary>
    /// DNN web-platform UI strings (issue #457). Source CSV lives at
    /// docs/dnn-localization/dnn-ui-strings.csv with columns:
    /// key, context, source_file, fr, en, ru, pt, es, ar, fa, zh, notes.
    /// "key" is the primary key (e.g. "ui.fallacy.find_out_more", "res.RuleSummary").
    /// "fr" is the canonical source; the 7 target languages (en/ru/pt/es/ar/fa/zh) are
    /// populated by the DatasetUpdater (gpt-5.5).
    ///
    /// DUAL PATH NOTE: the DatasetUpdater engine reads raw CSV headers via a PLAIN
    /// CsvConfiguration (DataSetInfo.GetDictionaryFromCsv — no ClassMap, no
    /// PrepareHeaderForMatch), so FieldsToInclude/FieldsToUpdate/PrimaryField in the task
    /// config use the literal header names (key, fr, en, source_file...). This ClassMap is
    /// exercised by the harvest/validation entity path (CsvBase.LoadFromContent) and by the
    /// DnnUiStringClassMapRegressionTests.
    /// </summary>
    public class DnnUiString : CsvBase<DnnUiString, DnnUiStringClassMap>, ICsvBase
    {
        public string Key { get; set; }

        /// <summary>
        /// Returns Key when present, otherwise a sequential id derived from RowIndex
        /// (mirrors Rule.GetId). The base Id is intentionally left unmapped.
        /// </summary>
        public new string GetId()
        {
            return !string.IsNullOrEmpty(Key) ? Key : $"DnnUi_{RowIndex + 1:D2}";
        }

        public string Context { get; set; }
        public string SourceFile { get; set; }
        public string Fr { get; set; }
        public string En { get; set; }
        public string Ru { get; set; }
        public string Pt { get; set; }
        public string Es { get; set; }
        public string Ar { get; set; }
        public string Fa { get; set; }
        public string Zh { get; set; }
        public string Notes { get; set; }
    }

    public sealed class DnnUiStringClassMap : ClassMap<DnnUiString>
    {
        public DnnUiStringClassMap()
        {
            // Only "key" is required so the CSV loads even before any translation exists;
            // every other column is Optional (MissingFieldFound only logs). "source_file"
            // resolves via PrepareHeaderForMatch (underscore stripped before matching).
            Map(m => m.Key).Name("key");
            Map(m => m.Context).Name("context").Optional();
            Map(m => m.SourceFile).Name("source_file").Optional();
            Map(m => m.Fr).Name("fr").Optional();
            Map(m => m.En).Name("en").Optional();
            Map(m => m.Ru).Name("ru").Optional();
            Map(m => m.Pt).Name("pt").Optional();
            Map(m => m.Es).Name("es").Optional();
            Map(m => m.Ar).Name("ar").Optional();
            Map(m => m.Fa).Name("fa").Optional();
            Map(m => m.Zh).Name("zh").Optional();
            Map(m => m.Notes).Name("notes").Optional();
        }
    }
}
