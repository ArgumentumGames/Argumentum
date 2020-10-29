using System.Collections.Generic;
using System.Diagnostics;

namespace Argumentum.AssetConverter
{
    public class WebBasedGeneratorConfig
    {

        public string ChromeBinaryPath { get; set; } = @"E:\LiberKey\MyApps\GoogleChromePortable\App\Chrome-bin\chrome.exe";

        public string CardpenUrl { get; set; }= @"http://argumentum.myia.org/portals/0/argumentum/Generation/CardPen/index.html";


        public List<CardSetConfig> CardSets { get; set; } = new List<CardSetConfig>(
            new []
            {
                new CardSetConfig(){
                    Name ="Rules",
                    DocumentName ="Argumentum-TarotCards.pdf",
                    NbCopies = 1,
                    HeigthMM = 110,
                    WidthMM = 65,
                    //BackExampleName = "Argumentum - Fallacies - Back - Francais",
                    FaceExampleName = "Argumentum - Rules - Francais"},
                new CardSetConfig(){
                    Name ="Memo",
                    DocumentName ="Argumentum-TarotCards.pdf",
                    NbCopies = 7,
                    HeigthMM = 110,
                    WidthMM = 65,
                    BackExampleName = "Argumentum - Memo - Back - Francais",
                    FaceExampleName = "Argumentum - Memo - Face - Francais"},
                new CardSetConfig(){
                    Name ="Fallacies", 
                    DocumentName ="Argumentum-TarotCards.pdf",
                    NbCopies = 1,
                    HeigthMM = 110,
                    WidthMM = 65,
                    BackExampleName = "Argumentum - Fallacies - Back - Francais", 
                    FaceExampleName = "Argumentum - Fallacies - Face - Francais"},
                new CardSetConfig(){
                    Name ="Scenarii",
                    DocumentName ="Argumentum-PokerCards.pdf",
                    NbCopies = 1,
                    HeigthMM = 84,
                    WidthMM = 55,
                    BackExampleName = "Argumentum - Scenarii - Back - Francais",
                    FaceExampleName = "Argumentum - Scenarii - Face - Francais"},
            });

        public void Apply(Stopwatch objSw)
        {
            var generator = new WebBasedGenerator(this, objSw);
            generator.Run();

        }


    }
}