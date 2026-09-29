using System;
using System.IO;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	/// <summary>
	/// #1618 point 9 — when the primary-key column is absent, the row matcher falls
	/// back to positional indexing. That fallback must be announced, like its
	/// neighbours (duplicate keys, empty keys) already are.
	/// Shares the "GSheetSyncConsoleCapture" collection with the runner tests,
	/// which also redirect Console.Out.
	/// </summary>
	[Collection("GSheetSyncConsoleCapture")]
	public class CsvDiffEnginePositionFallbackWarningTests
	{
		[Fact]
		public void Compare_MissingPrimaryKeyColumn_WarnsAboutPositionalMatching()
		{
			var engine = new CsvDiffEngine("pk");
			var oldCsv = "name,value\nalpha,100\n";
			var newCsv = "name,value\nalpha,200\n";

			var originalOut = Console.Out;
			var sw = new StringWriter();
			Console.SetOut(sw);
			try
			{
				engine.Compare(oldCsv, newCsv);
			}
			finally
			{
				Console.SetOut(originalOut);
			}

			sw.ToString().Should().Contain("matched by position");
		}
	}
}