using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class RecipientTests
    {
        [Test]
        public void RuleViolations_ShouldBeValid()
        {
            // Arrange
            var recipient = new MockRecipient
            {
                CompanyName = "ABC Company",
                PersonName = "John Doe",
                Phone = "1234567890",
                Email = "john.doe@example.com",
                Eori = "E123456789",
                Mobile = "ValidMobile",
                Message = "ValidMessage",
                Tin = "123456789012345678"
            };

            // Act
            var ruleViolations = recipient.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Is.Empty);
        }

        [Test]
        public void RuleViolations_ShouldBeInValid()
        {
            // Arrange
            var recipient = new MockRecipient
            {
                CompanyName = "A Very Long Company Name That Exceeds Maximum Length",
                PersonName = "A Very Long Person Name That Exceeds Maximum Length",
                Phone = "1234567890123456789012345678901234567890", // Exceeds maximum length
                Email = "invalid-email-format", // Invalid email format
                Eori = "E12345678901234567890123456789013333", // Exceeds maximum length
                Mobile = "A Very Long Mobile Number That Exceeds Maximum Length",
                Message = "A Very Long Message That Exceeds Maximum Length" // Exceeds maximum length
            };

            // Act
            var ruleViolations = recipient.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Has.Exactly(6).Items
                                           .And.Some.Property("PropertyName").EqualTo("CompanyName")
                                           .And.Some.Property("PropertyName").EqualTo("PersonName")
                                           .And.Some.Property("PropertyName").EqualTo("Phone")
                                           .And.Some.Property("PropertyName").EqualTo("Email")
                                           .And.Some.Property("PropertyName").EqualTo("Eori")
                                           .And.Some.Property("PropertyName").EqualTo("Mobile"));
        }

        [Test]
        public void ToXml_WithValidData_ShouldGenerateValidXml()
        {
            // Arrange
            var recipient = new MockRecipient
            {
                CompanyName = "ABC Company",
                Mobile = "ValidMobile",
                Message = "ValidMessage"
            };

            // Act
            string generatedXml = recipient.ToXml();

            // Assert
            Assert.That(() => recipient.ToXml(), Throws.Nothing);
            Assert.That(generatedXml, Does.Contain("<recipient>")
                                       .And.Contain("<company_to>ABC Company</company_to>")
                                       .And.Contain("<mobile_to>ValidMobile</mobile_to>")
                                       .And.Contain("<message_to>ValidMessage</message_to>")
                                       .And.Not.Contain(Environment.NewLine)); // Ensures it's a single line

            // Convert the generatedXml string to XElement for further assertions
            var xmlElement = XElement.Parse(generatedXml);

            Assert.That(xmlElement.Name.LocalName, Is.EqualTo("recipient"));
            Assert.That(xmlElement.Element("company_to").Value, Is.EqualTo("ABC Company"));
            Assert.That(xmlElement.Element("mobile_to").Value, Is.EqualTo("ValidMobile"));
            Assert.That(xmlElement.Element("message_to").Value, Is.EqualTo("ValidMessage"));
        }

        [Test]
        public void ToXml_WithInvalidData_ShouldThrowArgumentException()
        {
            // Arrange
            var recipient = new MockRecipient
            {
                CompanyName = "ABC Company",
                Mobile = "InvalidMobile to long ipsum lorem dolor et simat", // Invalid mobile format
                Message = "ValidMessage"
            };

            // Act & Assert
            Assert.That(() => recipient.ToXml(), Throws.ArgumentException.With.Message.EqualTo("Recipient element is not valid"));
        }
    }

    public class MockRecipient : Recipient
    {
        // Implement any necessary overrides or additional properties for testing
    }
}
