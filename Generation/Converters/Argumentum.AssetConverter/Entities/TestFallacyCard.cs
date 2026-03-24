using CsvHelper.Configuration;

namespace Argumentum.AssetConverter.Entities
{
    /// <summary>
    /// Représente une carte de fallacy simplifiée pour les tests visuels
    /// </summary>
    public class TestFallacyCard : CsvBase<TestFallacyCard, TestFallacyCardClassMap>, ICsvBase
    {
        /// <summary>
        /// Identifiant unique de la carte
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Titre de la fallacy
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type de la carte
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Chemin vers l'illustration
        /// </summary>
        public string IllustrationPath { get; set; }

        /// <summary>
        /// Description de la fallacy
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Retourne l'identifiant unique pour la génération d'images
        /// </summary>
        /// <returns>L'identifiant de la carte</returns>
        public string GetId()
        {
            return Id;
        }
    }

    /// <summary>
    /// Configuration de mapping CSV pour TestFallacyCard
    /// </summary>
    public sealed class TestFallacyCardClassMap : ClassMap<TestFallacyCard>
    {
        public TestFallacyCardClassMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Title).Name("Title");
            Map(m => m.Type).Name("Type");
            Map(m => m.IllustrationPath).Name("IllustrationPath");
            Map(m => m.Description).Name("Description");
        }
    }
}