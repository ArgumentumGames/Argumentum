using System.Collections.Generic;
using System.Diagnostics;

namespace Argumentum.AssetConverter
{
    public class WebBasedGeneratorConfig
    {

        public string ChromeBinaryPath { get; set; } = @"E:\LiberKey\MyApps\GoogleChromePortable\App\Chrome-bin\chrome.exe";

        public string CardpenUrl { get; set; }= @"http://argumentum.myia.org/portals/0/argumentum/Generation/CardPen/index.html";

        //public int TestCards { get; set; } = 2;

        public List<DocumentConfig> Documents { get; set; } = new List<DocumentConfig>(
            new[]
            {
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards.pdf",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules",
                            NbCopies = 1,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo",
                            NbCopies = 7,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies",
                            NbCopies = 1,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-PokerCards.pdf",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Scenarii",
                            NbCopies = 1,
                            HeigthMM = 84,
                            WidthMM = 55
                        }
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards-2.pdf",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules",
                            NbCopies = 1,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo",
                            NbCopies = 7,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-2",
                            NbCopies = 1,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                    }),
                },
                new DocumentConfig()
                {
                    DocumentName = "Argumentum-TarotCards-3.pdf",
                    CardSets = new List<DocumentCardSet>(new[]
                    {
                        new DocumentCardSet()
                        {
                            CardSetName = "Rules",
                            NbCopies = 1,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Memo",
                            NbCopies = 7,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                        new DocumentCardSet()
                        {
                            CardSetName = "Fallacies-3",
                            NbCopies = 1,
                            HeigthMM = 110,
                            WidthMM = 65,
                            BorderMM = 3,
                        },
                    }),
                },
            });




        public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>(
            new []
            {
                new CardSetConfig(){
                    Name ="Rules",
                    //BackExampleName = "Argumentum - Fallacies - Back - Francais",
                    FaceExampleName = "Argumentum - Rules - Francais"},
                new CardSetConfig(){
                    Name ="Memo",
                    BackExampleName = "Argumentum - Memo - Back - Francais",
                    FaceExampleName = "Argumentum - Memo - Face - Francais"},
                new CardSetConfig(){
                    Name ="Fallacies", 
                    BackExampleName = "Argumentum - Fallacies - Back - Francais", 
                    FaceExampleName = "Argumentum - Fallacies - Face - Francais"},
                new CardSetConfig(){
                    Name ="Scenarii",
                    BackExampleName = "Argumentum - Scenarii - Back - Francais",
                    FaceExampleName = "Argumentum - Scenarii - Face - Francais"},
                new CardSetConfig(){
                    Name ="Fallacies-2",
                    BackExampleName = "Argumentum - Fallacies - Back - Francais",
                    FaceExampleName = "Argumentum - Fallacies - Face - Francais - Bis"},
                new CardSetConfig(){
                    Name ="Fallacies-3",
                    BackExampleName = "Argumentum - Fallacies - Back - Francais",
                    FaceExampleName = "Argumentum - Fallacies - Face - v2 - Francais"},
            });

        public void Apply(Stopwatch objSw)
        {
            var generator = new WebBasedGenerator(this, objSw);
            generator.Run();

        }


    }
}