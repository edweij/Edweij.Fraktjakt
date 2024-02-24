using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class CreateShipmentTests
    {
        [Test]
        public void Constructor_WithValidParameters_ShouldCreateValidObject()
        {
            // Arrange
            var sender = new Sender(1, "key");
            var toAddress = new ToAddress("12345");
            var validItems = new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) };

            // Act
            var shipment = new CreateShipment(sender, toAddress, items: validItems);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shipment.Sender, Is.EqualTo(sender));
                Assert.That(shipment.ToAddress, Is.EqualTo(toAddress));
                Assert.That(shipment.IsValid, Is.True);
            });
        }

        [Test]
        public void Constructor_WithInvalidToAddress_ShouldThrowArgumentException()
        {
            // Arrange
            var sender = new Sender(1, "key");
            var invalidToAddress = new ToAddress("12345") { StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam" }; // Invalid to address details

            // Act & Assert
            Assert.That(() => new CreateShipment(sender, invalidToAddress), Throws.ArgumentException.With.Message.EqualTo("Provided toAddress not valid"));
        }

        [Test]
        public void IsValid_WithValidShipment_ShouldReturnTrue()
        {
            // Arrange
            var validShipment = new CreateShipment(
                new Sender(1, "key"),
                new ToAddress("12345"),
            items: new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) });

            // Act & Assert
            Assert.That(validShipment.IsValid, Is.True);
        }

        [Test]
        public void IsValid_WithInvalidShipment_ShouldReturnFalse()
        {
            // Arrange
            var invalidShipment = new CreateShipment(
                new Sender(1, "key"),
                new ToAddress("12345") 
                /* no items == invalid */
            );

            // Act & Assert
            Assert.That(invalidShipment.IsValid, Is.False);
        }

        [Test]
        public void ToXml_WithValidShipment_ShouldReturnValidXml()
        {
            // Arrange
            var validShipment = new CreateShipment(
                new Sender(1, "key"),
                new ToAddress("12345"),
            items: new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) }
            );

            // Act
            var xmlResult = validShipment.ToXml();

            // Assert
            var element = XElement.Parse(xmlResult);
            Assert.Multiple(() => {
                Assert.That(xmlResult, Is.Not.Empty);
                Assert.That(element.Name.LocalName, Is.EqualTo("CreateShipment"));
                Assert.That(element.Elements().Count, Is.EqualTo(6));
                Assert.That(element.Element("consignor"), Is.Not.Null);
                Assert.That(element.Element("insure_default"), Is.Not.Null);
                Assert.That(element.Element("price_sort"), Is.Not.Null);
                Assert.That(element.Element("address_to"), Is.Not.Null);
                Assert.That(element.Element("export_reason"), Is.Not.Null);
                Assert.That(element.Element("commodities"), Is.Not.Null);
            });
            
        }

        [Test]
        public void ToXml_WithInvalidShipment_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidShipment = new CreateShipment(
                new Sender(1, "key"),
                new ToAddress("12345")            
                /* no items or parcels == invalid */
            );

            // Act & Assert
            Assert.That(() => invalidShipment.ToXml(), Throws.ArgumentException.With.Message.EqualTo("Shipment element is not valid"));
        }
    }
}
