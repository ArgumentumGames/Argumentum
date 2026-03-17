using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.Optimization
{
    /// <summary>
    /// Classe responsable de l'optimisation des paramètres de parallélisme en fonction des ressources système disponibles.
    /// </summary>
    public class ParallelismOptimizer
    {
        private readonly ParallelismOptimizerConfig _config;
        private SystemResourceInfo _resourceInfo;
        private CancellationTokenSource _monitoringCts;
        private Task _monitoringTask;
        private readonly object _lockObject = new object();
        private DateTime _startTime;
        private PerformanceReport _performanceReport;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ParallelismOptimizer"/>.
        /// </summary>
        /// <param name="config">La configuration de l'optimiseur de parallélisme.</param>
        public ParallelismOptimizer(ParallelismOptimizerConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _performanceReport = new PerformanceReport();
        }

        /// <summary>
        /// Analyse les ressources système disponibles et optimise les paramètres de parallélisme.
        /// </summary>
        /// <param name="config">La configuration globale de l'application.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task OptimizeParallelism(AssetConverterConfig config)
        {
            _startTime = DateTime.Now;
            _performanceReport = new PerformanceReport
            {
                StartTime = _startTime,
                SystemInfo = new SystemInfo()
            };

            Logger.LogTitle("Analyse des ressources système");
            _resourceInfo = await AnalyzeSystemResources();
            
            Logger.LogTitle("Optimisation des paramètres de parallélisme");
            OptimizeParallelismParameters(config);
            
            if (_config.DynamicAdjustment)
            {
                Logger.LogTitle("Démarrage de la surveillance des ressources");
                StartResourceMonitoring(config);
            }
        }
/// <summary>
        /// Analyse les ressources système disponibles (CPU, mémoire, disque, réseau).
        /// </summary>
        /// <returns>Les informations sur les ressources système.</returns>
        public async Task<SystemResourceInfo> AnalyzeSystemResources()
        {
            var resourceInfo = new SystemResourceInfo();

            // Analyse du CPU
            resourceInfo.CpuInfo = await AnalyzeCpu();
            Logger.Log($"CPU: {resourceInfo.CpuInfo.ProcessorCount} cœurs, {resourceInfo.CpuInfo.CurrentUsagePercent}% d'utilisation");
            _performanceReport.SystemInfo.CpuCores = resourceInfo.CpuInfo.ProcessorCount;
            _performanceReport.SystemInfo.CpuModel = resourceInfo.CpuInfo.ProcessorName;
            _performanceReport.SystemInfo.CpuUsage = resourceInfo.CpuInfo.CurrentUsagePercent;

            // Analyse de la mémoire
            resourceInfo.MemoryInfo = await AnalyzeMemory();
            Logger.Log($"Mémoire: {FormatBytes(resourceInfo.MemoryInfo.TotalPhysicalMemory)} totale, {FormatBytes(resourceInfo.MemoryInfo.AvailablePhysicalMemory)} disponible ({resourceInfo.MemoryInfo.UsagePercent}% utilisé)");
            _performanceReport.SystemInfo.TotalMemory = resourceInfo.MemoryInfo.TotalPhysicalMemory;
            _performanceReport.SystemInfo.AvailableMemory = resourceInfo.MemoryInfo.AvailablePhysicalMemory;
            _performanceReport.SystemInfo.MemoryUsage = resourceInfo.MemoryInfo.UsagePercent;

            // Analyse du disque
            resourceInfo.DiskInfo = await AnalyzeDisk();
            Logger.Log($"Disque: {FormatBytes(resourceInfo.DiskInfo.TotalSpace)} total, {FormatBytes(resourceInfo.DiskInfo.AvailableSpace)} disponible, {resourceInfo.DiskInfo.AverageReadSpeed} Mo/s en lecture, {resourceInfo.DiskInfo.AverageWriteSpeed} Mo/s en écriture");
            _performanceReport.SystemInfo.TotalDiskSpace = resourceInfo.DiskInfo.TotalSpace;
            _performanceReport.SystemInfo.AvailableDiskSpace = resourceInfo.DiskInfo.AvailableSpace;
            _performanceReport.SystemInfo.DiskReadSpeed = resourceInfo.DiskInfo.AverageReadSpeed;
            _performanceReport.SystemInfo.DiskWriteSpeed = resourceInfo.DiskInfo.AverageWriteSpeed;

            // Analyse du réseau
            resourceInfo.NetworkInfo = await AnalyzeNetwork();
            Logger.Log($"Réseau: {resourceInfo.NetworkInfo.AverageBandwidth} Mbps, {resourceInfo.NetworkInfo.AverageLatency} ms de latence");
            _performanceReport.SystemInfo.NetworkBandwidth = resourceInfo.NetworkInfo.AverageBandwidth;
            _performanceReport.SystemInfo.NetworkLatency = resourceInfo.NetworkInfo.AverageLatency;

            return resourceInfo;
        }

        /// <summary>
        /// Analyse les informations sur le CPU.
        /// </summary>
        /// <returns>Les informations sur le CPU.</returns>
        private async Task<CpuInfo> AnalyzeCpu()
        {
            var cpuInfo = new CpuInfo();
            
            // Obtenir le nombre de processeurs logiques
            cpuInfo.ProcessorCount = Environment.ProcessorCount;
            
            try
            {
                // Obtenir le nom du processeur et d'autres informations
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            cpuInfo.ProcessorName = obj["Name"]?.ToString() ?? "Unknown";
                            break;
                        }
                    }
                }
                else
                {
                    // Pour Linux/macOS, on pourrait utiliser des commandes shell
                    cpuInfo.ProcessorName = "Non-Windows CPU";
                }
                
                // Mesurer l'utilisation actuelle du CPU
                var startTime = DateTime.UtcNow;
                var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
                
                // Attendre un court instant pour mesurer l'utilisation
                await Task.Delay(500);
                
                var endTime = DateTime.UtcNow;
                var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
                
                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
                
                cpuInfo.CurrentUsagePercent = (int)(cpuUsageTotal * 100);
                
                // Obtenir l'utilisation globale du système
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            cpuInfo.SystemUsagePercent = Convert.ToInt32(obj["PercentProcessorTime"]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Erreur lors de l'analyse du CPU: {ex.Message}");
                // Valeurs par défaut en cas d'erreur
                cpuInfo.CurrentUsagePercent = 50;
                cpuInfo.SystemUsagePercent = 50;
            }
            
            return cpuInfo;
        }
