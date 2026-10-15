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
        public void NeutralResx_DnnAgreementTemplate_SurvivesTheDifferentialInsert()
        {
            var value = ReadTermsValue(NeutralResx);

            value.Should().Contain("AGREEMENT BETWEEN USER AND [Portal:PortalName]",
                "l'insertion est différentielle : le gabarit de conditions d'usage DNN existant " +
                "doit survivre tel quel derrière le bloc de mentions légales");
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
            value.Should().Contain("AGREEMENT BETWEEN USER AND [Portal:PortalName]",
                "l'insertion est différentielle : le gabarit DNN existant survit derrière le bloc");
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
            value.Should().Contain("AGREEMENT BETWEEN USER AND [Portal:PortalName]",
                "l'insertion est différentielle : le gabarit DNN existant survit derrière le bloc");
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

        [Fact]
        public void NeutralResx_UsesEnglishCurlyQuotes_DnnTemplateKeepsStraightOnes()
        {
            var value = ReadTermsValue(NeutralResx);

            value.Should().Contain("“Argumentum”",
                "les citations du nom du site en anglais prennent les guillemets typographiques “ ”");
            value.Should().Contain("“Host”", "la désignation de l'hébergeur, guillemets typographiques");
            value.Should().Contain("\"Linked Sites\"",
                "CONTRÔLE DE PÉRIMÈTRE : le gabarit DNN derrière AGREEMENT garde ses guillemets DROITS — " +
                "l'insertion est différentielle, la typographie du grain ne touche que le bloc légal");
        }

        [Fact]
        public void SpanishResx_UsesSpanishGuillemets_DnnTemplateKeepsStraightOnes()
        {
            var value = ReadTermsValue(SpanishResx);

            value.Should().Contain("«Argumentum»",
                "espagnol : guillemets « » SANS espace intérieure (usage RAE, contrairement au français)");
            value.Should().Contain("\"Linked Sites\"",
                "CONTRÔLE DE PÉRIMÈTRE : le gabarit DNN garde ses guillemets droits");
        }

        [Fact]
        public void PortugueseResx_UsesCurlyQuotes_DnnTemplateKeepsStraightOnes()
        {
            var value = ReadTermsValue(PortugueseResx);

            value.Should().Contain("“Argumentum”",
                "portugais brésilien : guillemets typographiques “ ” (usage brésilien courant)");
            value.Should().Contain("\"Linked Sites\"",
                "CONTRÔLE DE PÉRIMÈTRE : le gabarit DNN garde ses guillemets droits");
        }

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
