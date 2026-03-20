using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using Argumentum.AssetConverter.Mindmapper;
using AutoMapper;
using ImageMagick;

namespace Argumentum.AssetConverter
{
    public abstract class ParallelDocumentCreatorConfigBase<TDocumentType, TItem>
        where TDocumentType : DocumentConfig, IMindMapDocumentConfig, new()
        where TItem : class, IMindMapItem, new()
    {
        static ParallelDocumentCreatorConfigBase()
        {
            //Attempt to fix missing Type/assembly.
            var tempConverter = new TypeConverter();
        }

        public int MaxDegreeOfParallelismMindMaps { get; set; } = 1;

        public virtual List<TDocumentType> DocumentConfigs { get; set; }

        public async Task Apply(AssetConverterConfig config)
        {
            Logger.LogTitle(GetLogTitle());
            Logger.LogExplanations(GetLogMessage());

            var parallelOptionsDocuments = new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismMindMaps };

            await Task.WhenAll(Enumerable
                .Where(DocumentConfigs, config => config.Enabled)
                .Select(mindMap => ProcessDocumentAsync(mindMap, config, parallelOptionsDocuments)));
        }

        public abstract string GetLogTitle();

        public abstract string GetLogMessage();

        private async Task ProcessDocumentAsync(TDocumentType mindMap,
            AssetConverterConfig assetConverterConfig, ParallelOptions parallelOptions)
        {
            var targetDataset = mindMap.DataSet;
            var objects = await GetObjectsFromDataset(assetConverterConfig, targetDataset);

            var targetLanguages = assetConverterConfig.LocalizationConfig.BuildLanguageList(mindMap.Translations);
            await Parallel.ForEachAsync(targetLanguages, parallelOptions, async (targetLanguage, token) =>
            {
                try
                {
                    var currentTranslatedMap = (TDocumentType)mindMap.Clone();
                    foreach (var documentLocalization in assetConverterConfig.LocalizationConfig.MindMapLocalization)
                    {
                        documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
                    }

                    var documentDirectory = assetConverterConfig.GetDocumentDirectory(targetLanguage);

                    await currentTranslatedMap.GenerateMindMapFile(objects, assetConverterConfig, documentDirectory, targetLanguage);
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            });
        }

        private static async Task<IList> GetObjectsFromDataset(AssetConverterConfig assetConverterConfig, string targetDataset)
        {
            IList objects;
            var dataSet = assetConverterConfig.DataSets.FirstOrDefault(ds => ds.Name == targetDataset);

            Logger.Log($"Dataset lookup for '{targetDataset}':");
            if (dataSet == null)
            {
                Logger.Log($"-> No dataset found in config. Defaulting to generic Load for path '{targetDataset}'.");
                // This part is tricky. We need to call a static method on a generic type TItem.
                var loadMethod = typeof(TItem).GetMethod("Load", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (loadMethod == null)
                {
                    throw new InvalidOperationException($"Could not find static 'Load' method on type {typeof(TItem).Name}.");
                }
                var result = loadMethod.Invoke(null, new object[] { targetDataset });
                objects = ((IEnumerable)result).Cast<object>().ToList();
            }
            else
            {
                var csvTypeName = dataSet.CsvType?.AssemblyQualifiedName ?? "null";
                Logger.Log($"-> Found dataset '{dataSet.Name}' with CsvType: {csvTypeName}");

                if (dataSet.CsvType != null && IsSubclassOfRawGeneric(typeof(CsvBase<,>), dataSet.CsvType))
                {
                    Logger.Log($"--> Type {dataSet.CsvType.Name} is a subclass of CsvBase. Invoking LoadAsync via reflection.");
                    var loadAsyncMethod = dataSet.CsvType.GetMethod("LoadAsync",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                    if (loadAsyncMethod == null)
                    {
                        throw new InvalidOperationException($"Could not find static 'LoadAsync' method on type {dataSet.CsvType.Name}.");
                    }

                    var task = (Task)loadAsyncMethod.Invoke(null,
                        new object[] { dataSet, assetConverterConfig.UseDebugParams });

                    await task.ConfigureAwait(false);
                    var result = (IEnumerable)task.GetType().GetProperty("Result")?.GetValue(task, null);

                    objects = result.Cast<object>().ToList();
                    Logger.Log($"--> Successfully loaded {objects.Count} items for type {dataSet.CsvType.Name}.");
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Dataset type {dataSet.CsvType?.AssemblyQualifiedName ?? "N/A"} is incompatible with mindmap generation");
                }
            }
            return objects;
        }

        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}