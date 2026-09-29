using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Argumentum.AssetConverter.DatasetUpdater;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde #1623 : toute tâche du DatasetUpdater — même désactivée — ne doit
    /// référencer que des colonnes qui existent dans l'en-tête du CSV source.
    ///
    /// Défaut fondateur : « Scenarii cosmetic polish PT register gpt-5.6-sol »
    /// désignait <c>PrimaryField = "pk"</c> et incluait <c>"pk"</c> /
    /// <c>"title_en"</c> — colonnes du corpus Fallacies copiées telles quelles
    /// (commit 3aade349) ; le CSV Scenarii a pour clé « path » et pour titre
    /// anglais « title ». À l'activation,
    /// <c>DataSetInfo.LoadCsvIntoDataTable</c> aurait affecté
    /// <c>Columns["pk"]</c> (null) à <c>PrimaryKey</c> et levé. La garde doit
    /// attraper ce défaut AVANT toute activation (protocole de smoke #202).
    /// </summary>
    public class DatasetUpdaterTaskGuardTests
    {
        /// <summary>
        /// Colonnes référencées par une tâche qui doivent exister dans le CSV
        /// source. Les valeurs vides (PrimaryField par défaut) sont neutres.
        /// </summary>
        internal static List<string> MissingColumns(IEnumerable<string> referenced, HashSet<string> header)
        {
            return referenced.Where(c => !string.IsNullOrEmpty(c) && !header.Contains(c)).ToList();
        }

        private static string ResolveCsvPath(DataSetInfo dataSet)
        {
            // Les chemins Debug des DataSets sont relatifs au répertoire de
            // sortie du converter (six niveaux sous la racine) : leurs "..\"
            // initiaux remontent à la racine du dépôt.
            var parts = dataSet.DebugFilePath.Split(
                new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                StringSplitOptions.RemoveEmptyEntries);
            var first = 0;
            while (first < parts.Length && parts[first] == "..")
            {
                first++;
            }
            return Path.Combine(new[] { TestRepoRoot.Find() }.Concat(parts.Skip(first)).ToArray());
        }

        private static HashSet<string> ReadCsvHeader(string path)
        {
            // StreamReader sans encoding explicite détecte le BOM UTF-8 :
            // sans lui, la première colonne se lirait « ﻿pk » (leçon #1276).
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.Read();
            csv.ReadHeader();
            return new HashSet<string>(csv.HeaderRecord ?? Array.Empty<string>(), StringComparer.Ordinal);
        }

        [Fact]
        public void DatasetUpdater_Tasks_Only_Reference_Existing_Csv_Columns()
        {
            var config = new AssetConverterConfig();
            var rootConfig = new DatasetUpdaterRootConfig();
            rootConfig.DatasetUpdaterConfigs.Should().NotBeEmpty("la garde ne doit pas se vérifier contre zéro tâche");

            var headers = new Dictionary<string, HashSet<string>>();
            var failures = new List<string>();

            foreach (var task in rootConfig.DatasetUpdaterConfigs)
            {
                var dataSet = config.DataSets.FirstOrDefault(d => d.Name == task.SourceDataset);
                if (dataSet == null)
                {
                    failures.Add($"[{task.Name}] dataset inconnu : {task.SourceDataset}");
                    continue;
                }

                var csvName = dataSet.Name;
                if (!headers.TryGetValue(csvName, out var header))
                {
                    var path = ResolveCsvPath(dataSet);
                    if (!File.Exists(path))
                    {
                        failures.Add($"[{task.Name}] CSV source introuvable : {path}");
                        continue;
                    }
                    header = ReadCsvHeader(path);
                    headers[csvName] = header;
                }

                var referenced = new[] { task.PrimaryField }
                    .Concat(task.FieldsToInclude ?? new List<string>())
                    .Concat(task.FieldsToUpdate ?? new List<string>());
                foreach (var missing in MissingColumns(referenced, header))
                {
                    failures.Add($"[{task.Name}] colonne '{missing}' absente de l'en-tête du CSV {task.SourceDataset}");
                }
            }

            failures.Should().BeEmpty(
                "toute tâche du DatasetUpdater, même désactivée, doit viser des colonnes réelles du CSV source — détails : "
                + string.Join(" ; ", failures));
        }

        [Fact]
        public void Missing_Column_Check_Detects_Invented_Columns()
        {
            // Contrôle inverse : le vérificateur lui-même doit rougir sur les
            // colonnes inventées du défaut fondateur (pk / title_en sur Scenarii).
            var scenariiLikeHeader = new HashSet<string> { "path", "catégorie", "titre", "title", "context_pt", "issue_pt", "suggestion_pt" };
            var brokenTaskColumns = new[] { "pk", "title", "title_en", "issue_pt" };

            var missing = MissingColumns(brokenTaskColumns, scenariiLikeHeader);

            missing.Should().BeEquivalentTo(new[] { "pk", "title_en" },
                "seules les colonnes inventées doivent être signalées");
        }
    }
}
