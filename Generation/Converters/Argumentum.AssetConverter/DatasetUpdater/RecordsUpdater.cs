using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Argumentum.AssetConverter;

public class RecordsUpdater
{
	public string PrimaryKeyField { get; set; }

	public List<Dictionary<string, object>> Records { get; set; }

	public string UpdateRecord(string primaryKey, string fieldName, string newValue)
	{
		newValue = DecodeValue(newValue);
		var targetRecord = Records.FirstOrDefault(x => x[PrimaryKeyField].ToString() == primaryKey);
		if (targetRecord == null)
		{
			return $"target record not found for pk={primaryKey}";
		}
		if (!targetRecord.ContainsKey(fieldName))
		{
			return $"field '{fieldName}' not found in record {primaryKey}. Available: {string.Join(", ", targetRecord.Keys)}";
		}
		var existingValue = targetRecord[fieldName];
		targetRecord[fieldName] = newValue;
		return $"{existingValue}\n==>\n{newValue}";
	}

	private static string DecodeValue(string newValue)
	{
		newValue = Regex.Unescape(newValue);
		newValue = HttpUtility.HtmlDecode(newValue);
		newValue = HttpUtility.UrlDecode(newValue);
		return newValue;
	}

	//[FunctionDescription("Given a field name to update, updates a series of 5 records given their primary keys and the new value for that field, returns lines of old and new values updated")]
	//public string UpdateFiveRecords(string fieldName, string primaryKey1, string newValue1,
	//	string primaryKey2, string newValue2,
	//	string primaryKey3, string newValue3,
	//	string primaryKey4, string newValue4,
	//	string primaryKey5, string newValue5)
	//{
	//	var updates = new Dictionary<string, string>()
	//	{
	//		{ primaryKey1, newValue1 },
	//		{ primaryKey2, newValue2 },
	//		{ primaryKey3, newValue3 },
	//		{ primaryKey4, newValue4 },
	//		{ primaryKey5, newValue5 }
	//	};
	//	var toReturn = new StringBuilder();
	//	foreach (var update in updates)
	//	{
	//		var targetRecord = Records.FirstOrDefault(x => x[PrimaryKeyField].ToString() == update.Key);
	//		if (targetRecord == null)
	//		{
	//			toReturn.Append($"target record not found for {update.Key}");
	//		}
	//		else
	//		{
	//			var existingValue = targetRecord[fieldName];
	//			targetRecord[fieldName] = update.Value;
	//			toReturn.Append($"{existingValue} => {update.Value}");
	//		}
	//	}
	//	return toReturn.ToString();
	//}


}