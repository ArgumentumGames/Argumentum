using Xunit;
using FluentAssertions;
using Scriban;
using System.Collections.Generic;

namespace Argumentum.AssetConverter.Tests.HtmlGeneration
{
    public class FallacyRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ScribanGeneratorTests
    {
        [Fact]
        public void Render_SimpleProperty_ShouldBeCorrect()
        {
            // Arrange
            var template = Template.Parse("<h1>{{ fallacy.name }}</h1>");
            var fallacy = new FallacyRecord { Id = 1, Name = "Ad Hominem", Description = "Attacking the person instead of the argument." };

            // Act
            var result = template.Render(new { fallacy });

            // Assert
            result.Should().Be("<h1>Ad Hominem</h1>");
        }

        [Fact]
        public void Render_ConditionalLogic_ShouldRenderCorrectly()
        {
            // Arrange
            var template = Template.Parse("{{ if fallacy.is_active }}<p>Active</p>{{ else }}<p>Inactive</p>{{ end }}");
            var activeFallacy = new FallacyRecord { Name = "Active Fallacy", IsActive = true };
            var inactiveFallacy = new FallacyRecord { Name = "Inactive Fallacy", IsActive = false };

            // Act
            var resultActive = template.Render(new { fallacy = activeFallacy });
            var resultInactive = template.Render(new { fallacy = inactiveFallacy });

            // Assert
            resultActive.Should().Be("<p>Active</p>");
            resultInactive.Should().Be("<p>Inactive</p>");
        }

        [Fact]
        public void Render_LoopLogic_ShouldRenderAllItems()
        {
            // Arrange
            var template = Template.Parse("<ul>{{ for item in items }}<li>{{ item.name }}</li>{{ end }}</ul>");
            var fallacies = new List<FallacyRecord>
            {
                new FallacyRecord { Name = "Straw Man" },
                new FallacyRecord { Name = "Red Herring" }
            };

            // Act
            var result = template.Render(new { items = fallacies });

            // Assert
            result.Should().Be("<ul><li>Straw Man</li><li>Red Herring</li></ul>");
        }
    }
}