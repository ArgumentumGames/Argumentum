using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Argumentum.AssetConverter;

public class DocumentLocalization
{

	public List<(string sourceText, List<(string Language, string destText)> textConversions)> StaticConversions { get; set; }
		= new List<(string sourceText, List<(string Language, string destText)> textConversions)>();


	public string DoStaticConversions(string template, string destLang)
	{
		foreach (var staticConversion in StaticConversions)
		{
			var convertedText =
				staticConversion.textConversions.First(convertedText => convertedText.Language == destLang).destText;
			template = template.Replace(staticConversion.sourceText, convertedText);
		}
		return template;
	}

	public List<string> TargetProperties { get; set; } = new List<string>();

	private Dictionary<Type, PropertyInfo[]> filteredProperties = new Dictionary<Type, PropertyInfo[]>();

	public void DoReflectionTranslate(object targetObject, string destLang)
	{
		var targetType = targetObject.GetType();
		if (!filteredProperties.TryGetValue(targetType, out PropertyInfo[] properties))
		{
			properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => TargetProperties.Contains(prop.Name) && prop.PropertyType == typeof(string))
				.ToArray();
			filteredProperties[targetType] = properties;
		}

		foreach (var prop in properties)
		{
			var value = (string)prop.GetValue(targetObject);
			var translatedValue = DoStaticConversions(value, destLang);
			prop.SetValue(targetObject, translatedValue);
		}
	}

	

}