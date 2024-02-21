using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class PartyTests
    {
        [Test]
        public void RuleViolations_WithValidData_ShouldReturnEmptyList()
        {
            // Arrange
            var party = new MockParty
            {
                CompanyName = "ABC Company",
                PersonName = "John Doe",
                Phone = "1234567890",
                Email = "john.doe@example.com",
                Eori = "E123456789",
                Tin = "123456789012345678"
            };

            // Act & Assert
            Assert.That(party.GetRuleViolations(), Is.Empty);
        }

        [Test]
        public void RuleViolations_WithInvalidData()
        {
            // Arrange
            var party = new MockParty
            {
                CompanyName = "A Very Long Company Name That Exceeds Maximum Length",
                PersonName = "A Very Long Person Name That Exceeds Maximum Length",
                Phone = "1234567890123456789012345678901234567890", // Exceeds maximum length
                Email = "invalid-email-format", // Invalid email format
                Eori = "E12345678901234567890123456789011", // Exceeds maximum length
                Tin = "123456789012345678901234567890123456789012345678901" // Exceeds maximum length
            };

            // Act
            var ruleViolations = party.GetRuleViolations();

            // Assert
            Assert.That(ruleViolations, Has.Exactly(6).Items
                                           .And.Some.Property("PropertyName").EqualTo("CompanyName")
                                           .And.Some.Property("PropertyName").EqualTo("PersonName")
                                           .And.Some.Property("PropertyName").EqualTo("Phone")
                                           .And.Some.Property("PropertyName").EqualTo("Email")
                                           .And.Some.Property("PropertyName").EqualTo("Eori")
                                           .And.Some.Property("PropertyName").EqualTo("Tin"));
        }       

        [Test]
        public void RuleViolations_WithInvalidEmailFormat_ShouldReturnViolation()
        {
            // Arrange
            var party = new MockParty
            {
                Email = "a very long invalid email address should generate two rule violation"
            };

            // Act & Assert
            Assert.That(party.GetRuleViolations(), Has.Exactly(2).Items
                                                    .And.Some.Property("PropertyName").EqualTo("Email")
                                                    .And.Some.Property("Error").EqualTo("Max length 64")
                                                    .And.Some.Property("PropertyName").EqualTo("Email")
                                                    .And.Some.Property("Error").EqualTo("Not valid"));
        }

        // Add more test cases for other properties as needed
    }    

    public class MockParty : Party
    {
        // Implement any necessary overrides or additional properties for testing
        public override string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}