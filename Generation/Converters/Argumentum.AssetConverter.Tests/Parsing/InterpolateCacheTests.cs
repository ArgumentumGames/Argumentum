using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Regression tests for the Interpolate expression cache.
    /// Bug fix: cache key must include parameter types to avoid collisions
    /// when different types share the same expression (e.g., "{item.Text}"
    /// used with both Fallacy and Virtue objects).
    /// See commit 94fee271 for the fix.
    /// </summary>
    public class InterpolateCacheTests
    {
        private class TypeA
        {
            public string Text { get; set; }
            public string Family { get; set; }
        }

        private class TypeB
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        [Fact]
        public void Interpolate_SameExpression_DifferentTypes_ShouldNotCollide()
        {
            // Arrange
            var expression = "{item.Text}";
            var itemA = new TypeA { Text = "Hello from A", Family = "FamilyA" };
            var itemB = new TypeB { Text = "Hello from B", Value = 42 };

            // Act - first call caches the expression for TypeA
            var resultA = expression.Interpolate(new Dictionary<string, object> { { "item", itemA } });

            // Act - second call with TypeB should NOT reuse TypeA's cached delegate
            var resultB = expression.Interpolate(new Dictionary<string, object> { { "item", itemB } });

            // Assert
            resultA.Should().Be("Hello from A");
            resultB.Should().Be("Hello from B", "cache key must include type info to avoid collision");
        }

        [Fact]
        public void Interpolate_SameExpression_SameType_ShouldReuseCache()
        {
            // Arrange
            var expression = "{item.Text}";
            var item1 = new TypeA { Text = "First" };
            var item2 = new TypeA { Text = "Second" };

            // Act
            var result1 = expression.Interpolate(new Dictionary<string, object> { { "item", item1 } });
            var result2 = expression.Interpolate(new Dictionary<string, object> { { "item", item2 } });

            // Assert - both should work, second uses cached delegate
            result1.Should().Be("First");
            result2.Should().Be("Second");
        }

        [Fact]
        public void Interpolate_TypeSpecificProperty_ShouldWork()
        {
            // Arrange - TypeA has Family, TypeB has Value
            var exprA = "{item.Family}";
            var exprB = "{item.Value}";
            var itemA = new TypeA { Text = "A", Family = "TestFamily" };
            var itemB = new TypeB { Text = "B", Value = 99 };

            // Act
            var resultA = exprA.Interpolate(new Dictionary<string, object> { { "item", itemA } });
            var resultB = exprB.Interpolate(new Dictionary<string, object> { { "item", itemB } });

            // Assert
            resultA.Should().Be("TestFamily");
            resultB.Should().Be("99");
        }

        [Fact]
        public void Interpolate_MultipleParameters_ShouldWork()
        {
            // Arrange
            var expression = "{item.Text} - {prefix.Text}";
            var item = new TypeA { Text = "Main" };
            var prefix = new TypeA { Text = "Pre" };

            // Act
            var result = expression.Interpolate(new Dictionary<string, object>
            {
                { "item", item },
                { "prefix", prefix }
            });

            // Assert
            result.Should().Be("Main - Pre");
        }
    }
}
