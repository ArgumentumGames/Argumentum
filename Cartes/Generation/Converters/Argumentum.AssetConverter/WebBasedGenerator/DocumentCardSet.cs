namespace Argumentum.AssetConverter
{
    public class DocumentCardSet
    {

        public string CardSetName { get; set; }


        //public string TemplateDocumentName { get; set; }

        public int NbCopies { get; set; } = 1;


        public bool SaveOriginalImage { get; set; } = true;

        public bool ConvertToCmyk { get; set; } = true;

        public decimal HeigthMM { get; set; }

        public decimal WidthMM { get; set; }

        public decimal BorderMM { get; set; }

       


        

    }
}