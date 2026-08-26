using System;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.ImageGeneration
{
    /// <summary>
    /// #1179 guard: image paths must be validated BEFORE the Magick write. The native ImageMagick
    /// path buffer is MAX_PATH (260) — the #1177/#1121 failures surfaced as WriteBlob Failed deep
    /// inside the encoder after the work was done. The guard must fire first, naming the path and
    /// its length.
    /// </summary>
    public class ImagePathLengthGuardTests
    {
        [Fact]
        public void EnsurePathWithinLimit_PathLongerThanLimit_ShouldThrowNamingPathAndLength()
        {
            var longPath = new string('a', ImageHelper.MaxSafeImagePathLength + 10);

            Action act = () => ImageHelper.EnsurePathWithinLimit(longPath);

            act.Should().Throw<InvalidOperationException>()
                .Which.Message.Should().Contain($"{longPath.Length} chars")
                .And.Contain(longPath);
        }

        [Fact]
        public void EnsurePathWithinLimit_PathAtLimit_ShouldNotThrow()
        {
            var pathAtLimit = new string('a', ImageHelper.MaxSafeImagePathLength);

            Action act = () => ImageHelper.EnsurePathWithinLimit(pathAtLimit);

            act.Should().NotThrow("a path at exactly the limit is safe — the guard fires above it, not at it");
        }
    }
}
