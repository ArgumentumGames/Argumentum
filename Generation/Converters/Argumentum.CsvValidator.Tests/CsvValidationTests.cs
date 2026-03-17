using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Argumentum.CsvValidator;
using CsvHelper;
using Xunit;

namespace Argumentum.CsvValidator.Tests
{
    public class CsvValidationTests
    {
        [Fact]
        public void Load_WithValidCsv_ShouldLoadAllRecordsCorrectly()
        {
            // Arrange
            var csvContent = File.ReadAllText("TestData/valid_fallacies.csv");
            
            // Act
            var fallacies = Fallacy.LoadFromContent(csvContent, null);

            // Assert
            Assert.Equal(2, fallacies.Count);
            
            var firstFallacy = fallacies.First();
            Assert.Equal("F001", firstFallacy.Pk);
            Assert.Equal("Appeal to pity", firstFallacy.SimpleNameEn);

            var secondFallacy = fallacies.Skip(1).First();
            Assert.Equal("F002", secondFallacy.Pk);
            Assert.Equal("Argument from adverse consequences", secondFallacy.SimpleNameEn);
        }

        [Fact]
        public void Load_WithInvalidCsv_ShouldThrowException()
        {
            // Arrange
            var invalidCsvContent = File.ReadAllText("TestData/invalid_fallacies.csv");

            // Act & Assert
            var ex = Assert.Throws<HeaderValidationException>(() => Fallacy.LoadFromContent(invalidCsvContent, null));
            Assert.Contains("Header with name 'pk'", ex.Message);
        }
    }
}