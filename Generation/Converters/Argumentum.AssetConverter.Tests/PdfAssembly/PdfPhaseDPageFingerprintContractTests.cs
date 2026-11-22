using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// #1460 <b>phase D</b> (pool #458, P24-38) — le contrôle contractuel de l'empreinte par page
    /// sur des PDF <b>fabriqués pour le test</b> (aucun artefact réel : la référence réelle attend
    /// les « PDF validés » de #2132, aucun PDF n'est commis dans le dépôt).
    ///
    /// <para>DoD du grain : page permutée ⇒ rouge, page modifiée ⇒ rouge, PDF identique ⇒ vert.
    /// Chaque prédicat est vu dans les <b>deux</b> sens (#1046 : une garde jamais vue rouge est un
    /// no-op) — y compris la classe <b>must-stay-green</b> : changer la sérialisation physique
    /// (compression, ordre d'écriture, /Length indirect, bruit d'espacements) ne doit PAS rougir,
    /// sinon la future référence réelle serait liée à une version de générateur.</para>
    ///
    /// <para>Le fabriqueur synthétique écrit des PDF minimaux valides (catalogue, arbre /Pages,
    /// /Contents, XObjects image) dont chaque variante d'encodage est contrôlée : c'est le seul
    /// moyen d'obtenir exactement « même contenu logique, autre sérialisation » — un générateur
    /// réel n'offre pas ce levier de manière déterministe en test unitaire. Tout l'assemblage
    /// passe par Latin1 (un char = un octet) : les fils binaires traversent le StringBuilder sans
    /// perte, et le fichier final reste un PDF valide pour tout lecteur réel.</para>
    /// </summary>
    public class PdfPhaseDPageFingerprintContractTests
    {
        // ─────────────────────────────────────────────────────────────────────────
        // (1) PDF IDENTIQUE ⇒ VERT — la propriété qui rend la référence portable.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Two_Physical_Serializations_Of_The_Same_Document_Yield_Equal_Fingerprints()
        {
            var contents = new[] { Content(0), Content(1), Content(2) };
            var images = new[] { ImageBytes(0), ImageBytes(1), ImageBytes(2) };

            var plain = BuildPdf(contents, perPageImages: images);
            var reshuffled = BuildPdf(
                contents, perPageImages: images,
                flateContents: true, flateImages: true, zlibLevel: CompressionLevel.Optimal,
                reverseWriteOrder: true, indirectLength: true, noise: "% sérialisation bruitée\n");

            var fpPlain = PdfPageFingerprintIntegrity.Fingerprints(plain);
            var fpShuffled = PdfPageFingerprintIntegrity.Fingerprints(reshuffled);

            // Témoin de non-vacuité : les deux fichiers diffèrent physiquement (sinon l'égalité
            // des empreintes ne prouverait rien).
            Convert.ToHexString(plain).Should().NotBe(Convert.ToHexString(reshuffled));

            fpPlain.Should().HaveCount(3);
            fpShuffled.Should().Equal(fpPlain,
                "deux sérialisations physiques du même contenu logique (flux compressés, ordre des " +
                "objets inversé, /Length indirect, commentaires) doivent donner des listes d'empreintes " +
                "égales — sans quoi la référence réelle dépendrait du générateur qui l'a produite.");
            fpPlain.Distinct().Should().HaveCount(3,
                "trois pages de contenus distincts doivent donner trois empreintes distinctes — " +
                "une empreinte constante rendrait la comparaison vacue (0 == 0 déguisé).");
        }

        [Fact]
        public void Compression_Level_Alone_Stays_Green()
        {
            var contents = new[] { Content(0), Content(1), Content(2) };

            // indirectLength sans inversion d'ordre : l'entier /Length suit PHYSIQUEMENT son flux,
            // l'instrument doit résoudre la longueur par recherche d'endstream (repli standard).
            var fastest = BuildPdf(contents, flateContents: true, zlibLevel: CompressionLevel.Fastest,
                indirectLength: true);
            var smallest = BuildPdf(contents, flateContents: true, zlibLevel: CompressionLevel.SmallestSize);

            // Témoin : les deux sérialisations compressées diffèrent bien physiquement.
            Convert.ToHexString(fastest).Should().NotBe(Convert.ToHexString(smallest));

            PdfPageFingerprintIntegrity.Fingerprints(smallest).Should().Equal(
                PdfPageFingerprintIntegrity.Fingerprints(fastest),
                "le niveau de compression change les octets encodés, jamais le contenu décodé — " +
                "la classe must-stay-green du discriminant sérialisation/contenu.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (2) PAGE PERMUTÉE ⇒ ROUGE — l'ordre de l'arbre /Pages est de l'identité.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Permuted_Kids_Order_Is_Red_At_The_Swapped_Indices_Only()
        {
            var contents = new[] { Content(0), Content(1), Content(2) };
            var images = new[] { ImageBytes(0), ImageBytes(1), ImageBytes(2) };

            var expected = PdfPageFingerprintIntegrity.Fingerprints(BuildPdf(contents, perPageImages: images));
            // /Kids réordonné : la page 2 prend la position 1 et réciproquement.
            var permuted = PdfPageFingerprintIntegrity.Fingerprints(
                BuildPdf(contents, perPageImages: images, kidsOrder: new[] { 1, 0, 2 }));

            var failures = PdfPageFingerprintIntegrity.Compare(expected, permuted);
            var joined = string.Join(" | ", failures);

            failures.Should().HaveCount(2, "seules les pages 1 et 2 ont échangé leur place.");
            joined.Should().Contain("page 1 :").And.Contain("page 2 :",
                "l'écart nomme les pages touchées, jamais un booléen muet.");
            joined.Should().NotContain("page 3", "la page 3 n'a pas bougé.");
            joined.Should().Contain(expected[0][..12]).And.Contain(expected[1][..12],
                "les empreintes attendues sont nommées (préfixes 12 hex) dans le message.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (3) PAGE MODIFIÉE ⇒ ROUGE — à SA page seulement.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Modified_Page_Content_Is_Red_At_That_Page_Only()
        {
            var contents = new[] { Content(0), Content(1), Content(2) };
            var modified = new[] { Content(0), Content(1), Content(2) + " 10 10 50 50 re f" };
            var images = new[] { ImageBytes(0), ImageBytes(1), ImageBytes(2) };

            var expected = PdfPageFingerprintIntegrity.Fingerprints(BuildPdf(contents, perPageImages: images));
            var actual = PdfPageFingerprintIntegrity.Fingerprints(BuildPdf(modified, perPageImages: images));

            var failures = PdfPageFingerprintIntegrity.Compare(expected, actual);
            failures.Should().HaveCount(1, "seule la page 3 a changé de contenu.");
            failures[0].Should().Contain("page 3")
                .And.NotContain("page 1").And.NotContain("page 2",
                    "une modification du flux de contenu de la page 3 ne touche que la page 3.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (4) XObject partagé — les ressources sont DANS l'empreinte.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Shared_XObject_Ties_Pages_And_Mutating_It_Reddens_Both()
        {
            var sameContent = new[] { Content(0), Content(0) }; // deux pages strictement identiques
            var sharedImage = new[] { ("/Im0", ImageBytes(0)) };

            var pdf = BuildPdf(sameContent, sharedImages: sharedImage);
            var fp = PdfPageFingerprintIntegrity.Fingerprints(pdf);
            fp.Should().HaveCount(2);
            fp[1].Should().Be(fp[0],
                "deux pages de contenu identique partageant le même XObject (même objet, pas une copie) " +
                "ont le même sous-graphe donc la même empreinte.");

            // La même image, un octet muté : les DEUX pages doivent rougir — c'est la preuve que
            // les ressources référencées participent à l'empreinte (un hash du seul flux de
            // contenu, qui ne change pas ici, resterait vert).
            var mutated = new[] { ("/Im0", MutateOneByte(ImageBytes(0))) };
            var failures = PdfPageFingerprintIntegrity.Compare(
                fp, PdfPageFingerprintIntegrity.Fingerprints(BuildPdf(sameContent, sharedImages: mutated)));

            var joined = string.Join(" | ", failures);
            failures.Should().HaveCount(2);
            joined.Should().Contain("page 1 :").And.Contain("page 2 :");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (5) Le /Parent ne fuit pas dans l'identité d'une page.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Appended_Page_Leaves_Earlier_Fingerprints_Unchanged()
        {
            var three = new[] { Content(0), Content(1), Content(2) };
            var four = new[] { Content(0), Content(1), Content(2), Content(3) };
            var images3 = new[] { ImageBytes(0), ImageBytes(1), ImageBytes(2) };
            var images4 = new[] { ImageBytes(0), ImageBytes(1), ImageBytes(2), ImageBytes(3) };

            var fp3 = PdfPageFingerprintIntegrity.Fingerprints(BuildPdf(three, perPageImages: images3));
            var fp4 = PdfPageFingerprintIntegrity.Fingerprints(BuildPdf(four, perPageImages: images4));

            fp4.Take(3).Should().Equal(fp3,
                "ajouter une page change le /Count du nœud /Pages partagé (le /Parent de chaque feuille) : " +
                "cette donnée amont NE doit PAS entrer dans l'empreinte d'une page, sinon toute retouche " +
                "du document invaliderait toutes les références déjà enregistrées.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (6) Fermeture éclairée — jamais de liste partielle silencieuse.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Missing_Catalog_Fails_Closed_With_A_Named_Cause()
        {
            var bytes = BuildPdf(new[] { Content(0) }, stripCatalogType: true);

            var act = () => PdfPageFingerprintIntegrity.Fingerprints(bytes);

            act.Should().Throw<PdfPageFingerprintIntegrity.PdfFingerprintException>()
                .WithMessage("*Catalog*",
                    "sans objet /Type /Catalog l'instrument nomme la cause au lieu de rendre une liste " +
                    "partielle (vide) lisible comme « PDF sans pages ».");
        }

        [Fact]
        public void ObjStm_Detected_Fails_Closed()
        {
            var bytes = BuildPdf(new[] { Content(0) }, addObjStmJunk: true);

            var act = () => PdfPageFingerprintIntegrity.Fingerprints(bytes);

            act.Should().Throw<PdfPageFingerprintIntegrity.PdfFingerprintException>()
                .WithMessage("*ObjStm*",
                    "les objets compressés en flux rendent la table incomplète : fermeture éclairée, " +
                    "jamais un sous-ensemble d'objets traité comme le document entier.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // (7) L'écart de nombre de pages est nommé en premier.
        // ─────────────────────────────────────────────────────────────────────────

        [Fact]
        public void Page_Count_Mismatch_Is_Named_First()
        {
            var fp3 = PdfPageFingerprintIntegrity.Fingerprints(
                BuildPdf(new[] { Content(0), Content(1), Content(2) }));
            var fp4 = PdfPageFingerprintIntegrity.Fingerprints(
                BuildPdf(new[] { Content(0), Content(1), Content(2), Content(3) }));

            var failures = PdfPageFingerprintIntegrity.Compare(fp3, fp4);

            failures.Should().HaveCount(1,
                "les trois premières pages coïncident : le seul écart est le compte lui-même.");
            failures[0].Should().Contain("attendu 3").And.Contain("mesuré 4",
                "la ligne de compte nomme les deux valeurs avant toute comparaison indice à indice.");
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Fabriqueur de PDF synthétiques — chaque levier d'encodage est un paramètre.
        // ─────────────────────────────────────────────────────────────────────────

        private static string Content(int seed) =>
            $"q 0 0 1 rg 10 {10 + seed} 100 200 re f{Repeat(" 1 0 0 1 2 3 cm", 40)} /Im0 Do";

        private static string Repeat(string s, int times) => string.Concat(Enumerable.Repeat(s, times));

        private static byte[] ImageBytes(int seed)
        {
            var bytes = new byte[64];
            for (var i = 0; i < bytes.Length; i++) bytes[i] = (byte)((seed * 37 + i * 11) & 0xFF);
            return bytes;
        }

        private static byte[] MutateOneByte(byte[] source)
        {
            var mutated = (byte[])source.Clone();
            mutated[mutated.Length / 2] ^= 0xFF;
            return mutated;
        }

        /// <summary>Construit un PDF minimal valide. Les numéros d'objets sont stables ; seuls
        /// l'ordre physique d'écriture, l'encodage des flux et le bruit varient — la liste des
        /// variantes EST la matrice des témoins ci-dessus.</summary>
        private static byte[] BuildPdf(
            IReadOnlyList<string> contents,
            IReadOnlyList<(string Name, byte[] Bytes)>? sharedImages = null,
            IReadOnlyList<byte[]>? perPageImages = null,
            bool flateContents = false,
            bool flateImages = false,
            CompressionLevel zlibLevel = CompressionLevel.Optimal,
            bool reverseWriteOrder = false,
            bool indirectLength = false,
            string noise = "",
            int[]? kidsOrder = null,
            bool stripCatalogType = false,
            bool addObjStmJunk = false)
        {
            sharedImages ??= Array.Empty<(string, byte[])>();
            perPageImages ??= Array.Empty<byte[]>();
            kidsOrder ??= Enumerable.Range(0, contents.Count).ToArray();

            var next = 1;
            var catalogObj = next++;
            var pagesObj = next++;
            var sharedObjs = sharedImages.Select(_ => next++).ToArray();
            var contentObjs = new int[contents.Count];
            var imageObjs = new int[contents.Count];
            var pageObjs = new int[contents.Count];
            for (var i = 0; i < contents.Count; i++)
            {
                contentObjs[i] = next++;
                imageObjs[i] = perPageImages.Count > i ? next++ : 0;
                pageObjs[i] = next++;
            }

            var bodies = new Dictionary<int, string>();
            var lengthValues = new Dictionary<int, int>(); // objNum du /Length indirect -> valeur

            // Émet un objet flux : /Length direct, ou /Length indirect (objet entier séparé,
            // écrit APRÈS le flux — l'instrument doit alors résoudre par recherche d'endstream).
            void EmitStream(int objNum, string dictPrefix, byte[] payload, bool flate)
            {
                var wire = flate ? Zlib(payload, zlibLevel) : payload;
                string lengthEntry;
                if (indirectLength)
                {
                    var lengthObj = next++;
                    lengthValues[lengthObj] = wire.Length;
                    lengthEntry = $"/Length {lengthObj} 0 R";
                }
                else
                {
                    lengthEntry = $"/Length {wire.Length}";
                }

                var filterEntry = flate ? " /Filter /FlateDecode" : "";
                bodies[objNum] = $"{dictPrefix} {lengthEntry}{filterEntry} >>\nstream\n" +
                    $"{Encoding.Latin1.GetString(wire)}\nendstream";
            }

            // Catalogue + racine des pages.
            bodies[catalogObj] = stripCatalogType
                ? $"<< /Pages {pagesObj} 0 R >>"
                : $"<< /Type /Catalog /Pages {pagesObj} 0 R >>";
            bodies[pagesObj] = $"<< /Type /Pages /Count {contents.Count} /Kids [ " +
                string.Join(" ", kidsOrder.Select(p => $"{pageObjs[p]} 0 R")) + " ] >>";

            // Images partagées (un objet pour toutes les pages).
            for (var s = 0; s < sharedImages.Count; s++)
            {
                EmitStream(sharedObjs[s],
                    "<< /Type /XObject /Subtype /Image /Width 16 /Height 16 /ColorSpace /DeviceGray /BitsPerComponent 8",
                    sharedImages[s].Bytes, flateImages);
            }

            // Pages : contenu, image propre éventuelle, ressources.
            for (var i = 0; i < contents.Count; i++)
            {
                EmitStream(contentObjs[i], "<<", Encoding.ASCII.GetBytes(contents[i]), flateContents);
                if (imageObjs[i] > 0)
                {
                    EmitStream(imageObjs[i],
                        "<< /Type /XObject /Subtype /Image /Width 16 /Height 16 /ColorSpace /DeviceGray /BitsPerComponent 8",
                        perPageImages[i], flateImages);
                }

                var resources = new StringBuilder("<< /XObject << ");
                if (imageObjs[i] > 0) resources.Append($"/Im0 {imageObjs[i]} 0 R ");
                for (var s = 0; s < sharedImages.Count; s++) resources.Append($"{sharedImages[s].Name} {sharedObjs[s]} 0 R ");
                resources.Append(">> >>");
                bodies[pageObjs[i]] =
                    $"<< /Type /Page /Parent {pagesObj} 0 R /MediaBox [0 0 200 300] /Resources {resources} /Contents {contentObjs[i]} 0 R >>";
            }

            foreach (var (lenObj, value) in lengthValues)
            {
                bodies[lenObj] = value.ToString();
            }

            // Assemblage physique (Latin1 : un char = un octet, les fils binaires passent intacts).
            var order = Enumerable.Range(1, next - 1).ToList();
            if (reverseWriteOrder) order.Reverse();

            var sb = new StringBuilder();
            sb.Append("%PDF-1.4\n");
            foreach (var objNum in order)
            {
                sb.Append($"{objNum} 0 obj\n{bodies[objNum]}\nendobj\n");
                if (noise.Length > 0) sb.Append(noise);
            }

            if (addObjStmJunk)
            {
                sb.Append("99 0 obj\n<< /Type /ObjStm /N 0 /First 0 /Length 0 >>\nstream\n\nendstream\nendobj\n");
            }

            // xref + trailer (l'instrument ne lit ni l'un ni l'autre — résolution par catalogue —
            // mais le fichier reste un PDF valide pour tout lecteur réel).
            var maxObj = Math.Max(next - 1, addObjStmJunk ? 99 : 0);
            var xrefOffset = sb.Length;
            sb.Append($"xref\n0 {maxObj + 1}\n");
            sb.Append("0000000000 65535 f \n");
            for (var objNum = 1; objNum <= maxObj; objNum++)
            {
                sb.Append($"{objNum:D10} 00000 n \n");
            }

            sb.Append($"trailer\n<< /Size {maxObj + 1} /Root {catalogObj} 0 R >>\nstartxref\n{xrefOffset}\n%%EOF\n");
            return Encoding.Latin1.GetBytes(sb.ToString());
        }

        private static byte[] Zlib(byte[] data, CompressionLevel level)
        {
            using var ms = new MemoryStream();
            ms.WriteByte(0x78);
            ms.WriteByte(0x9C);
            using (var ds = new DeflateStream(ms, level, leaveOpen: true))
            {
                ds.Write(data, 0, data.Length);
            }

            return ms.ToArray();
        }
    }
}
