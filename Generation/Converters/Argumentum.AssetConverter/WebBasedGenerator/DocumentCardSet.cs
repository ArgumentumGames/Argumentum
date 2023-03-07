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

        public DocumentCard FrontCards { get; set; } = new DocumentCard();

        public DocumentCard BackCards { get; set; } = new DocumentCard();

    }
}