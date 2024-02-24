using Edweij.Fraktjakt.APIClient.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edweij.Fraktjakt.APIClient.Tests.StructTests
{
    [TestFixture]
    public class ShippingStateIdTests
    {
        [Test]
        public void DefaultConstructor_IdZero_ShouldNotThrowException()
        {
            // Arrange & Act
            ShippingStateId shippingStateId = new ShippingStateId();

            // Assert
            Assert.That(shippingStateId.Id, Is.EqualTo(0));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ParameterizedConstructor_ValidId_ShouldNotThrowException(int validId)
        {
            // Arrange & Act
            ShippingStateId shippingStateId = new ShippingStateId(validId);

            // Assert
            Assert.That(shippingStateId.Id, Is.EqualTo(validId));
        }

        [TestCase(-1)]
        [TestCase(5)]
        [TestCase(10)]
        public void ParameterizedConstructor_InvalidId_ShouldThrowException(int invalidId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ShippingStateId(invalidId));
        }

        [TestCase(0, "Hanteras av transportören")]
        [TestCase(1, "Avsänt")]
        [TestCase(2, "Levererat")]
        [TestCase(3, "Kvitterats")]
        [TestCase(4, "Retur")]
        public void ToString_ValidId_ShouldReturnCorrectString(int validId, string expectedString)
        {
            // Arrange
            ShippingStateId shippingStateId = new ShippingStateId(validId);

            // Act
            string result = shippingStateId.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedString));
        }
    }
}
