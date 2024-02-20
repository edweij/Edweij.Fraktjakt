using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class FraktjaktStateIdTests
    {
        [Test]
        public void DefaultConstructor_IdZero_ShouldNotThrowException()
        {
            // Arrange & Act
            FraktjaktStateId fraktjaktStateId = new FraktjaktStateId();

            // Assert
            Assert.That(fraktjaktStateId.Id, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(12)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        public void ParameterizedConstructor_ValidId_ShouldNotThrowException(int validId)
        {
            // Arrange & Act
            FraktjaktStateId fraktjaktStateId = new FraktjaktStateId(validId);

            // Assert
            Assert.That(fraktjaktStateId.Id, Is.EqualTo(validId));
        }

        [TestCase(-1)]
        [TestCase(8)]
        [TestCase(20)]
        public void ParameterizedConstructor_InvalidId_ShouldThrowException(int invalidId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new FraktjaktStateId(invalidId));
        }

        [TestCase(0, "Obetald")]
        [TestCase(1, "Förberedande")]
        [TestCase(3, "Betald")]
        [TestCase(5, "Levererat")]
        [TestCase(7, "Retur")]
        [TestCase(12, "Hanteras av transportören")]
        [TestCase(17, "Rättas")]
        [TestCase(18, "Väntande")]
        [TestCase(19, "Söks")]
        public void ToString_ValidId_ShouldReturnCorrectString(int validId, string expectedString)
        {
            // Arrange
            FraktjaktStateId fraktjaktStateId = new FraktjaktStateId(validId);

            // Act
            string result = fraktjaktStateId.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedString));
        }
    }
}
