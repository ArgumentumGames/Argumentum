using System.Text.Json.Serialization;

namespace Argumentum.AssetConverter
{
    public class DocumentCardSet
    {

        public string CardSetName { get; set; }


        //public string TemplateDocumentName { get; set; }

        public int NbCopies { get; set; } = 1;


        public bool SaveOriginalImage { get; set; } = true;

        /// <summary>CMYK conversion for Debug builds (preview-friendly, smaller files).</summary>
        public bool ConvertToCmykDebug { get; set; } = false;

        /// <summary>
        /// Per-image CMYK conversion for Release builds. Default false since #1111: the PNG write
        /// destroys CMYK (#632), so the sRGB→CMYK→RGB round-trip only shifted pixels. The
        /// authoritative CMYK path is the Ghostscript post-process (PdfCmykPostProcess, --pdf-cmyk).
        /// </summary>
        public bool ConvertToCmykRelease { get; set; } = false;

        public bool GetConvertToCmyk(AssetConverterConfig config)
            => config.UseDebugParams ? ConvertToCmykDebug : ConvertToCmykRelease;

        public DocumentCard FrontCards { get; set; } = new DocumentCard();

        public DocumentCard BackCards { get; set; } = new DocumentCard();

    }
}