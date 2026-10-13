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
    /// </summary>
    public class DnnLegalMentionsResxGuardTests
    {
        private const string NeutralResx = "DNNPlatform/App_GlobalResources/GlobalResources.resx";
        private const string FrenchResx = "DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx";
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
            served.Should().Contain("\"Publisher of the Site\"",
                "la désignation contractuelle du bloc, forme corrigée");

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
