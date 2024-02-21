using Edweij.Fraktjakt.APIClient.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class ShippingDocumentTypeIdTests
    {
        [TestCase(1, "Pro Forma-faktura")]
        [TestCase(2, "Handelsfaktura")]
        [TestCase(3, "Fraktetikett")]
        [TestCase(4, "Fraktsedel")]
        [TestCase(5, "Sändningslista")]
        [TestCase(10, "Följesedel")]
        [TestCase(11, "CN22")]
        [TestCase(12, "CN23")]
        [TestCase(13, "Säkerhetsdeklaration")]
        public void ParameterizedConstructor_ValidId_ShouldNotThrowException(int validId, string expectedDocumentType)
        {
            // Arrange & Act
            ShippingDocumentTypeId documentTypeId = new ShippingDocumentTypeId(validId);

            // Assert
            Assert.That(documentTypeId.Id, Is.EqualTo(validId));
        }

        [TestCase(0)]
        [TestCase(6)]
        [TestCase(14)]
        public void ParameterizedConstructor_InvalidId_ShouldThrowException(int invalidId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ShippingDocumentTypeId(invalidId));
        }

        [TestCase(1, "Pro Forma-faktura")]
        [TestCase(2, "Handelsfaktura")]
        [TestCase(3, "Fraktetikett")]
        [TestCase(4, "Fraktsedel")]
        [TestCase(5, "Sändningslista")]
        [TestCase(10, "Följesedel")]
        [TestCase(11, "CN22")]
        [TestCase(12, "CN23")]
        [TestCase(13, "Säkerhetsdeklaration")]
        public void ToString_ShouldReturnCorrectDocumentType(int inputId, string expectedDocumentType)
        {
            // Arrange
            ShippingDocumentTypeId documentTypeId = new ShippingDocumentTypeId(inputId);

            // Act
            string result = documentTypeId.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedDocumentType));
        }

        [Test]
        public void ImplicitConversionFromInt_ValidId_ShouldNotThrowException()
        {
            // Arrange & Act
            ShippingDocumentTypeId documentTypeId = 1;

            // Assert
            Assert.That(documentTypeId.Id, Is.EqualTo(1));
        }

        [Test]
        public void ImplicitConversionFromInt_InvalidId_ShouldThrowException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = (ShippingDocumentTypeId)0; });
        }
    }
}
