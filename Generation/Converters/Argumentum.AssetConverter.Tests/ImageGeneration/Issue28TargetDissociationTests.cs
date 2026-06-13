using Xunit;
using FluentAssertions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageMagick;
using Xunit.Abstractions;

namespace Argumentum.AssetConverter.Tests.ImageGeneration
{
    /// <summary>
    /// Issue #28: (a) dissociate front/back target folders, (b) halt the pipeline
    /// before PDF assembly. Both opt-in via config, default behaviour unchanged.
    /// </summary>
    public class Issue28TargetDissociationTests : IDisposable
    {
        private readonly string _testOutputDir;
        private readonly ITestOutputHelper _output;

        public Issue28TargetDissociationTests(ITestOutputHelper output)
        {
            _output = output;
            _testOutputDir = Path.Combine(AppContext.BaseDirectory, "TestRuns", Guid.NewGuid().ToString());
            if (Directory.Exists(_testOutputDir))
            {
                Directory.Delete(_testOutputDir, true);
            }
            Directory.CreateDirectory(_testOutputDir);
        }

        private AssetConverterConfig BuildConfig(bool separateFrontBack, bool stopBeforePdf, List<CardSetDocumentConfig> docConfigs)
        {
            var imageOutputDir = Path.Combine(_testOutputDir, "GeneratedImages");
            Directory.CreateDirectory(imageOutputDir);

            return new AssetConverterConfig
            {
                BaseTargetDirectoryName = imageOutputDir,
                SeparateFrontBackFolders = separateFrontBack,
                StopBeforePdfGeneration = stopBeforePdf,
                LocalizationConfig = new LocalizationConfig { Enabled = false, DefaultLanguage = "en" },
                WebBasedGeneratorConfig = new WebBasedGeneratorConfig
                {
                    EnableParallelism = false,
                    CardSetDocuments = docConfigs
                }
            };
        }

        // ---- (a) Front/back folder dissociation ----

        [Fact]
        public void GetImageFolder_WhenSeparateDisabled_KeepsFlatLayoutForFrontAndBack()
        {
            var docConfig = new CardSetDocumentConfig { DocumentName = "TestDoc", ImageFormat = MagickFormat.Png };
            var config = BuildConfig(separateFrontBack: false, stopBeforePdf: false, new List<CardSetDocumentConfig> { docConfig });

            var frontFolder = ImageHelper.GetImageFolder(config, docConfig, "en", "TestSet", isBack: false);
            var backFolder = ImageHelper.GetImageFolder(config, docConfig, "en", "TestSet", isBack: true);

            // Backward-compatible: front and back share the same card-set folder.
            frontFolder.Should().Be(backFolder);
            frontFolder.Should().NotContain(@"front" + Path.DirectorySeparatorChar);
            backFolder.Should().NotContain(@"back" + Path.DirectorySeparatorChar);
        }

        [Fact]
        public void GetImageFolder_WhenSeparateEnabled_RoutesFrontAndBackToDistinctSubFolders()
        {
            var docConfig = new CardSetDocumentConfig { DocumentName = "TestDoc", ImageFormat = MagickFormat.Png };
            var config = BuildConfig(separateFrontBack: true, stopBeforePdf: false, new List<CardSetDocumentConfig> { docConfig });

            var frontFolder = ImageHelper.GetImageFolder(config, docConfig, "en", "TestSet", isBack: false);
            var backFolder = ImageHelper.GetImageFolder(config, docConfig, "en", "TestSet", isBack: true);

            frontFolder.Should().NotBe(backFolder);
            frontFolder.TrimEnd(Path.DirectorySeparatorChar).Should().EndWith("front");
            backFolder.TrimEnd(Path.DirectorySeparatorChar).Should().EndWith("back");
            Directory.Exists(frontFolder).Should().BeTrue();
            Directory.Exists(backFolder).Should().BeTrue();
        }

        [Fact]
        public void GetImageFileName_WhenSeparateEnabled_PlacesBackImageUnderBackSubFolder()
        {
            var docConfig = new CardSetDocumentConfig { DocumentName = "TestDoc", ImageFormat = MagickFormat.Png };
            var config = BuildConfig(separateFrontBack: true, stopBeforePdf: false, new List<CardSetDocumentConfig> { docConfig });

            var facePath = ImageHelper.GetImageFileName(config, docConfig, "en", "TestSet", "card1_face", isBack: false);
            var backPath = ImageHelper.GetImageFileName(config, docConfig, "en", "TestSet", "default", isBack: true);

            facePath.Should().Contain(@"front" + Path.DirectorySeparatorChar);
            facePath.Should().EndWith("card1_face.png");
            backPath.Should().Contain(@"back" + Path.DirectorySeparatorChar);
            backPath.Should().EndWith("default.png");
        }

        // ---- (b) Halt before PDF generation ----

        [Fact]
        public void GenerateCardSetDocuments_WhenStopBeforePdf_ProducesNoPdf()
        {
            var docConfig = new CardSetDocumentConfig
            {
                DocumentName = "TestDoc",
                Enabled = true,
                NoBack = true,
                DocumentFormat = CardDocumentFormat.FacesOnly,
                ImageFormat = MagickFormat.Png,
                CardSets = new List<DocumentCardSet> { new DocumentCardSet { CardSetName = "TestSet" } }
            };
            // QuestPdfGeneration IS enabled, but StopBeforePdfGeneration must short-circuit assembly.
            var config = BuildConfig(separateFrontBack: false, stopBeforePdf: true, new List<CardSetDocumentConfig> { docConfig });
            config.Mode = ConverterMode.WebBasedImageGeneration | ConverterMode.QuestPdfGeneration;

            var generator = new global::Argumentum.AssetConverter.WebBasedGenerator
            {
                AssetConverterConfig = config,
                Config = config.WebBasedGeneratorConfig
            };

            // Build a fake on-disk face image and a docImages dictionary as if image generation had run.
            var facePath = ImageHelper.GetImageFileName(config, docConfig, "en", "TestSet", "card1");
            Directory.CreateDirectory(Path.GetDirectoryName(facePath)!);
            using (var img = new MagickImage(MagickColors.Transparent, 1, 1))
            {
                img.Format = MagickFormat.Png;
                img.Write(facePath);
            }

            var docImages = new ConcurrentDictionary<(CardSetDocumentConfig document, string language), List<CardImages>>();
            docImages.TryAdd((docConfig, "en"), new List<CardImages> { new CardImages { Front = facePath } });

            var documentDir = config.GetDocumentDirectory("en");

            // Act
            generator.GenerateCardSetDocuments(docImages);

            // Assert: no PDF produced because the pipeline halted before assembly.
            var pdfs = Directory.Exists(documentDir)
                ? Directory.GetFiles(documentDir, "*.pdf", SearchOption.AllDirectories)
                : Array.Empty<string>();
            pdfs.Should().BeEmpty("StopBeforePdfGeneration must prevent any PDF assembly even when QuestPdfGeneration is set");
        }

        public void Dispose()
        {
            if (Directory.Exists(_testOutputDir))
            {
                try { Directory.Delete(_testOutputDir, true); } catch { /* best effort */ }
            }
        }
    }
}
