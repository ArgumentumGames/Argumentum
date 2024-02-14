using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Argumentum.AssetConverter.Entities;
using AutoMapper;
using ImageMagick;

namespace Argumentum.AssetConverter;

public abstract class ParallelFallacyDocumentCreatorConfigBase<TDocumentType> where TDocumentType : FallacyDocumentConfigBase, new()
{
	static ParallelFallacyDocumentCreatorConfigBase()
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
			.Where<TDocumentType>(DocumentConfigs, config => config.Enabled)
			.Select<TDocumentType, Task>(mindMap => ProcessFallacyDocumentAsync(mindMap, config, parallelOptionsDocuments)));

	}

	

	public abstract string GetLogTitle();

	public abstract string GetLogMessage();

	private async Task ProcessFallacyDocumentAsync(TDocumentType mindMap,
		AssetConverterConfig assetConverterConfig, ParallelOptions parallelOptions)
	{

		var targetDataset = mindMap.DataSet;
		var fallacies = await GetFallaciesFromDataset(assetConverterConfig, targetDataset);

		var targetLanguages = assetConverterConfig.LocalizationConfig.BuildLanguageList(mindMap.Translations);
		await Parallel.ForEachAsync(targetLanguages, parallelOptions, async (targetLanguage, token) =>
		{
			try
			{
				var currentTranslatedMap = (TDocumentType) mindMap.Clone();
				foreach (var documentLocalization in assetConverterConfig.LocalizationConfig.MindMapLocalization)
				{
					documentLocalization.DoReflectionTranslate(currentTranslatedMap, targetLanguage);
				}

				var documentDirectory = assetConverterConfig.GetDocumentDirectory(targetLanguage);

				await currentTranslatedMap.GenerateFallacyFile(fallacies, assetConverterConfig, documentDirectory, targetLanguage);
			}
			catch (Exception e)
			{
				Logger.LogException(e);
			}
		});




	}

	private static async Task<IList<Fallacy>> GetFallaciesFromDataset(AssetConverterConfig assetConverterConfig, string targetDataset)
	{
		IList<Fallacy> fallacies;
		var dataSet = assetConverterConfig.DataSets.FirstOrDefault(ds => ds.Name == targetDataset);
		if (dataSet == null)
		{
			fallacies = Fallacy.Load(targetDataset);
		}
		else
		{
			if (dataSet.CsvType != null && dataSet.CsvType != typeof(Fallacy))
			{
				if (IsSubclassOfRawGeneric(typeof(CsvBase<,>), dataSet.CsvType))
				{
					var config = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
					var mapper = config.CreateMapper();

					// Trouver la m�thode LoadAsync sur le type sp�cifique
					var loadAsyncMethod = dataSet.CsvType.GetMethod("LoadAsync",
						BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);


					// Invoquer la m�thode
					var task = (Task)loadAsyncMethod.Invoke(null,
						new object[] { dataSet, assetConverterConfig.UseDebugParams });

					// Attendre la fin de la t�che et obtenir le r�sultat
					await task.ConfigureAwait(false);
					var result = (IEnumerable)task.GetType().GetProperty("Result")?.GetValue(task, null);

					// Mapper les objets � Fallacy
					fallacies = (from object baseObject in result select mapper.Map<Fallacy>(baseObject)).ToList();
				}
				else
				{
					throw new InvalidOperationException(
						$"Dataset type {dataSet.CsvType.AssemblyQualifiedName} is incompatible with mindmap generation");
				}
			}
			else
			{
				fallacies = await Fallacy.LoadAsync(dataSet, assetConverterConfig.UseDebugParams);
			}
		}

		return fallacies;
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