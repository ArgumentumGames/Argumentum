using System.Text.Json.Serialization;

namespace Argumentum.AssetConverter
{
    public class DocumentCardSet
    {

        public string CardSetName { get; set; }


        //public string TemplateDocumentName { get; set; }

        public int NbCopies { get; set; } = 1;


        public bool SaveOriginalImage { get; set; } = true;

        public bool ConvertToCmyk { get; set; } = true;

        /// <summary>CMYK conversion for Debug builds (preview-friendly, smaller files).</summary>
        public bool ConvertToCmykDebug { get; set; } = false;

        /// <summary>CMYK conversion for Release builds (printer quality).</summary>
        public bool ConvertToCmykRelease { get; set; } = true;

        public bool GetConvertToCmyk(AssetConverterConfig config)
            => config.UseDebugParams ? ConvertToCmykDebug : ConvertToCmykRelease;

        public DocumentCard FrontCards { get; set; } = new DocumentCard();

        public DocumentCard BackCards { get; set; } = new DocumentCard();

    }
}