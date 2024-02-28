using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests
{
    public class QueryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShipmentQueryConstructorShouldThrowOnNullOrInvalidparameters()
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => new Query(null, null));
                Assert.Throws<ArgumentNullException>(() => new Query(new Sender(1, "key"), null)); // to address is null
                Assert.Throws<ArgumentNullException>(() => new Query(null, new ToAddress("62141"))); // sender is null
                Assert.Throws<ArgumentException>(() => new Query(new Sender(-1, "   "), new ToAddress("62141"))); // invalid sender
                Assert.Throws<ArgumentException>(() => new Query(new Sender(1, "key"), new ToAddress("12345") { StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam" })); // invalid to adress
                Assert.Throws<ArgumentException>(() => new Query(new Sender(1, "key"), new ToAddress("12345"), fromAddress: new FromAddress("12345") { StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam" })); // invalid from adress
                Assert.Throws<ArgumentException>(() => new Query(new Sender(1, "key"), new ToAddress("12345"), items: new List<ShipmentItem>() { new ShipmentItem("TestItem", 0, 1.5f, 10.0f) })); // invalid items
                Assert.Throws<ArgumentException>(() => new Query(new Sender(1, "key"), new ToAddress("12345"), parcels: new List<Parcel>() { new Parcel(1.5f) { Length = 0 } })); // invalid parcels
            });

        }

        [Test]
        public void ShipmentQuerySetPropertyWithInvalidValueShouldThrow()
        {
            var query = new Query(new Sender(1, "key"), new ToAddress("62141"));
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentNullException>(() => query.FromAddress = null); // Set fromadress to null
                Assert.Throws<ArgumentException>(() => query.FromAddress = new FromAddress("12345") { StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam" }); // Set fromadress to invalid address
                Assert.Throws<ArgumentNullException>(() => query.Items = null); // Set items to null
                Assert.Throws<ArgumentException>(() => query.Items = new List<ShipmentItem>() { new ShipmentItem("TestItem", 0, 1.5f, 10.0f) }); // Set items with invalid item
                Assert.Throws<ArgumentNullException>(() => query.Parcels = null); // Set parcels to null
                Assert.Throws<ArgumentException>(() => query.Parcels = new List<Parcel>() { new Parcel(1.5f) { Length = 0 } }); // Set parcels with invalid item
            });

        }

        [Test]
        public void ShipmentQueryValidation()
        {
            var query = new Query(new Sender(1, "key"), new ToAddress("62141"));
            var errors = query.GetRuleViolations();
            Assert.Multiple(() =>
            {
                Assert.That(errors.Count(), Is.EqualTo(2));
                Assert.That(errors.Any(v => v.PropertyName == "Items"), Is.True);
                Assert.That(errors.Any(v => v.PropertyName == "Parcels"), Is.True);
                query.Items = new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) };
                errors = query.GetRuleViolations();
                Assert.That(errors.Count(), Is.EqualTo(0));
                query.CallbackUrl = "   ";
                query.ShippingProductId = 0;
                query.Value = -100f;
                Assert.That(errors.Count(), Is.EqualTo(3));
                Assert.That(errors.Any(v => v.PropertyName == "CallbackUrl"), Is.True);
                Assert.That(errors.Any(v => v.PropertyName == "ShippingProductId"), Is.True);
                Assert.That(errors.Any(v => v.PropertyName == "Value"), Is.True);
                query.ToAddress.StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam";
                Assert.That(errors.Count(), Is.EqualTo(5));
            });
        }

        [Test]
        public void ShipmentItemGeneratesCorrectXml()
        {
            var query = new Query(new Sender(1, "key"), new ToAddress("62141"), items: new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) });
            var element = XElement.Parse(query.ToXml());
            Assert.Multiple(() =>
            {
                Assert.That(element.Elements().Count(), Is.EqualTo(17));
                Assert.That(element.Name.LocalName, Is.EqualTo("shipment"));
                Assert.That(element.Element("consignor"), Is.Not.Null);
                Assert.That(element.Element("insure_default"), Is.Not.Null);
                Assert.That(element.Element("insure_default").Value, Is.EqualTo("0"));
                Assert.That(element.Element("price_sort"), Is.Not.Null);
                Assert.That(element.Element("price_sort").Value, Is.EqualTo("1"));
                Assert.That(element.Element("express"), Is.Not.Null);
                Assert.That(element.Element("express").Value, Is.EqualTo("0"));
                Assert.That(element.Element("pickup"), Is.Not.Null);
                Assert.That(element.Element("pickup").Value, Is.EqualTo("0"));
                Assert.That(element.Element("dropoff"), Is.Not.Null);
                Assert.That(element.Element("dropoff").Value, Is.EqualTo("0"));
                Assert.That(element.Element("green"), Is.Not.Null);
                Assert.That(element.Element("green").Value, Is.EqualTo("0"));
                Assert.That(element.Element("quality"), Is.Not.Null);
                Assert.That(element.Element("quality").Value, Is.EqualTo("0"));
                Assert.That(element.Element("time_guarantee"), Is.Not.Null);
                Assert.That(element.Element("time_guarantee").Value, Is.EqualTo("0"));
                Assert.That(element.Element("cold"), Is.Not.Null);
                Assert.That(element.Element("cold").Value, Is.EqualTo("0"));
                Assert.That(element.Element("frozen"), Is.Not.Null);
                Assert.That(element.Element("frozen").Value, Is.EqualTo("0"));
                Assert.That(element.Element("no_agents"), Is.Not.Null);
                Assert.That(element.Element("no_agents").Value, Is.EqualTo("0"));
                Assert.That(element.Element("no_prices"), Is.Not.Null);
                Assert.That(element.Element("no_prices").Value, Is.EqualTo("0"));
                Assert.That(element.Element("agents_in"), Is.Not.Null);
                Assert.That(element.Element("agents_in").Value, Is.EqualTo("0"));
                Assert.That(element.Element("shipper_info"), Is.Not.Null);
                Assert.That(element.Element("shipper_info").Value, Is.EqualTo("0"));
                Assert.That(element.Element("commodities"), Is.Not.Null);
                Assert.That(element.Element("address_to"), Is.Not.Null);
            });
        }

        [Test]
        public void ShipmentQueryXmlReplacesEntities()
        {
            var query = new Query(new Sender(1, "key"), new ToAddress("62141"), items: new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) })
            {
                CallbackUrl = "<&'\">"
            };
            var result = query.ToXml();
            Assert.That(result, Contains.Substring("<callback_url>&lt;&amp;'\"&gt;</callback_url>"));
        }

        [Test]
        public void Equals_SameInstance_ReturnsTrue()
        {
            // Arrange
            var sender = new Sender(1, "ApiKey");
            var toAddress = new ToAddress("12345");
            var query = new Query(sender, toAddress);

            // Act
            var result = query.Equals(query);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey");
            var toAddress1 = new ToAddress("12345");
            var query1 = new Query(sender1, toAddress1);

            var sender2 = new Sender(1, "ApiKey");
            var toAddress2 = new ToAddress("12345");
            var query2 = new Query(sender2, toAddress2);

            // Act
            var result = query1.Equals(query2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey1");
            var toAddress1 = new ToAddress("12345");
            var query1 = new Query(sender1, toAddress1);

            var sender2 = new Sender(2, "ApiKey2");
            var toAddress2 = new ToAddress("67890");
            var query2 = new Query(sender2, toAddress2);

            // Act
            var result = query1.Equals(query2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetHashCode_SameValues_ReturnsSameHashCode()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey");
            var toAddress1 = new ToAddress("12345");
            var query1 = new Query(sender1, toAddress1);

            var sender2 = new Sender(1, "ApiKey");
            var toAddress2 = new ToAddress("12345");
            var query2 = new Query(sender2, toAddress2);

            // Act
            var hashCode1 = query1.GetHashCode();
            var hashCode2 = query2.GetHashCode();

            // Assert
            Assert.That(hashCode1, Is.EqualTo(hashCode2));
        }

        [Test]
        public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey1");
            var toAddress1 = new ToAddress("12345");
            var query1 = new Query(sender1, toAddress1);

            var sender2 = new Sender(2, "ApiKey2");
            var toAddress2 = new ToAddress("67890");
            var query2 = new Query(sender2, toAddress2);

            // Act
            var hashCode1 = query1.GetHashCode();
            var hashCode2 = query2.GetHashCode();

            // Assert
            Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
        }

    }
}