/// <summary>
        /// Analyse les informations sur la mémoire.
        /// </summary>
        /// <returns>Les informations sur la mémoire.</returns>
        private async Task<MemoryInfo> AnalyzeMemory()
        {
            var memoryInfo = new MemoryInfo();
            
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            memoryInfo.TotalPhysicalMemory = Convert.ToInt64(obj["TotalVisibleMemorySize"]) * 1024; // KB to bytes
                            memoryInfo.AvailablePhysicalMemory = Convert.ToInt64(obj["FreePhysicalMemory"]) * 1024; // KB to bytes
                            break;
                        }
                    }
                }
                else
                {
                    // Pour Linux/macOS, on pourrait utiliser des commandes shell
                    memoryInfo.TotalPhysicalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
                    memoryInfo.AvailablePhysicalMemory = memoryInfo.TotalPhysicalMemory / 2; // Estimation
                }
                
                memoryInfo.UsedPhysicalMemory = memoryInfo.TotalPhysicalMemory - memoryInfo.AvailablePhysicalMemory;
                memoryInfo.UsagePercent = (int)((double)memoryInfo.UsedPhysicalMemory / memoryInfo.TotalPhysicalMemory * 100);
                
                // Obtenir l'utilisation de la mémoire du processus actuel
                using (var process = Process.GetCurrentProcess())
                {
                    memoryInfo.ProcessMemoryUsage = process.WorkingSet64;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Erreur lors de l'analyse de la mémoire: {ex.Message}");
                // Valeurs par défaut en cas d'erreur
                memoryInfo.TotalPhysicalMemory = 8L * 1024 * 1024 * 1024; // 8 GB
                memoryInfo.AvailablePhysicalMemory = 4L * 1024 * 1024 * 1024; // 4 GB
                memoryInfo.UsedPhysicalMemory = 4L * 1024 * 1024 * 1024; // 4 GB
                memoryInfo.UsagePercent = 50;
            }
            
            return memoryInfo;
        }

        /// <summary>
        /// Analyse les informations sur le disque.
        /// </summary>
        /// <returns>Les informations sur le disque.</returns>
        private async Task<DiskInfo> AnalyzeDisk()
        {
            var diskInfo = new DiskInfo();
            
            try
            {
                // Obtenir les informations sur l'espace disque
                var currentDirectory = Environment.CurrentDirectory;
                var driveInfo = new DriveInfo(Path.GetPathRoot(currentDirectory));
                
                diskInfo.TotalSpace = driveInfo.TotalSize;
                diskInfo.AvailableSpace = driveInfo.AvailableFreeSpace;
                diskInfo.UsedSpace = diskInfo.TotalSpace - diskInfo.AvailableSpace;
                diskInfo.UsagePercent = (int)((double)diskInfo.UsedSpace / diskInfo.TotalSpace * 100);
                
                // Mesurer les performances de lecture/écriture
                var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");
                const int bufferSize = 1024 * 1024 * 10; // 10 MB
                var buffer = new byte[bufferSize];
                new Random().NextBytes(buffer);
                
                // Test d'écriture
                var stopwatch = Stopwatch.StartNew();
                using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                {
                    await fs.WriteAsync(buffer, 0, buffer.Length);
                    await fs.FlushAsync();
                }
                stopwatch.Stop();
                var writeTime = stopwatch.ElapsedMilliseconds;
                diskInfo.AverageWriteSpeed = bufferSize / (1024.0 * 1024.0) / (writeTime / 1000.0);
                
                // Test de lecture
                stopwatch.Restart();
                using (var fs = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                {
                    await fs.ReadAsync(buffer, 0, buffer.Length);
                }
                stopwatch.Stop();
                var readTime = stopwatch.ElapsedMilliseconds;
                diskInfo.AverageReadSpeed = bufferSize / (1024.0 * 1024.0) / (readTime / 1000.0);
                
                // Nettoyer
                try
                {
                    File.Delete(tempFile);
                }
                catch
                {
                    // Ignorer les erreurs de nettoyage
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Erreur lors de l'analyse du disque: {ex.Message}");
                // Valeurs par défaut en cas d'erreur
                diskInfo.TotalSpace = 500L * 1024 * 1024 * 1024; // 500 GB
                diskInfo.AvailableSpace = 250L * 1024 * 1024 * 1024; // 250 GB
                diskInfo.UsedSpace = 250L * 1024 * 1024 * 1024; // 250 GB
                diskInfo.UsagePercent = 50;
                diskInfo.AverageReadSpeed = 100; // 100 MB/s
                diskInfo.AverageWriteSpeed = 50; // 50 MB/s
            }
            
            return diskInfo;
        }
/// <summary>
        /// Analyse les informations sur le réseau.
        /// </summary>
        /// <returns>Les informations sur le réseau.</returns>
        private async Task<NetworkInfo> AnalyzeNetwork()
        {
            var networkInfo = new NetworkInfo();
            
            try
            {
                // Mesurer la latence
                var pingTasks = new List<Task<long>>();
                var hosts = new[] { "8.8.8.8", "1.1.1.1", "208.67.222.222" }; // Google DNS, Cloudflare DNS, OpenDNS
                
                foreach (var host in hosts)
                {
                    pingTasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            using (var ping = new Ping())
                            {
                                var reply = await ping.SendPingAsync(host, 1000);
                                return reply.Status == IPStatus.Success ? reply.RoundtripTime : 0;
                            }
                        }
                        catch
                        {
                            return 0;
                        }
                    }));
                }
                
                await Task.WhenAll(pingTasks);
                var validPings = pingTasks.Select(t => t.Result).Where(r => r > 0).ToList();
                networkInfo.AverageLatency = validPings.Any() ? validPings.Average() : 50; // Valeur par défaut: 50ms
                
                // Estimer la bande passante en fonction des interfaces réseau actives
                var activeNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                                 ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                    .ToList();
                
                if (activeNetworkInterfaces.Any())
                {
                    // Prendre la vitesse maximale parmi les interfaces actives
                    networkInfo.AverageBandwidth = activeNetworkInterfaces.Max(ni => ni.Speed) / 1_000_000.0; // bits/s to Mbps
                }
                else
                {
                    networkInfo.AverageBandwidth = 100; // Valeur par défaut: 100 Mbps
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Erreur lors de l'analyse du réseau: {ex.Message}");
                // Valeurs par défaut en cas d'erreur
                networkInfo.AverageLatency = 50; // 50 ms
                networkInfo.AverageBandwidth = 100; // 100 Mbps
            }
            
            return networkInfo;
        }

        /// <summary>
        /// Optimise les paramètres de parallélisme en fonction des ressources système.
        /// </summary>
        /// <param name="config">La configuration globale de l'application.</param>
        public void OptimizeParallelismParameters(AssetConverterConfig config)
        {
            if (config == null || _resourceInfo == null)
            {
                Logger.LogWarning("Impossible d'optimiser les paramètres de parallélisme: configuration ou informations sur les ressources manquantes.");
                return;
            }

            // Récupérer les valeurs actuelles pour les inclure dans le rapport
            _performanceReport.OriginalParameters = new ParallelismParameters
            {
                MaxDegreeOfParallelismCardpen = config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpen,
                MaxDegreeOfParallelismCardpenTranslations = config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpenTranslations,
                MaxDegreeOfParallelismImages = config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImages,
                MaxDegreeOfParallelismImageTranslations = config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImageTranslations,
                MaxDegreeOfParallelismDocuments = config.WebBasedGeneratorConfig.MaxDegreeOfParallelismDocuments
            };

            // Calculer les paramètres optimaux
            var cpuCores = _resourceInfo.CpuInfo.ProcessorCount;
            var memoryAvailableGB = _resourceInfo.MemoryInfo.AvailablePhysicalMemory / (1024.0 * 1024 * 1024);
            var diskSpeedFactor = Math.Min(_resourceInfo.DiskInfo.AverageReadSpeed / 100.0, 1.0); // Normaliser à 100 MB/s
            var networkFactor = Math.Min(_resourceInfo.NetworkInfo.AverageBandwidth / 100.0, 1.0); // Normaliser à 100 Mbps
            
            // Facteur global basé sur les ressources disponibles et leur pondération
            var cpuFactor = (1.0 - (_resourceInfo.CpuInfo.SystemUsagePercent / 100.0)) * _config.CpuWeightFactor;
            var memoryFactor = (1.0 - (_resourceInfo.MemoryInfo.UsagePercent / 100.0)) * _config.MemoryWeightFactor;
            var resourceFactor = (cpuFactor + memoryFactor + diskSpeedFactor * _config.DiskWeightFactor + networkFactor * _config.NetworkWeightFactor) / 
                                (_config.CpuWeightFactor + _config.MemoryWeightFactor + _config.DiskWeightFactor + _config.NetworkWeightFactor);
            
            // Calculer les valeurs optimales en fonction des ressources
            var optimalCardpen = Math.Max(
                _config.MinThreadsCardpen,
                (int)Math.Round(cpuCores * 0.5 * resourceFactor)
            );
            
            var optimalCardpenTranslations = Math.Max(
                _config.MinThreadsCardpenTranslations,
                (int)Math.Round(cpuCores * 0.3 * resourceFactor)
            );
            
            var optimalImages = Math.Max(
                _config.MinThreadsImages,
                (int)Math.Round(cpuCores * 0.6 * resourceFactor)
            );
            
            var optimalImageTranslations = Math.Max(
                _config.MinThreadsImageTranslations,
                (int)Math.Round(cpuCores * 0.4 * resourceFactor)
            );
            
            var optimalDocuments = Math.Max(
                _config.MinThreadsDocuments,
                (int)Math.Round(cpuCores * 0.7 * resourceFactor)
            );
            
            // Appliquer les valeurs optimisées
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpen = optimalCardpen;
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpenTranslations = optimalCardpenTranslations;
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImages = optimalImages;
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImageTranslations = optimalImageTranslations;
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismDocuments = optimalDocuments;
            
            // Enregistrer les paramètres optimisés dans le rapport
            _performanceReport.OptimizedParameters = new ParallelismParameters
            {
                MaxDegreeOfParallelismCardpen = optimalCardpen,
                MaxDegreeOfParallelismCardpenTranslations = optimalCardpenTranslations,
                MaxDegreeOfParallelismImages = optimalImages,
                MaxDegreeOfParallelismImageTranslations = optimalImageTranslations,
                MaxDegreeOfParallelismDocuments = optimalDocuments
            };
            
            // Journaliser les paramètres optimisés
            Logger.LogSuccess("Paramètres de parallélisme optimisés:");
            Logger.Log($"MaxDegreeOfParallelismCardpen: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpen} (était {_performanceReport.OriginalParameters.MaxDegreeOfParallelismCardpen})");
            Logger.Log($"MaxDegreeOfParallelismCardpenTranslations: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpenTranslations} (était {_performanceReport.OriginalParameters.MaxDegreeOfParallelismCardpenTranslations})");
            Logger.Log($"MaxDegreeOfParallelismImages: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImages} (était {_performanceReport.OriginalParameters.MaxDegreeOfParallelismImages})");
            Logger.Log($"MaxDegreeOfParallelismImageTranslations: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImageTranslations} (était {_performanceReport.OriginalParameters.MaxDegreeOfParallelismImageTranslations})");
            Logger.Log($"MaxDegreeOfParallelismDocuments: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismDocuments} (était {_performanceReport.OriginalParameters.MaxDegreeOfParallelismDocuments})");
        }
