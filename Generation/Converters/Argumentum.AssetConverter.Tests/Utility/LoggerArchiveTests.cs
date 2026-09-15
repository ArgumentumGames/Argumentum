using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Utility
{
    /// <summary>
    /// #1179 guard: the generation log of a run must survive the CMYK pass of the same run.
    /// The old static ctor deleted file_logger.log at startup — the follow-up CMYK process erased
    /// the generation log and its originating exception, costing a full investigation cycle on
    /// #1177. The previous log is now archived to file_logger-&lt;timestamp&gt;.log.
    /// </summary>
    public class LoggerArchiveTests : IDisposable
    {
        private readonly string _tempDir;

        public LoggerArchiveTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ArgumentumTests_LoggerArchive", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
        }

        [Fact]
        public void ArchivePreviousLog_ExistingLiveLog_ShouldBeMovedToTimestampedArchive()
        {
            var liveLog = Path.Combine(_tempDir, "file_logger.log");
            const string sentinel = "generation log of the previous run";
            File.WriteAllText(liveLog, sentinel);

            // #1192: the log to archive is passed EXPLICITLY. Swapping the process-wide
            // Logger.LogFile raced with concurrent Log() calls from other xUnit collections —
            // green in isolation, red in the full suite. No global state is touched here.
            Logger.ArchivePreviousLog(liveLog);

            // The DoD property: the previous run's log SURVIVES a later pass, content intact.
            var archives = Directory.GetFiles(_tempDir, "file_logger-*.log");
            archives.Should().ContainSingle("exactly one timestamped archive must exist");
            File.ReadAllText(archives.Single()).Should().StartWith(sentinel,
                "the archived content must be preserved");
        }

        [Fact]
        public void ArchivePreviousLog_NoLiveLog_ShouldBeNoOp()
        {
            var absentLog = Path.Combine(_tempDir, "file_logger.log"); // does not exist

            Action act = () => Logger.ArchivePreviousLog(absentLog);

            act.Should().NotThrow();
            Directory.GetFiles(_tempDir, "file_logger-*.log").Should().BeEmpty(
                "no archive must be created from nothing");
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }
    }
}
