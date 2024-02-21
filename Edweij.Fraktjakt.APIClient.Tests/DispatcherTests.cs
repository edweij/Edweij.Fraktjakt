using Edweij.Fraktjakt.APIClient.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class DispatcherTests
    {       

        [Test]
        public void RuleViolations_ShouldBeValid_WithAllValidData()
        {
            // Arrange
            var dispatcher = new MockDispatcher
            {
                CompanyName = "ABC Company",
                PersonName = "John Doe",
                Phone = "1234567890",
                Email = "john.doe@example.com",
                Eori = "E123456789",
                Rex = "ValidRex",
                Voec = "ValidVoec",
                GbVat = "GB12345678",
                Ioss = "ValidIoss",
                Tin = "123456789012345678"
            };

            // Act
            var ruleViolations = dispatcher.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Is.Empty);
        }
        [Test]
        public void RuleViolations_WithInvalidData()
        {
            // Arrange
            var dispatcher = new MockDispatcher
            {
                CompanyName = "A Very Long Company Name That Exceeds Maximum Length",
                PersonName = "A Very Long Person Name That Exceeds Maximum Length",
                Phone = "1234567890123456789012345678901234567890", // Exceeds maximum length
                Email = "invalid-email-format", // Invalid email format
                Eori = "E12345678901234567890123456789013", // Exceeds maximum length
                Rex = "A Very Long Rex That Exceeds Maximum Length",
                Voec = "A Very Long Voec That Exceeds Maximum Length",
                GbVat = "InvalidGbVatFormat", // Invalid GbVat format
                Ioss = "A Very Long Ioss That Exceeds Maximum Length", // Exceeds maximum length
                Tin = "123456789012345678901" // Exceeds maximum length
            };

            // Act
            var ruleViolations = dispatcher.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Has.Exactly(10).Items
                                           .And.Some.Property("PropertyName").EqualTo("CompanyName")
                                           .And.Some.Property("PropertyName").EqualTo("PersonName")
                                           .And.Some.Property("PropertyName").EqualTo("Phone")
                                           .And.Some.Property("PropertyName").EqualTo("Email")
                                           .And.Some.Property("PropertyName").EqualTo("Eori")
                                           .And.Some.Property("PropertyName").EqualTo("Rex")
                                           .And.Some.Property("PropertyName").EqualTo("Voec")
                                           .And.Some.Property("PropertyName").EqualTo("GbVat")
                                           .And.Some.Property("PropertyName").EqualTo("Ioss"));
        }

        [Test]
        public void ToXml_WithValidData_ShouldGenerateValidXml()
        {
            // Arrange
            var dispatcher = new MockDispatcher
            {
                CompanyName = "ABC Company",
                Rex = "ValidRex",
                Voec = "ValidVoec",
                GbVat = "GB12345678",
                Ioss = "ValidIoss"
            };

            // Act
            string generatedXml = dispatcher.ToXml();

            // Assert
            Assert.That(() => dispatcher.ToXml(), Throws.Nothing);
            Assert.That(generatedXml, Does.Contain("<sender>")
                                       .And.Contain("<company_from>ABC Company</company_from>")
                                       .And.Contain("<rex>ValidRex</rex>")
                                       .And.Contain("<voec>ValidVoec</voec>")
                                       .And.Contain("<gb_vat>GB12345678</gb_vat>")
                                       .And.Contain("<ioss>ValidIoss</ioss>")
                                       .And.Contain("</sender>")
                                       .And.Not.Contain(Environment.NewLine)); // Ensures it's a single line
        }

        [Test]
        public void ToXml_WithInvalidData_ShouldThrowArgumentException()
        {
            // Arrange
            var dispatcher = new MockDispatcher
            {
                CompanyName = "ABC Company",
                Rex = "ValidRex",
                Voec = "ValidVoec",
                GbVat = "InvalidGbVatFormat", // Invalid GbVat format
                Ioss = "ValidIoss"
            };

            // Act & Assert
            Assert.That(() => dispatcher.ToXml(), Throws.ArgumentException.With.Message.EqualTo("Dispatcher element is not valid"));
        }

        


    }

    public class MockDispatcher : Dispatcher
    {
        // Implement any necessary overrides or additional properties for testing
    }
}