/// <summary>
        /// Démarre la surveillance des ressources système pour ajuster dynamiquement les paramètres de parallélisme.
        /// </summary>
        /// <param name="config">La configuration globale de l'application.</param>
        public void StartResourceMonitoring(AssetConverterConfig config)
        {
            if (!_config.DynamicAdjustment)
            {
                return;
            }
            
            _monitoringCts = new CancellationTokenSource();
            _monitoringTask = Task.Run(async () =>
            {
                try
                {
                    Logger.Log("Surveillance des ressources système démarrée.");
                    
                    while (!_monitoringCts.Token.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(_config.MonitoringIntervalSeconds), _monitoringCts.Token);
                        
                        try
                        {
                            // Analyser les ressources actuelles
                            var currentResources = await AnalyzeSystemResources();
                            
                            // Vérifier si un ajustement est nécessaire
                            if (currentResources.CpuInfo.SystemUsagePercent > _config.TargetCpuUsagePercent ||
                                currentResources.MemoryInfo.UsagePercent > _config.TargetMemoryUsagePercent)
                            {
                                Logger.LogWarning($"Utilisation élevée des ressources détectée: CPU {currentResources.CpuInfo.SystemUsagePercent}%, Mémoire {currentResources.MemoryInfo.UsagePercent}%");
                                
                                // Réduire le parallélisme
                                lock (_lockObject)
                                {
                                    AdjustParallelismParameters(config, 0.8);
                                }
                                
                                // Enregistrer l'ajustement dans le rapport
                                _performanceReport.ResourceAdjustments.Add(new ResourceAdjustment
                                {
                                    Timestamp = DateTime.Now,
                                    CpuUsage = currentResources.CpuInfo.SystemUsagePercent,
                                    MemoryUsage = currentResources.MemoryInfo.UsagePercent,
                                    AdjustmentFactor = 0.8,
                                    Reason = "Utilisation élevée des ressources"
                                });
                            }
                            else if (currentResources.CpuInfo.SystemUsagePercent < _config.TargetCpuUsagePercent * 0.7 &&
                                     currentResources.MemoryInfo.UsagePercent < _config.TargetMemoryUsagePercent * 0.7)
                            {
                                Logger.Log($"Utilisation faible des ressources détectée: CPU {currentResources.CpuInfo.SystemUsagePercent}%, Mémoire {currentResources.MemoryInfo.UsagePercent}%");
                                
                                // Augmenter le parallélisme
                                lock (_lockObject)
                                {
                                    AdjustParallelismParameters(config, 1.2);
                                }
                                
                                // Enregistrer l'ajustement dans le rapport
                                _performanceReport.ResourceAdjustments.Add(new ResourceAdjustment
                                {
                                    Timestamp = DateTime.Now,
                                    CpuUsage = currentResources.CpuInfo.SystemUsagePercent,
                                    MemoryUsage = currentResources.MemoryInfo.UsagePercent,
                                    AdjustmentFactor = 1.2,
                                    Reason = "Utilisation faible des ressources"
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogWarning($"Erreur lors de la surveillance des ressources: {ex.Message}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Annulation normale
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Erreur dans la tâche de surveillance: {ex.Message}");
                }
                finally
                {
                    Logger.Log("Surveillance des ressources système arrêtée.");
                }
            });
        }

        /// <summary>
        /// Ajuste les paramètres de parallélisme par un facteur donné.
        /// </summary>
        /// <param name="config">La configuration globale de l'application.</param>
        /// <param name="factor">Le facteur d'ajustement (< 1 pour réduire, > 1 pour augmenter).</param>
        private void AdjustParallelismParameters(AssetConverterConfig config, double factor)
        {
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpen = Math.Max(
                _config.MinThreadsCardpen,
                (int)Math.Round(config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpen * factor)
            );
            
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpenTranslations = Math.Max(
                _config.MinThreadsCardpenTranslations,
                (int)Math.Round(config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpenTranslations * factor)
            );
            
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImages = Math.Max(
                _config.MinThreadsImages,
                (int)Math.Round(config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImages * factor)
            );
            
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImageTranslations = Math.Max(
                _config.MinThreadsImageTranslations,
                (int)Math.Round(config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImageTranslations * factor)
            );
            
            config.WebBasedGeneratorConfig.MaxDegreeOfParallelismDocuments = Math.Max(
                _config.MinThreadsDocuments,
                (int)Math.Round(config.WebBasedGeneratorConfig.MaxDegreeOfParallelismDocuments * factor)
            );
            
            Logger.Log($"Paramètres de parallélisme ajustés (facteur: {factor}):");
            Logger.Log($"MaxDegreeOfParallelismCardpen: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpen}");
            Logger.Log($"MaxDegreeOfParallelismCardpenTranslations: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismCardpenTranslations}");
            Logger.Log($"MaxDegreeOfParallelismImages: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImages}");
            Logger.Log($"MaxDegreeOfParallelismImageTranslations: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismImageTranslations}");
            Logger.Log($"MaxDegreeOfParallelismDocuments: {config.WebBasedGeneratorConfig.MaxDegreeOfParallelismDocuments}");
        }

        /// <summary>
        /// Génère un rapport de performance sur l'optimisation du parallélisme.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task GeneratePerformanceReport()
        {
            _performanceReport.EndTime = DateTime.Now;
            _performanceReport.TotalDuration = _performanceReport.EndTime - _performanceReport.StartTime;
            
            // Ajouter des recommandations basées sur les ajustements effectués
            if (_performanceReport.ResourceAdjustments.Count > 0)
            {
                var cpuAdjustments = _performanceReport.ResourceAdjustments.Where(a => a.CpuUsage > _config.TargetCpuUsagePercent).Count();
                var memoryAdjustments = _performanceReport.ResourceAdjustments.Where(a => a.MemoryUsage > _config.TargetMemoryUsagePercent).Count();
                
                if (cpuAdjustments > memoryAdjustments)
                {
                    _performanceReport.Recommendations.Add("Le CPU semble être le facteur limitant. Envisagez de réduire le parallélisme global ou d'améliorer les performances du CPU.");
                }
                else if (memoryAdjustments > cpuAdjustments)
                {
                    _performanceReport.Recommendations.Add("La mémoire semble être le facteur limitant. Envisagez d'augmenter la mémoire disponible ou de réduire la consommation mémoire des opérations.");
                }
                
                if (_performanceReport.ResourceAdjustments.Count(a => a.AdjustmentFactor < 1) > _performanceReport.ResourceAdjustments.Count(a => a.AdjustmentFactor > 1))
                {
                    _performanceReport.Recommendations.Add("Le système a dû réduire le parallélisme plusieurs fois. Les valeurs initiales étaient probablement trop élevées.");
                }
                else if (_performanceReport.ResourceAdjustments.Count(a => a.AdjustmentFactor > 1) > _performanceReport.ResourceAdjustments.Count(a => a.AdjustmentFactor < 1))
                {
                    _performanceReport.Recommendations.Add("Le système a pu augmenter le parallélisme plusieurs fois. Les valeurs initiales étaient probablement trop conservatrices.");
                }
            }
            
            // Sauvegarder le rapport au format JSON
            if (_config.GenerateDetailedReport)
            {
                try
                {
                    var reportJson = JsonSerializer.Serialize(_performanceReport, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(_config.PerformanceReportPath, reportJson);
                    Logger.LogSuccess($"Rapport de performance généré: {_config.PerformanceReportPath}");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Erreur lors de la génération du rapport de performance: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Formate un nombre d'octets en une chaîne lisible (KB, MB, GB, etc.).
        /// </summary>
        /// <param name="bytes">Le nombre d'octets à formater.</param>
        /// <returns>Une chaîne formatée.</returns>
        private string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }
    }

    #region Classes de données pour l'optimisation du parallélisme

    /// <summary>
    /// Informations sur les ressources système.
    /// </summary>
    public class SystemResourceInfo
    {
        public CpuInfo CpuInfo { get; set; } = new CpuInfo();
        public MemoryInfo MemoryInfo { get; set; } = new MemoryInfo();
        public DiskInfo DiskInfo { get; set; } = new DiskInfo();
        public NetworkInfo NetworkInfo { get; set; } = new NetworkInfo();
    }

    /// <summary>
    /// Informations sur le CPU.
    /// </summary>
    public class CpuInfo
    {
        public int ProcessorCount { get; set; }
        public string ProcessorName { get; set; } = "Unknown";
        public int CurrentUsagePercent { get; set; }
        public int SystemUsagePercent { get; set; }
    }

    /// <summary>
    /// Informations sur la mémoire.
    /// </summary>
    public class MemoryInfo
    {
        public long TotalPhysicalMemory { get; set; }
        public long AvailablePhysicalMemory { get; set; }
        public long UsedPhysicalMemory { get; set; }
        public int UsagePercent { get; set; }
        public long ProcessMemoryUsage { get; set; }
    }

    /// <summary>
    /// Informations sur le disque.
    /// </summary>
    public class DiskInfo
    {
        public long TotalSpace { get; set; }
        public long AvailableSpace { get; set; }
        public long UsedSpace { get; set; }
        public int UsagePercent { get; set; }
        public double AverageReadSpeed { get; set; }
        public double AverageWriteSpeed { get; set; }
    }

    /// <summary>
    /// Informations sur le réseau.
    /// </summary>
    public class NetworkInfo
    {
        public double AverageBandwidth { get; set; }
        public double AverageLatency { get; set; }
    }

    /// <summary>
    /// Rapport de performance pour l'optimisation du parallélisme.
    /// </summary>
    public class PerformanceReport
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public SystemInfo SystemInfo { get; set; } = new SystemInfo();
        public ParallelismParameters OriginalParameters { get; set; } = new ParallelismParameters();
        public ParallelismParameters OptimizedParameters { get; set; } = new ParallelismParameters();
        public List<ResourceAdjustment> ResourceAdjustments { get; set; } = new List<ResourceAdjustment>();
        public List<string> Recommendations { get; set; } = new List<string>();
    }

    /// <summary>
    /// Informations système pour le rapport de performance.
    /// </summary>
    public class SystemInfo
    {
        public int CpuCores { get; set; }
        public string CpuModel { get; set; } = "Unknown";
        public int CpuUsage { get; set; }
        public long TotalMemory { get; set; }
        public long AvailableMemory { get; set; }
        public int MemoryUsage { get; set; }
        public long TotalDiskSpace { get; set; }
        public long AvailableDiskSpace { get; set; }
        public double DiskReadSpeed { get; set; }
        public double DiskWriteSpeed { get; set; }
        public double NetworkBandwidth { get; set; }
        public double NetworkLatency { get; set; }
    }

    /// <summary>
    /// Paramètres de parallélisme pour le rapport de performance.
    /// </summary>
    public class ParallelismParameters
    {
        public int MaxDegreeOfParallelismCardpen { get; set; }
        public int MaxDegreeOfParallelismCardpenTranslations { get; set; }
        public int MaxDegreeOfParallelismImages { get; set; }
        public int MaxDegreeOfParallelismImageTranslations { get; set; }
        public int MaxDegreeOfParallelismDocuments { get; set; }
    }

    /// <summary>
    /// Ajustement des ressources pour le rapport de performance.
    /// </summary>
    public class ResourceAdjustment
    {
        public DateTime Timestamp { get; set; }
        public int CpuUsage { get; set; }
        public int MemoryUsage { get; set; }
        public double AdjustmentFactor { get; set; }
        public string Reason { get; set; } = "";
    }

    #endregion
}