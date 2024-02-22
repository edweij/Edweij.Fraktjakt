using Edweij.Fraktjakt.APIClient.RequestModels;
using NUnit.Framework.Internal;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{

    [TestFixture]
    public class DirectOrderTests
    {
        private DirectOrder validDirectOrder;
        private DirectOrder invalidDirectOrder;

        [SetUp]
        public void SetUp()
        {
            // Create a valid DirectOrder instance
            var validSender = new Sender(1, "key");
            var validToAddress = new ToAddress { PostalCode = "12345" };
            var validItems = new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) };
            var validFromAddress = new FromAddress { PostalCode = "12345" };
            var validParcels = new List<Parcel> { new Parcel { Weight = 0.5f } };

            validDirectOrder = new DirectOrder(validSender, validToAddress, 1, validItems, validFromAddress, validParcels)
            {
                Value = 100.0f
            };

            // Create an invalid DirectOrder instance
            var invalidToAddress = new ToAddress { PostalCode = "12345" };
            invalidDirectOrder = new DirectOrder(validSender, invalidToAddress, 1, validItems, validFromAddress, validParcels);
            invalidDirectOrder.ToAddress.PostalCode = "";
        }
        [Test]
        public void Constructor_WithInvalidArguments_ShouldThrowException()
        {
            // Arrange
            var validSender = new Sender(1, "key");
            var validToAddress = new ToAddress { PostalCode = "12345"};
            var validItems = new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) };
            var validFromAddress = new FromAddress { PostalCode = "12345"};
            var validParcels = new List<Parcel> { new Parcel { Weight = 0.5f } };

            // Act & Assert
            // Invalid sender
            Assert.That(() => new DirectOrder(null, validToAddress, 1, validItems, validFromAddress, validParcels), Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("sender"));
            

            // Invalid toAddress
            Assert.That(() => new DirectOrder(validSender, null, 1, validItems, validFromAddress, validParcels), Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("toAddress"));
            Assert.That(() => new DirectOrder(validSender, new ToAddress(), 1, validItems, validFromAddress, validParcels), Throws.TypeOf<ArgumentException>().With.Message.EqualTo("toAddress"));

            // Invalid items
            var invalidItems = new List<ShipmentItem> { new ShipmentItem("TestItem", 1, 1.5f, 10.0f) { Description = "Description" } }; // Invalid item
            Assert.That(() => new DirectOrder(validSender, validToAddress, 1, invalidItems, validFromAddress, validParcels), Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Items contain invalid item"));

            // Invalid fromAddress
            var invalidFromAddress = new FromAddress(); // Invalid fromAddress
            Assert.That(() => new DirectOrder(validSender, validToAddress, 1, validItems, invalidFromAddress, validParcels), Throws.TypeOf<ArgumentException>().With.Message.EqualTo("fromAddress not valid"));

            // Invalid parcels
            var invalidParcels = new List<Parcel> { new Parcel() }; // Invalid parcel
            Assert.That(() => new DirectOrder(validSender, validToAddress, 1, validItems, validFromAddress, invalidParcels), Throws.TypeOf<ArgumentException>().With.Message.EqualTo("parcels not valid"));
        }        

        [Test]
        public void ToXml_WithValidOrder_ShouldGenerateValidXml()
        {
            // Arrange & Act
            var generatedXml = validDirectOrder.ToXml();

            // Assert
            Assert.Multiple(() =>
            {
                var el = XElement.Parse(generatedXml);
                Assert.That(el.Name.LocalName, Is.EqualTo("OrderSpecification"));
                Assert.That(el.Elements().Count(), Is.EqualTo(10));
                Assert.That(el.Descendants("value").Single().Value, Is.EqualTo("100"));
                Assert.That(el.Descendants("shipping_product_id").Single().Value, Is.EqualTo("1"));
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "value"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "consignor"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "insure_default"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "shipping_product_id"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "export_reason"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "no_agents"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "commodities"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "address_to"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "address_from"), Is.True);
                Assert.That(el.Elements().Any(e => e.Name.LocalName == "parcels"), Is.True);
            });
        }

        [Test]
        public void ToXml_WithInvalidOrder_ShouldThrowException()
        {
            // Act & Assert
            Assert.That(invalidDirectOrder.ToXml, Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Order element is not valid"));

        }

        [Test]
        public void GetRuleViolations_WithValidOrder_ShouldReturnEmptyList()
        {
            // Arrange & Act
            var ruleViolations = validDirectOrder.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Is.Empty);
        }

        [Test]
        public void GetRuleViolations_WithInValidOrder_ShouldReturnRuleViolations()
        {
            // Arrange & Act
            var ruleViolations = invalidDirectOrder.GetRuleViolations();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ruleViolations.Count(), Is.EqualTo(1));
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "PostalCode" && v.Error == "PostalCode is required"));                
            });
        }

    }

}
