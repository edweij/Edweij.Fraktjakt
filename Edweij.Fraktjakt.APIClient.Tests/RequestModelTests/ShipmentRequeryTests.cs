using Edweij.Fraktjakt.APIClient.RequestModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests
{
    [TestFixture]
    public class ShipmentReQueryTests
    {
        [Test]
        public void Constructor_ValidSenderAndShipmentId_ShouldCreateInstance()
        {
            // Arrange
            var sender = new Sender(1, "key");
            int shipmentId = 123;

            // Act
            ReQuery shipmentReQuery = new ReQuery(sender, shipmentId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shipmentReQuery.Sender, Is.EqualTo(sender));
                Assert.That(shipmentReQuery.ShipmentId, Is.EqualTo(shipmentId));
                Assert.That(shipmentReQuery.Value, Is.Null);
                Assert.That(shipmentReQuery.ShipperInfo, Is.False);
            });
        }

        [Test]
        public void Constructor_NullSender_ThrowsArgumentNullException()
        {
            // Arrange
            Sender sender = null;
            int shipmentId = 123;

            Assert.Throws<ArgumentNullException>(() => new ReQuery(sender, shipmentId));
        }

        [Test]
        public void Constructor_InvalidShipmentId_ThrowsArgumentException()
        {
            // Arrange
            var sender = new Sender(1, "key");
            int invalidShipmentId = 0;

            // Act and Assert
            Assert.Throws<ArgumentException>(() => new ReQuery(sender, invalidShipmentId));
        }

        [Test]
        public void ToXml_ValidInput_ShouldReturnXmlString()
        {
            // Arrange
            var sender = new Sender(1, "key");
            int shipmentId = 123;
            float value = 45.67f;
            bool shipperInfo = true;

            var shipmentReQuery = new ReQuery(sender, shipmentId)
            {
                Value = value,
                ShipperInfo = shipperInfo
            };

            // Act
            string xml = shipmentReQuery.ToXml();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(xml, Does.Contain($"<shipment_id>{shipmentId}</shipment_id>"));
                Assert.That(xml, Does.Contain($"<value>{value.ToString("0.00", CultureInfo.InvariantCulture)}</value>"));
                Assert.That(xml, Does.Contain($"<shipper_info>{(shipperInfo ? "1" : "0")}</shipper_info>"));
                Assert.That(xml, Does.Contain(sender.ToXml()));
            });
        }
    }
}
