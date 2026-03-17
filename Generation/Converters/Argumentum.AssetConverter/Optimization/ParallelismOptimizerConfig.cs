using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Optimization
{
    /// <summary>
    /// Configuration pour l'optimiseur de parallélisme qui ajuste automatiquement les paramètres
    /// de parallélisme en fonction des ressources système disponibles.
    /// </summary>
    public class ParallelismOptimizerConfig
    {
        /// <summary>
        /// Indique si l'optimiseur de parallélisme est activé.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Indique si l'optimiseur doit s'exécuter automatiquement avant chaque génération.
        /// </summary>
        public bool RunBeforeGeneration { get; set; } = true;

        /// <summary>
        /// Indique si l'optimiseur doit surveiller et ajuster dynamiquement les paramètres pendant la génération.
        /// </summary>
        public bool DynamicAdjustment { get; set; } = true;

        /// <summary>
        /// Intervalle en secondes pour la surveillance des ressources système pendant la génération.
        /// </summary>
        public int MonitoringIntervalSeconds { get; set; } = 5;

        /// <summary>
        /// Pourcentage maximum d'utilisation du CPU à cibler.
        /// </summary>
        public int TargetCpuUsagePercent { get; set; } = 80;

        /// <summary>
        /// Pourcentage maximum d'utilisation de la mémoire à cibler.
        /// </summary>
        public int TargetMemoryUsagePercent { get; set; } = 70;

        /// <summary>
        /// Facteur de pondération pour l'importance du CPU dans les calculs d'optimisation.
        /// </summary>
        public double CpuWeightFactor { get; set; } = 1.0;

        /// <summary>
        /// Facteur de pondération pour l'importance de la mémoire dans les calculs d'optimisation.
        /// </summary>
        public double MemoryWeightFactor { get; set; } = 0.8;

        /// <summary>
        /// Facteur de pondération pour l'importance du disque dans les calculs d'optimisation.
        /// </summary>
        public double DiskWeightFactor { get; set; } = 0.5;

        /// <summary>
        /// Facteur de pondération pour l'importance du réseau dans les calculs d'optimisation.
        /// </summary>
        public double NetworkWeightFactor { get; set; } = 0.3;

        /// <summary>
        /// Chemin du fichier de rapport de performance.
        /// </summary>
        public string PerformanceReportPath { get; set; } = "ParallelismPerformanceReport.json";

        /// <summary>
        /// Indique si un rapport de performance détaillé doit être généré.
        /// </summary>
        public bool GenerateDetailedReport { get; set; } = true;

        /// <summary>
        /// Nombre minimum de threads à utiliser pour Cardpen, quelle que soit l'optimisation.
        /// </summary>
        public int MinThreadsCardpen { get; set; } = 1;

        /// <summary>
        /// Nombre minimum de threads à utiliser pour les traductions Cardpen, quelle que soit l'optimisation.
        /// </summary>
        public int MinThreadsCardpenTranslations { get; set; } = 1;

        /// <summary>
        /// Nombre minimum de threads à utiliser pour les images, quelle que soit l'optimisation.
        /// </summary>
        public int MinThreadsImages { get; set; } = 1;

        /// <summary>
        /// Nombre minimum de threads à utiliser pour les traductions d'images, quelle que soit l'optimisation.
        /// </summary>
        public int MinThreadsImageTranslations { get; set; } = 1;

        /// <summary>
        /// Nombre minimum de threads à utiliser pour les documents, quelle que soit l'optimisation.
        /// </summary>
        public int MinThreadsDocuments { get; set; } = 1;

        /// <summary>
        /// Applique la configuration de l'optimiseur de parallélisme.
        /// </summary>
        /// <param name="config">La configuration globale de l'application.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task<bool> Apply(AssetConverterConfig config)
        {
            if (!Enabled)
            {
                Logger.Log("L'optimiseur de parallélisme est désactivé.");
                return true;
            }

            Logger.LogTitle("Optimisation du parallélisme");
            
            var optimizer = new ParallelismOptimizer(this);
            await optimizer.OptimizeParallelism(config);
            
            return true;
        }
    }
}