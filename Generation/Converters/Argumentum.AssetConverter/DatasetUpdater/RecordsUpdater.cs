using System.Collections.Generic;
using System.Linq;
using OpenAI.Utilities.FunctionCalling;

namespace Argumentum.AssetConverter;

public class RecordsUpdater
{
	public string PrimaryKeyField { get; set; }
	public List<Dictionary<string, object>> Records { get; set; }

	[FunctionDescription("Updates a record's field given its primary key, the field's name and the new value for that field")]
	public bool UpdateRecord(string primaryKey, string fieldName, string newValue)
	{
		var targetRecord = Records.FirstOrDefault(x => x[PrimaryKeyField].ToString() == primaryKey);
		if (targetRecord == null)
		{
			return false;
		}
		targetRecord[fieldName] = newValue;
		return true;
	}
}