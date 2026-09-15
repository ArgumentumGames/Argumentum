using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// #1179 guard: a document×language couple that reaches PDF assembly with ZERO images must
    /// fail loudly instead of silently skipping — the #1177 defect, where the silent skip kept the
    /// stale previous PDF in place and the CMYK pass then refreshed its mtime, shipping 3 French
    /// PDFs under en/es/ru names.
    /// </summary>
    public class EmptyImagesFailLoudTests
    {
        [Fact]
        public void GeneratePrintAndPlay_WithEmptyImageList_ShouldThrowNotSkip()
        {
            // Arrange — the #1179 DoD provocation: a couple produces zero images (card set
            // nonexistent or genuinely empty) and still reaches assembly.
            var pdfManager = new PdfManager();
            var outputPath = Path.Combine(Path.GetTempPath(), $"arg1179_{Guid.NewGuid()}.pdf");

            // Act
            Action act = () => pdfManager.GeneratePrintAndPlay(outputPath, new CardSetDocumentConfig(),
                new List<CardImages>(), true);

            // Assert — fail loud, naming the file, and leave nothing behind
            act.Should().Throw<InvalidOperationException>()
                .Which.Message.Should().Contain(outputPath)
                .And.Contain("0 images");
            File.Exists(outputPath).Should().BeFalse("a refused generation must not leave a file behind");
        }
    }
}
