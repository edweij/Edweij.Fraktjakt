using Edweij.Fraktjakt.APIClient.ResponseModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.StructTests
{
    [TestFixture]
    public class ShippingStateTests
    {
        [Test]
        public void FromXml_ValidXmlElement_ShouldReturnShippingState()
        {
            // Arrange
            XElement validXmlElement = new("shipping_state",
                new XElement("shipment_id", "123"),
                new XElement("name", "StateName"),
                new XElement("id", "0"),
                new XElement("fraktjakt_id", "0"));

            // Act
            ShippingState shippingState = ShippingState.FromXml(validXmlElement);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shippingState.ShipmentId, Is.EqualTo(123));
                Assert.That(shippingState.Name, Is.EqualTo("StateName"));
                Assert.That(shippingState.StateId.Id, Is.EqualTo(0));
                Assert.That(shippingState.FraktjaktStateId.Id, Is.EqualTo(0));
            });
        }

        [Test]
        public void FromXml_InvalidXmlElement_ShouldThrowException()
        {
            // Arrange
            XElement invalidXmlElement = new("invalidElement");

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => ShippingState.FromXml(invalidXmlElement));
        }

        [Test]
        public void FromXml_MissingRequiredElement_ShouldThrowException()
        {
            // Arrange
            XElement invalidXmlElement = new("shipping_state",
                new XElement("name", "StateName"),
                new XElement("id", "0"));

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => ShippingState.FromXml(invalidXmlElement));
        }

        [Test]
        public void FromXml_InvalidNumericValue_ShouldThrowException()
        {
            // Arrange
            XElement invalidXmlElement = new XElement("shipping_state",
                new XElement("shipment_id", "InvalidValue"),
                new XElement("name", "StateName"),
                new XElement("id", "1"),
                new XElement("fraktjakt_id", "1"));

            // Act & Assert
            Assert.Throws<FormatException>(() => ShippingState.FromXml(invalidXmlElement));
        }

    }
}
