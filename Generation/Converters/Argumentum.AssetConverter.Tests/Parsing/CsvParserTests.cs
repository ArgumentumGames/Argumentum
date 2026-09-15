using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    public class CsvParserTests
    {
        // Dummy record for testing purposes
        public class FallacyRecord
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        // Dummy parser for testing purposes
        public class CsvParser
        {
            public IEnumerable<T> Parse<T>(string filePath)
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
        }

        [Fact]
        public void Parse_ValidCsv_ShouldReturnCorrectData()
        {
            // Arrange
            var parser = new CsvParser();
            var filePath = "Assets/Parsing/valid.csv";

            // Act
            var result = parser.Parse<FallacyRecord>(filePath);

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Argument from Ignorance");
        }

        [Fact]
        public void Parse_MalformedCsv_ShouldThrowException()
        {
            // Arrange
            var parser = new CsvParser();
            var filePath = "Assets/Parsing/malformed.csv";

            // Act
            var act = () => parser.Parse<FallacyRecord>(filePath);

            // Assert
            act.Should().Throw<CsvHelper.MissingFieldException>();
        }

        [Fact]
        public void Parse_EmptyCsv_ShouldReturnEmptyCollection()
        {
            // Arrange
            var parser = new CsvParser();
            var filePath = "Assets/Parsing/empty.csv";

            // Act
            var result = parser.Parse<FallacyRecord>(filePath);

            // Assert
            result.Should().BeEmpty();
        }
    }
}