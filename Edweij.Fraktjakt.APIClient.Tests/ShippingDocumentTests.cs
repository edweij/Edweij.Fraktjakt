using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class ShippingDocumentTests
    {
        [Test]
        public void FromXml_ValidXml_ShouldReturnShippingDocument()
        {
            // Arrange
            XElement validXml = XElement.Parse(@"
            <shippingDocument>
                <name>DocumentName</name>
                <type_id>1</type_id>
                <type_name>DocumentType</type_name>
                <type_description>DocumentTypeDescription</type_description>
                <state_name>DocumentState</state_name>
                <state_description>DocumentStateDescription</state_description>
                <format_name>PDF</format_name>
                <file>Base64EncodedFile</file>
            </shippingDocument>");

            // Act
            ShippingDocument shippingDocument = ShippingDocument.FromXml(validXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shippingDocument.Name, Is.EqualTo("DocumentName"));
                Assert.That(shippingDocument.TypeId.Id, Is.EqualTo(1));
                Assert.That(shippingDocument.TypeName, Is.EqualTo("DocumentType"));
                Assert.That(shippingDocument.TypeDescription, Is.EqualTo("DocumentTypeDescription"));
                Assert.That(shippingDocument.StateName, Is.EqualTo("DocumentState"));
                Assert.That(shippingDocument.StateDescription, Is.EqualTo("DocumentStateDescription"));
                Assert.That(shippingDocument.FormatName, Is.EqualTo("PDF"));
                Assert.That(shippingDocument.File, Is.EqualTo("Base64EncodedFile"));
            });
        }

        [Test]
        public void PdfFromBase64_ValidFile_ShouldReturnByteArray()
        {
            // Arrange
            string base64String = "SGVsbG8gd29ybGQ="; // Example base64-encoded "Hello world"
            ShippingDocument shippingDocument = new ShippingDocument(
                "DocumentName",
                new ShippingDocumentTypeId(1),
                "DocumentType",
                "DocumentTypeDescription",
                "DocumentState",
                "DocumentStateDescription",
                "PDF",
                base64String);

            // Act
            byte[] result = shippingDocument.PdfFromBase64();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Length, Is.GreaterThan(0));
                Assert.That(Convert.ToBase64String(result), Is.EqualTo(base64String));
            });
        }

        [Test]
        public void PdfFromBase64_InvalidFile_ShouldThrowException()
        {
            // Arrange
            ShippingDocument shippingDocument = new ShippingDocument(
                "DocumentName",
                new ShippingDocumentTypeId(1),
                "DocumentType",
                "DocumentTypeDescription",
                "DocumentState",
                "DocumentStateDescription",
                "PDF",
                "InvalidBase64String");

            // Act & Assert
            Assert.Throws<FormatException>(() => shippingDocument.PdfFromBase64());
        }
    }
}
