using System;
using Argumentum.AssetConverter.GSheetSync;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.GSheetSync
{
	/// <summary>
	/// #1618 points 3-5 — backup tab titles are collision-proof at millisecond
	/// precision, and the partial-upload failure message names the restore path.
	/// </summary>
	public class GSheetServiceGuardMessageTests
	{
		[Fact]
		public void BuildBackupTitle_KeepsSecondPrecision_AndAddsMilliseconds()
		{
			var t = new DateTime(2026, 9, 29, 21, 0, 0, 0);

			GSheetService.BuildBackupTitle(t).Should().Be("Backup 2026-09-29 21-00-00-000");
		}

		[Fact]
		public void BuildBackupTitle_TwoUploadsInSameSecond_DoNotCollide()
		{
			var t1 = new DateTime(2026, 9, 29, 21, 0, 0, 0);
			var t2 = new DateTime(2026, 9, 29, 21, 0, 0, 250);

			GSheetService.BuildBackupTitle(t1).Should()
				.NotBe(GSheetService.BuildBackupTitle(t2));
		}

		[Fact]
		public void BuildBatchUpdateMismatchMessage_NamesCountsAndRestorePath()
		{
			var message = GSheetService.BuildBatchUpdateMismatchMessage(10, 7, "Fallacies");

			message.Should().Contain("10").And.Contain("7");
			message.Should().Contain("NOT rolled back");
			message.Should().Contain("Backup");
			message.Should().Contain("Fallacies");
		}
	}
}