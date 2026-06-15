using Argumentum.AssetConverter.Entities;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Regression tests for the real CsvHelper ClassMaps of the entity types that had
    /// historical mapping bugs (Rule.GetId() empty, ArgumentVirtue.Pk unmapped, Scenario
    /// accented FR headers, Virtue pk→Id+Pk). Fallacy already has coverage elsewhere.
    ///
    /// These exercise the REAL load path — <see cref="CsvBase{T,TMap}.LoadFromContent"/> —
    /// which registers the ClassMap, applies PrepareHeaderForMatch (diacritics/underscore/
    /// hyphen/space stripping + lowercasing) so accented French headers resolve, treats
    /// MissingFieldFound as a non-fatal log, and assigns 0-based RowIndex post-load.
    ///
    /// Additive only: no existing test or production code is modified. Inline CSV keeps
    /// each test self-contained (no external fixture files).
    /// </summary>
    public class EntityClassMapRegressionTests
    {
        // ───────────────────────────── Rule ─────────────────────────────

        [Fact]
        public void Rule_LoadFromContent_MapsEveryColumnToItsProperty()
        {
            var csv =
                "pk,Text,Text_en,Text_ru,Text_pt,Text_es,Text_ar,Text_fa,Text_zh,print_and_play\n" +
                "Rules_01,Règle One,Rule One,Правило,Regra One,Regla,قاعدة,قانون,规则,yes\n";

            var rules = Rule.LoadFromContent(csv);

            rules.Should().ContainSingle();
            var r = rules[0];
            r.Pk.Should().Be("Rules_01");
            r.Text.Should().Be("Règle One");
            r.Text_en.Should().Be("Rule One");
            r.Text_ru.Should().Be("Правило");
            r.Text_pt.Should().Be("Regra One");
            r.Text_es.Should().Be("Regla");
            r.Text_ar.Should().Be("قاعدة");
            r.Text_fa.Should().Be("قانون");
            r.Text_zh.Should().Be("规则");
            r.PrintAndPlay.Should().Be("yes");
        }

        /// <summary>
        /// Historical bug (CLAUDE.md): Rule.GetId() used to return string.Empty.
        /// It now returns Pk when present.
        /// </summary>
        [Fact]
        public void Rule_GetId_ReturnsPkWhenPresent()
        {
            var csv = "pk,Text,Text_en,Text_ru,Text_pt\nRules_42,T,T,T,T\n";

            var rules = Rule.LoadFromContent(csv);

            rules[0].GetId().Should().Be("Rules_42");
        }

        /// <summary>
        /// Historical bug (CLAUDE.md): Rule.GetId() used to return string.Empty when Pk
        /// was absent. It now generates a sequential id from RowIndex: Rules_01, Rules_02…
        /// </summary>
        [Fact]
        public void Rule_GetId_ReturnsSequentialIdFromRowIndex_WhenPkAbsent()
        {
            // Pk column omitted entirely (it is Optional in RuleClassMap).
            var csv =
                "Text,Text_en,Text_ru,Text_pt\n" +
                "A,A,A,A\n" +
                "B,B,B,B\n" +
                "C,C,C,C\n";

            var rules = Rule.LoadFromContent(csv);

            rules.Should().HaveCount(3);
            rules[0].Pk.Should().BeNull();
            rules[0].GetId().Should().Be("Rules_01");
            rules[1].GetId().Should().Be("Rules_02");
            rules[2].GetId().Should().Be("Rules_03");
        }

        /// <summary>
        /// The localized Rule columns (es/ar/fa/zh) and print_and_play are Optional in the
        /// ClassMap. A CSV header set that omits them must parse without throwing and leave
        /// those properties null (MissingFieldFound only logs).
        /// </summary>
        [Fact]
        public void Rule_OptionalLocalizedColumns_AbsentDoesNotThrow()
        {
            var csv = "pk,Text,Text_en,Text_ru,Text_pt\nRules_01,T,T,T,T\n";

            var act = () => Rule.LoadFromContent(csv);

            act.Should().NotThrow();
            var r = act().Should().Subject.First();
            r.Pk.Should().Be("Rules_01");
            r.Text_es.Should().BeNull();
            r.Text_ar.Should().BeNull();
            r.Text_fa.Should().BeNull();
            r.Text_zh.Should().BeNull();
            r.PrintAndPlay.Should().BeNull();
        }

        // All non-Optional ArgumentVirtue/Virtue columns (FR + meta). The localized
        // en/ru/pt/es members are .Optional() so they may be omitted from the header.
        private const string ArgumentVirtueFrMetaHeader =
            "pk,path,depth,decimal_path_padded,family_fr,subfamily_fr,subsubfamily_fr," +
            "title_fr,description_fr,remark_fr,link_fr,family_fr_camelcase,depth_max4,card,update,locked";

        // ──────────────────────── ArgumentVirtue ────────────────────────

        /// <summary>
        /// Historical bug (CLAUDE.md): ArgumentVirtueClassMap did not map the Id/pk column.
        /// It now maps pk → Pk, and GetId() returns Pk.
        /// </summary>
        [Fact]
        public void ArgumentVirtue_LoadFromContent_MapsPkColumnToPkProperty()
        {
            var csv =
                ArgumentVirtueFrMetaHeader + "\n" +
                "1.1,1.1,1,1.1,Famille,Sub,Subsub,Titre,Description,Remark,Link,FamilleCc,1,1,,\n";

            var virtues = ArgumentVirtue.LoadFromContent(csv);

            virtues.Should().ContainSingle();
            var v = virtues[0];
            v.Pk.Should().Be("1.1");
            v.Path.Should().Be("1.1");
            v.Depth.Should().Be(1);
            v.FamilyFr.Should().Be("Famille");
            v.TitleFr.Should().Be("Titre");
            v.DescriptionFr.Should().Be("Description");
            v.GetId().Should().Be("1.1");
        }

        [Fact]
        public void ArgumentVirtue_OptionalLocalizedColumns_AbsentDoesNotThrow()
        {
            // Only FR + meta columns provided; en/ru/pt/es (all Optional) omitted from the
            // header. HeaderValidated passes (Optional members are not required), and the
            // localized properties stay null.
            var csv =
                ArgumentVirtueFrMetaHeader + "\n" +
                "1.1,1.1,1,1.1,Famille,Sub,Subsub,Titre,Description,Remark,Link,FamilleCc,1,1,,\n";

            var act = () => ArgumentVirtue.LoadFromContent(csv);

            act.Should().NotThrow();
            var v = act().Should().Subject.First();
            v.Pk.Should().Be("1.1");
            v.FamilyEn.Should().BeNull();
            v.FamilyRu.Should().BeNull();
            v.TitleEs.Should().BeNull();
        }

        // ─────────────────────────── Scenario ───────────────────────────

        // Every ScenarioClassMap column except print_and_play (the only .Optional() one).
        // HeaderValidated requires all non-Optional members to be present in the header.
        private static readonly string[] ScenarioColumns =
        {
            "path", "coordonnées", "catégorie", "sous-catégorie", "titre", "baratineur",
            "piocheur", "contexte", "enjeu", "suggestion", "CCby", "category", "subcategory",
            "title", "smoothTalker", "drawer", "context", "issue", "suggestion_en",
            "category_ru", "subcategory_ru", "title_ru", "smoothTalker_ru", "drawer_ru",
            "context_ru", "issue_ru", "suggestion_ru", "category_pt", "subcategory_pt",
            "title_pt", "smoothTalker_pt", "drawer_pt", "context_pt", "issue_pt",
            "suggestion_pt", "category_es", "subcategory_es", "title_es", "smoothTalker_es",
            "drawer_es", "context_es", "issue_es", "suggestion_es", "édition février 2022",
            "print_&_play_fevrier_2022",
        };

        /// <summary>
        /// Scenario headers contain accented French and embedded spaces/hyphens
        /// (catégorie, sous-catégorie, coordonnées, édition février 2022). These only resolve
        /// because CsvBase.LoadFromContent applies PrepareHeaderForMatch (diacritics/underscore/
        /// hyphen/space stripping + lowercasing). A regression here would silently zero-out the
        /// FR fields.
        /// </summary>
        [Fact]
        public void Scenario_LoadFromContent_MapsAccentedFrenchHeaders()
        {
            var header = string.Join(",", ScenarioColumns);
            var row = new string[ScenarioColumns.Length];
            row[0] = "1";            // path
            row[1] = "coord";        // coordonnées
            row[2] = "cat1";         // catégorie
            row[3] = "sub1";         // sous-catégorie
            row[4] = "Titre";        // titre
            row[43] = "2022";        // édition février 2022
            var csv = header + "\n" + string.Join(",", row) + "\n";

            var scenarii = Scenario.LoadFromContent(csv);

            scenarii.Should().ContainSingle();
            var s = scenarii[0];
            s.path.Should().Be("1");
            s.coordonnees.Should().Be("coord");
            s.categorie.Should().Be("cat1");
            s.sous_categorie.Should().Be("sub1");
            s.titre.Should().Be("Titre");
            s.edition_fevrier_2022.Should().Be("2022");
        }

        /// <summary>
        /// Scenario.GetId() returns the path column (the canonical id), or empty if null.
        /// </summary>
        [Fact]
        public void Scenario_GetId_ReturnsPathColumn()
        {
            var header = string.Join(",", ScenarioColumns);
            var row = new string[ScenarioColumns.Length];
            row[0] = "1.2.3";        // path
            var csv = header + "\n" + string.Join(",", row) + "\n";

            var scenarii = Scenario.LoadFromContent(csv);

            scenarii[0].path.Should().Be("1.2.3");
            scenarii[0].GetId().Should().Be("1.2.3");
        }

        // ───────────────────────────── Virtue ───────────────────────────

        /// <summary>
        /// VirtueClassMap maps BOTH Id (CsvBase base property) and Pk to the same "pk"
        /// column. The base GetId() returns Id, so both must be populated for GetId() to
        /// resolve correctly.
        /// </summary>
        [Fact]
        public void Virtue_LoadFromContent_MapsPkColumnToBothPkAndId()
        {
            var csv =
                ArgumentVirtueFrMetaHeader + "\n" +
                "2.1,2.1,2,2.1,Famille,Sub,Subsub,Titre,Description,Remark,Link,FamilleCc,2,1,,\n";

            var virtues = Virtue.LoadFromContent(csv);

            virtues.Should().ContainSingle();
            var v = virtues[0];
            v.Pk.Should().Be("2.1");
            v.Id.Should().Be("2.1");
            v.GetId().Should().Be("2.1");
        }
    }
}
