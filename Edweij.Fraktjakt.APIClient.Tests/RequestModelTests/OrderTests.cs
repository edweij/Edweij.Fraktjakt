﻿using Edweij.Fraktjakt.APIClient.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests
{
    [TestFixture]
    public class OrderTests
    {

        private MockOrder validMockOrder;
        private MockOrder invalidMockOrder;

        [SetUp]
        public void SetUp()
        {
            // Create a valid MockOrder instance
            var validSender = new Sender(1, "key");
            var validItems = new List<ShipmentItem> { new ShipmentItem("TestItem", 2, 1.5f, 10.0f) };

            validMockOrder = new MockOrder(validSender, 1, validItems) { Value = 100.0f, AgentId = 100, CallbackUrl = "callbackurl", Reference = "12345", InsureDefault = true, ExportReason = Enums.ExportReason.SAMPLE, NoAgents = false, SenderEmail = "test@test.se" };

            // Create an invalid MockOrder instance

            invalidMockOrder = new MockOrder(validSender, 1, validItems)
            {
                Reference = "Lorem ipsum dolor sit amet, consectetur adipiscing dui...",
                Dispatcher = new Dispatcher { Email = "invalid-email" },
                Recipient = new Recipient { Email = "invalid-email" },
                PickupInfo = new PickupInfo { PickupDate = DateTime.Now.AddDays(-1) }

            };
        }

        [Test]
        public void Constructor_WithInvalidArguments_ShouldThrowException()
        {
            // Arrange
            var validSender = new Sender(1, "key");
            var validItems = new List<ShipmentItem> { new ShipmentItem("Name", 1, 0.5f, 125f) };

            // Act & Assert
            Assert.Multiple(() =>
            {
                // Invalid sender
                Assert.That(() => new MockOrder(null, 1, validItems), Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("sender"));

                // Invalid shippingProductId
                Assert.That(() => new MockOrder(validSender, 0, validItems), Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Provided shippingProductId not valid"));

                // Invalid items
                var invalidItems = new List<ShipmentItem> { new ShipmentItem("TestItem", 1, 1.5f, 10.0f) { Description = "description" } }; // Invalid item
                Assert.That(() => new MockOrder(validSender, 1, invalidItems), Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Items contain invalid item"));
            });

        }

        [Test]
        public void ToXml_WithValidOrder_ShouldGenerateValidXml()
        {
            // Arrange & Act
            var generatedXml = validMockOrder.ToXml();

            // Assert
            Assert.Multiple(() =>
            {
                // XDocument assertions
                var xDocument = XDocument.Parse(generatedXml);
                Assert.That(xDocument.Root!.Name.LocalName, Is.EqualTo("OrderSpecification"));
                Assert.That(xDocument.Descendants("value").Single().Value, Is.EqualTo("100"));
                Assert.That(xDocument.Descendants("shipping_product_id").Single().Value, Is.EqualTo("1"));
                Assert.That(xDocument.Descendants("agent_id").Single().Value, Is.EqualTo("100"));
                Assert.That(xDocument.Descendants("callback_url").Single().Value, Is.EqualTo("callbackurl"));
                Assert.That(xDocument.Descendants("reference").Single().Value, Is.EqualTo("12345"));
                Assert.That(xDocument.Descendants("insure_default").Single().Value, Is.EqualTo("1"));
                Assert.That(xDocument.Descendants("export_reason").Single().Value, Is.EqualTo("SAMPLE"));
                Assert.That(xDocument.Descendants("no_agents").Single().Value, Is.EqualTo("0"));
                Assert.That(xDocument.Descendants("sender_email").Single().Value, Is.EqualTo("test@test.se"));
            });
        }

        [Test]
        public void ToXml_WithInvalidOrder_ShouldThrowException()
        {
            // Act & Assert
            Assert.That(invalidMockOrder.ToXml, Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Order element is not valid"));

        }

        [Test]
        public void GetRuleViolations_WithValidOrder_ShouldReturnEmptyList()
        {
            // Arrange & Act
            var ruleViolations = validMockOrder.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Is.Empty);
        }

        [Test]
        public void GetRuleViolations_WithInValidOrder_ShouldReturnRuleViolations()
        {
            // Arrange & Act
            var ruleViolations = invalidMockOrder.GetRuleViolations();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ruleViolations.Count(), Is.EqualTo(5));
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "Reference" && v.Error == "Max length 50"));
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "Reference" && v.Error == "May only contain space, 0-9 and a-z or A-Z"));
            });
        }


    }

    public class MockOrder : Order
    {
        public MockOrder(Sender sender, int shippingProductId, IEnumerable<ShipmentItem> items) : base(sender, shippingProductId, items)
        { }
    }
}
