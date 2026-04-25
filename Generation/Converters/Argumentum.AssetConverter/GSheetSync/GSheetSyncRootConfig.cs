using System.Collections.Generic;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.GSheetSync
{
	public class GSheetSyncRootConfig
	{
		public List<GSheetSyncConfig> SyncConfigs { get; set; } = new List<GSheetSyncConfig>(new[]
		{
			new GSheetSyncConfig
			{
				Name = "Fallacies Taxonomy",
				SpreadsheetId = "1TrQUyzXMMM-9pHdNWz1fdJ3xQ5XcHgwVH52SOnM61ow",
				Gid = 969304769,
				LocalCsvPath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv",
				PrimaryKeyColumn = "pk",
			},
			new GSheetSyncConfig
			{
				Name = "Scenarii Cards",
				SpreadsheetId = "1SQb9R7Dpi0jPz2JX-HXk1WFn9t68e3aq9MCGif7lM10",
				Gid = 1376497878,
				LocalCsvPath = @"..\..\..\..\..\..\Cards\Scenarii\Argumentum Scenarii - Cards.csv",
				PrimaryKeyColumn = "path",
			},
			new GSheetSyncConfig
			{
				Name = "Virtues Taxonomy",
				SpreadsheetId = "1Asxe0Kb3_pLUSWJnB1HNiBG_EOaz_oU_X3eO9ixnVhA",
				Gid = 349188118,
				LocalCsvPath = @"..\..\..\..\..\..\Cards\Fallacies\Argumentum Virtues - Taxonomy.csv",
				PrimaryKeyColumn = "pk",
			},
			new GSheetSyncConfig
			{
				Name = "Rules Cards",
				SpreadsheetId = "1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ",
				Gid = 0,
				LocalCsvPath = @"..\..\..\..\..\..\Cards\Rules\Argumentum Rules - Cards.csv",
				PrimaryKeyColumn = "pk",
			},
		});

		public async Task Apply(AssetConverterConfig config)
		{
			foreach (var syncConfig in SyncConfigs)
			{
				if (syncConfig.Enabled)
				{
					Logger.LogTitle($"GSheet Sync: {syncConfig.Name}");
					var runner = new GSheetSyncRunner(syncConfig);
					await runner.RunAsync();
					Logger.LogTitle($"GSheet Sync Complete: {syncConfig.Name}");
				}
			}
		}
	}
}
