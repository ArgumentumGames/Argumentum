using System.Collections.Generic;

namespace Argumentum.AssetConverter;

public class CardSetJob
{
	public string Name;
	public CardSetConfig Config;
	public List<(string sourceLanguage, string targetLanguage)> Translations;

	public static IEqualityComparer<CardSetJob> Comparer =
		EqualityComparerFactory.Create<CardSetJob>(arg => arg.Name.GetHashCode(),
			(obj1, obj2) => obj1.Name == obj2.Name);
}