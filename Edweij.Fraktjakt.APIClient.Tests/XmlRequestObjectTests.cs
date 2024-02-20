using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class XmlRequestObjectTests
    {
        [Test]
        public void XmlWriterSettings_ShouldHaveCorrectSettings()
        {
            // Arrange
            var xmlRequestObject = new MockXmlRequestObject();

            // Act
            var xmlWriterSettings = xmlRequestObject.ExposedXmlWriterSettings;

            // Assert
            Assert.That(xmlWriterSettings, Is.Not.Null);
            Assert.That(xmlWriterSettings.Indent, Is.False);
            Assert.That(xmlWriterSettings.Encoding, Is.EqualTo(Encoding.UTF8));
            Assert.That(xmlWriterSettings.NewLineOnAttributes, Is.False);
            Assert.That(xmlWriterSettings.CheckCharacters, Is.True);
            Assert.That(xmlWriterSettings.OmitXmlDeclaration, Is.True);
            Assert.That(xmlWriterSettings.WriteEndDocumentOnClose, Is.True);
        }

        [Test]
        public void ToXml_ShouldGenerateValidXml()
        {
            // Arrange
            var xmlRequestObject = new MockXmlRequestObject();

            // Act
            var generatedXml = xmlRequestObject.ToXml();

            // Assert
            Assert.That(generatedXml, Is.Not.Null);
            Assert.That(xmlRequestObject.IsValidXml(generatedXml), Is.True);
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
        public XmlWriterSettings ExposedXmlWriterSettings => base.XmlWriterSettings;

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
