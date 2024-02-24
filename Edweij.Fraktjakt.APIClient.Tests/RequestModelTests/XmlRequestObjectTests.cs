using Edweij.Fraktjakt.APIClient.RequestModels;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests
{
    [TestFixture]
    public class XmlRequestObjectTests
    {


        [Test]
        public void ToXml_ShouldGenerateValidXml()
        {
            // Arrange
            var xmlRequestObject = new MockXmlRequestObject();

            // Act
            var generatedXml = xmlRequestObject.ToXml();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(generatedXml, Is.Not.Null);
                Assert.That(xmlRequestObject.IsValidXml(generatedXml), Is.True);
            });
        }

        [Test]
        public void IsValidXml_ShouldReturnTrueForValidXml()
        {
            // Arrange
            var xmlRequestObject = new MockXmlRequestObject();
            string validXml = "<testElement><childElement>TestValue</childElement></testElement>";

            // Act & Assert
            Assert.That(xmlRequestObject.IsValidXml(validXml), Is.True);
        }

        [Test]
        public void IsValidXml_ShouldReturnFalseForInvalidXml()
        {
            // Arrange
            var xmlRequestObject = new MockXmlRequestObject();
            string invalidXml = "<testElement><childElement>TestValue</testElement>";

            // Act & Assert
            Assert.That(xmlRequestObject.IsValidXml(invalidXml), Is.False);
        }
    }

    public class MockXmlRequestObject : XmlRequestObject
    {
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            throw new NotImplementedException();
        }

        public override string ToXml()
        {
            // Implement the ToXml method for testing purposes
            return "<testElement><childElement>TestValue</childElement></testElement>";
        }
    }
}
