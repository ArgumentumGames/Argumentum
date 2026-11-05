using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// #1460 <b>phase A</b> — géométrie de page et <i>page counts</i> des PDF <b>produits</b>, sans
    /// rendu. Pur et sans I/O : la CI prouve que le vérificateur sait devenir ROUGE (#1046 : une
    /// garde jamais vue rouge est un no-op) avant qu'il ne soit cru vert sur un bundle.
    ///
    /// <para><b>Frontière — ⛔ pas un second référentiel.</b> Les comptes de pages ne sont PAS
    /// re-dérivés ici : ils viennent de <see cref="PdfDeckCountDerivation"/>, le moteur des organes
    /// de contrat existants (#1187/#1288, #1212). Ce fichier n'ajoute qu'une chose : la
    /// <b>géométrie</b>, et le fait que le compte dérivé <i>se matérialise</i> dans le PDF.</para>
    ///
    /// <para><b>D'où vient l'attendu d'une taille ?</b> De deux sources, jamais d'une troisième :</para>
    /// <list type="bullet">
    /// <item><description><b>Format déclaré</b> (A4, A0) : la valeur est celle de
    /// <c>QuestPDF.Helpers.PageSizes</c>, c'est-à-dire le <b>même objet</b> que lit
    /// <c>PrintAndPlayDocument.Compose</c> pour composer la page. Voir
    /// <see cref="DeclaredPageFormats"/>.</description></item>
    /// <item><description><b>Document « carte »</b> (Tarot, Virtues, Poker) : la page est écrite par
    /// Magick.NET (<c>PdfManager.GenerateAlternateFaceAndBack</c>) à la <b>taille physique du PNG</b>,
    /// et c'est la <b>config qui fixe cette taille</b> : <c>ImageHelper</c> redimensionne chaque PNG de
    /// carte à <c>FrontCards/BackCards.WidthMM × HeigthMM</c> à 300 dpi (<c>ResizeInMM</c>,
    /// <c>ImageHelper.cs:170-185</c>). L'attendu est donc <b>dérivable de la config</b> (± la
    /// tolérance), preuve à l'appui : le Poker produit 63,5 × 88,9 mm, exactement son nominal, et le
    /// Tarot d'un arbre d'avant #1250 portait l'ancien nominal (l. 60 × 113 d'alors) — l'écart mesuré
    /// le 05/11 était l'âge de l'arbre, pas un défaut de dérivation.</description></item>
    /// </list>
    ///
    /// <para>⚠️ <b>Ce que cet organe n'établit pas</b> : il ne dit rien de la <i>justesse</i> d'une
    /// taille (un format peut être laid et régulier), ni de l'orientation d'une page, ni du contenu.
    /// Il ne compare pas non plus deux langues entre elles — chaque mesure porte son propre attendu.</para>
    /// </summary>
    internal static class PdfPageGeometryIntegrity
    {
        /// <summary>Points (1/72 in) → millimètres. Le PDF exprime les MediaBox en points ; les
        /// formats nominaux s'écrivent en mm : c'est la seule conversion de ce fichier.</summary>
        public const double MmPerPoint = 25.4 / 72.0;

        /// <summary>
        /// Tolérance <b>pointeur</b> exigée par la spec #1460 : les formats ont des valeurs
        /// nominales en mm, le PDF les exprime en points, et l'arrondi de la bibliothèque n'est pas
        /// le format. 1 mm absorbe les arrondis mesurés (A0 : 1189,2 vs 1189 nominal, soit 0,2 mm ;
        /// A4 : 209,9 vs 210, soit 0,1 mm) sans absorber une erreur de format — le plus petit écart
        /// entre deux formats de ce dépôt est A4/A0, très au-delà.
        /// </summary>
        public const double ToleranceMm = 1.0;

        /// <summary>
        /// Formatage <b>invariant</b> des nombres dans les messages de diagnostic. Un message de
        /// garde est lu par des humains et par des assertions ; il ne doit pas changer de forme
        /// selon la culture de la machine (« 59,9 mm » sous une locale française, « 59.9 » ailleurs).
        /// Mesuré le 05/11 : sans cela, les témoins de ce fichier rougissaient sur ce poste.
        /// </summary>
        private static string F(double value, string format = "F1")
            => value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>Date de diagnostic en invariant — ⚠️ une date formatée sous une locale
        /// non grégorienne (ar-SA = UmAlQura, fa-IR = persan, cf. mémoire
        /// <c>reference-dotnet-framework-ar-fa-calendars</c>) changerait d'ANNÉE, pas seulement de forme.</summary>
        private static string D(DateTime value)
            => value.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        /// <summary>Un format de page <b>déclaré</b> par la chaîne de production (QuestPDF
        /// <c>PageSizes</c>), avec la provenance de la valeur.</summary>
        public sealed record PageFormat(string Id, double WidthMm, double HeightMm, string Origin);

        /// <summary>
        /// Les deux formats que la chaîne <b>déclare</b> (documents Print&amp;Play et Web). Les
        /// valeurs sont celles de QuestPDF, pas une recopie de documentation : <c>A4</c> = 595 × 842 pt
        /// = 209,94 × 297,04 mm, <c>A0</c> = 2384 × 3371 pt = 841,05 × 1189,20 mm. Les tailles
        /// <b>mesurées</b> sur les PDF produits le 05/11 (8/8 langues) sont rappelées dans
        /// <c>Origin</c> pour qu'un écart se lise sans rouvrir un PDF.
        /// </summary>
        public static readonly IReadOnlyList<PageFormat> DeclaredPageFormats = new[]
        {
            new PageFormat("A4", 595.4 * MmPerPoint, 841.9 * MmPerPoint,
                "QuestPDF.Helpers.PageSizes.A4 = 595,4 x 841,9 pt (210,04 x 297,00 mm) ; MediaBox PRODUIT 595 x 842 pt " +
                "(arrondi au point) — mesuré 209,9 x 297,0 mm sur 8/8 langues (05/11)"),
            new PageFormat("A0", 2384.0 * MmPerPoint, 3371.0 * MmPerPoint,
                "QuestPDF.Helpers.PageSizes.A0 (MediaBox produit 2384 x 3371 pt) — mesuré 841,0 x 1189,2 mm sur 8/8 langues (05/11)"),
        };

        /// <summary>Résout un format déclaré ; un identifiant inconnu <b>lève</b> — un format absent
        /// de la table rendrait la mesure muette, pas tolérante.</summary>
        public static PageFormat Declared(string formatId)
        {
            foreach (var f in DeclaredPageFormats)
                if (string.Equals(f.Id, formatId, StringComparison.Ordinal)) return f;
            throw new InvalidOperationException(
                $"PdfPageGeometryIntegrity: format '{formatId}' n'est pas un format déclaré " +
                $"({string.Join(", ", DeclaredPageFormats.Select(f => f.Id))}) — ajoutez-le à DeclaredPageFormats avec sa provenance, " +
                "ou mesurez-le depuis l'artefact au lieu de le recopier.");
        }

        /// <summary>Une mesure : une page de PDF, son attendu EN MM, et la provenance de cet attendu
        /// (les deux ne se séparent jamais — c'est la leçon « un nombre sans sa provenance » de #1187).</summary>
        /// <param name="DistinctPageSizes">Nombre de tailles de page <b>distinctes</b> rencontrées dans
        /// le document. 1 = uniforme. Un document peut légitimement changer de format entre pages
        /// (aucun ne le fait aujourd'hui) ; l'organe, lui, exige l'uniformité et le dit.</param>
        public sealed record SizeExpectation(
            string Doc, string Lang,
            double ExpectedWidthMm, double ExpectedHeightMm, string Origin,
            double ActualWidthPt, double ActualHeightPt, int DistinctPageSizes);

        /// <summary>
        /// Deux prédicats, chacun nommé par sa conséquence :
        /// <list type="number">
        /// <item><description><b>uniformité</b> — toutes les pages d'un document ont la même
        /// MediaBox. Un PDF qui change de taille en cours de route est un défaut de composition
        /// qu'aucun contrôle de compte de pages ne voit ;</description></item>
        /// <item><description><b>format</b> — la taille observée est celle de l'attendu, à
        /// <see cref="ToleranceMm"/> près, dans les deux sens (largeur ET hauteur : une page
        /// paysage n'est pas « le bon format tourné », c'est un autre format).</description></item>
        /// </list>
        /// </summary>
        public static IReadOnlyList<string> Check(IEnumerable<SizeExpectation> records)
        {
            var failures = new List<string>();
            foreach (var r in records)
            {
                if (r.DistinctPageSizes != 1)
                {
                    failures.Add(
                        $"{r.Lang}/{r.Doc}: {r.DistinctPageSizes} tailles de page DISTINCTES dans un même document — " +
                        $"la première page mesure {F(r.ActualWidthPt, "F2")} x {F(r.ActualHeightPt, "F2")} pt. " +
                        "Un document est composé sur un seul format ; une taille qui varie en cours de route signale " +
                        "des pages issues de deux compositions (voir la mutation « permuter deux pages », #1460 §2).");
                    continue; // une taille non uniforme rend la comparaison de format sans objet.
                }

                var widthMm = r.ActualWidthPt * MmPerPoint;
                var heightMm = r.ActualHeightPt * MmPerPoint;
                var dWidth = Math.Abs(widthMm - r.ExpectedWidthMm);
                var dHeight = Math.Abs(heightMm - r.ExpectedHeightMm);
                if (dWidth <= ToleranceMm && dHeight <= ToleranceMm) continue;

                failures.Add(
                    $"{r.Lang}/{r.Doc}: page {F(widthMm)} x {F(heightMm)} mm, attendu " +
                    $"{F(r.ExpectedWidthMm)} x {F(r.ExpectedHeightMm)} mm (±{F(ToleranceMm)} mm) — " +
                    $"écart {F(dWidth)} mm en largeur, {F(dHeight)} mm en hauteur. Attendu : {r.Origin}. " +
                    "Si l'attendu vient d'un format déclaré (A4/A0), la composition a changé de PageSize ; " +
                    "s'il vient du nominal de config d'un document carte (WidthMM/HeigthMM → ResizeInMM → PNG → page), " +
                    "l'arbre a probablement été généré AVANT ce nominal (régénérez — cf. #1250) ou la chaîne " +
                    "ImageHelper a cessé de dimensionner le PNG ; dans tous les cas " +
                    "le PDF produit n'est pas au format annoncé, et c'est le format qui part chez le fabricant (#1187).");
            }
            return failures;
        }

        /// <summary>
        /// Les trois états de la garde d'arbre, gradés par <b>conséquence</b> (⛔ pas un booléen) —
        /// les mêmes que le pilote <c>tools/1460-structure-charpente-pilot.py</c>, dont ceci est le
        /// jumeau côté organe :
        /// <list type="bullet">
        /// <item><description><b>0 MESURABLE</b> — un arbre frais porte tous les PDF attendus ; les
        /// assertions de dimensions et de comptes ont un objet.</description></item>
        /// <item><description><b>1 DIVERGENT</b> — l'arbre est frais mais l'inventaire contredit
        /// l'attendu (un PDF attendu manque). ⚠️ Cet état était déclaré <b>inatteignable</b> dans la
        /// charpente (§3) : c'est la phase A qui le branche, en apportant la source d'attendu.</description></item>
        /// <item><description><b>2 NON MESURABLE</b> — arbre absent, arbre sans PDF, ou arbre
        /// <b>antérieur au corpus</b>. Confondre 2 et 0 est exactement le défaut que la garde ferme.</description></item>
        /// </list>
        /// </summary>
        public static (TreeVerdict Verdict, string Motif) JudgeTree(
            bool treeExists, int pdfCount, DateTime corpusNewest, DateTime treeNewest, int missingExpected)
        {
            if (!treeExists)
                return (TreeVerdict.NotMeasurable, "arbre ABSENT — aucun Target/ pour cette configuration");
            if (pdfCount == 0)
                return (TreeVerdict.NotMeasurable, "arbre présent MAIS 0 PDF — rien à mesurer");
            if (treeNewest < corpusNewest)
                return (TreeVerdict.NotMeasurable,
                    $"arbre ANTÉRIEUR au corpus de {F((corpusNewest - treeNewest).TotalDays)} j " +
                    $"({D(treeNewest)} < {D(corpusNewest)}) — STALE, " +
                    "il ne peut pas porter l'état courant du corpus (famille « regen sans clobber = stale trap »)");
            if (missingExpected > 0)
                return (TreeVerdict.Divergent,
                    $"arbre frais MAIS {missingExpected} PDF attendu(s) absent(s) — l'inventaire contredit l'attendu");
            return (TreeVerdict.Measurable, $"arbre frais : {pdfCount} PDF");
        }
    }

    /// <summary>Verdict de la garde d'arbre (#1460 §3), gradé par conséquence. Les valeurs sont les
    /// codes de sortie du pilote Python — même échelle des deux côtés du dépôt.</summary>
    public enum TreeVerdict
    {
        Measurable = 0,
        Divergent = 1,
        NotMeasurable = 2,
    }
}
