using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using System.Net;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests
{
    [TestFixture]
    public class ShippingDocumentsResponseTests
    {
        [Test]
        public async Task FromHttpResponse_ValidResponse_ShouldReturnShippingDocumentsResponse()
        {
            // Arrange
            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("<shippingDocumentsResponse><server_status>OK</server_status><code>0</code><warning_message>Warning</warning_message><error_message>Error</error_message><shipping_documents><shipping_document><name>Doc1</name><type_id>1</type_id><type_name>Pro Forma-faktura</type_name><type_description>Description1</type_description><state_name>State1</state_name><state_description>StateDescription1</state_description><format_name>PDF</format_name><file>Base64EncodedPDF1</file></shipping_document><shipping_document><name>Doc2</name><type_id>2</type_id><type_name>Handelsfaktura</type_name><type_description>Description2</type_description><state_name>State2</state_name><state_description>StateDescription2</state_description><format_name>PDF</format_name><file>Base64EncodedPDF2</file></shipping_document></shipping_documents></shippingDocumentsResponse>")
            };

            // Act
            var response = await ShippingDocumentsResponse.FromHttpResponse(httpResponse);

            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.True);
                Assert.That(response.ServerStatus, Is.EqualTo("OK"));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(response.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(response.ErrorMessage, Is.EqualTo("Error"));

                Assert.That(response.Result.Documents, Is.Not.Null);
                Assert.That(response.Result.Documents.ToList(), Has.Count.EqualTo(2));

                ShippingDocument doc1 = response.Result.Documents.First();
                ShippingDocument doc2 = response.Result.Documents.Skip(1).First();

                Assert.That(doc1.Name, Is.EqualTo("Doc1"));
                Assert.That(doc1.TypeId.Id, Is.EqualTo(1));
                Assert.That(doc1.TypeName, Is.EqualTo("Pro Forma-faktura"));
                Assert.That(doc1.TypeDescription, Is.EqualTo("Description1"));
                Assert.That(doc1.StateName, Is.EqualTo("State1"));
                Assert.That(doc1.StateDescription, Is.EqualTo("StateDescription1"));
                Assert.That(doc1.FormatName, Is.EqualTo("PDF"));
                Assert.That(doc1.File, Is.EqualTo("Base64EncodedPDF1"));

                Assert.That(doc2.Name, Is.EqualTo("Doc2"));
                Assert.That(doc2.TypeId.Id, Is.EqualTo(2));
                Assert.That(doc2.TypeName, Is.EqualTo("Handelsfaktura"));
                Assert.That(doc2.TypeDescription, Is.EqualTo("Description2"));
                Assert.That(doc2.StateName, Is.EqualTo("State2"));
                Assert.That(doc2.StateDescription, Is.EqualTo("StateDescription2"));
                Assert.That(doc2.FormatName, Is.EqualTo("PDF"));
                Assert.That(doc2.File, Is.EqualTo("Base64EncodedPDF2"));
            });
        }

        [Test]
        public void FromXml_ValidXml_ShouldReturnShippingDocumentsResponse()
        {
            // Arrange
            string validXml = @"
            <shippingDocumentsResponse>
                <server_status>OK</server_status>
                <code>0</code>
                <warning_message>Warning</warning_message>
                <error_message>Error</error_message>
                <shipping_documents>
                    <shipping_document>
                        <name>Doc1</name>
                        <type_id>1</type_id>
                        <type_name>Pro Forma-faktura</type_name>
                        <type_description>Description1</type_description>
                        <state_name>State1</state_name>
                        <state_description>StateDescription1</state_description>
                        <format_name>PDF</format_name>
                        <file>Base64EncodedPDF1</file>
                    </shipping_document>
                    <shipping_document>
                        <name>Doc2</name>
                        <type_id>2</type_id>
                        <type_name>Handelsfaktura</type_name>
                        <type_description>Description2</type_description>
                        <state_name>State2</state_name>
                        <state_description>StateDescription2</state_description>
                        <format_name>PDF</format_name>
                        <file>Base64EncodedPDF2</file>
                    </shipping_document>
                </shipping_documents>
            </shippingDocumentsResponse>";

            // Act
            var response = ShippingDocumentsResponse.FromXml(validXml);
                       

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.True);
                Assert.That(response.ServerStatus, Is.EqualTo("OK"));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(response.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(response.ErrorMessage, Is.EqualTo("Error"));

                Assert.That(response.Result.Documents, Is.Not.Null);
                Assert.That(response.Result.Documents.ToList(), Has.Count.EqualTo(2));

                ShippingDocument doc1 = response.Result.Documents.First();
                ShippingDocument doc2 = response.Result.Documents.Skip(1).First();

                Assert.That(doc1.Name, Is.EqualTo("Doc1"));
                Assert.That(doc1.TypeId.Id, Is.EqualTo(1));
                Assert.That(doc1.TypeName, Is.EqualTo("Pro Forma-faktura"));
                Assert.That(doc1.TypeDescription, Is.EqualTo("Description1"));
                Assert.That(doc1.StateName, Is.EqualTo("State1"));
                Assert.That(doc1.StateDescription, Is.EqualTo("StateDescription1"));
                Assert.That(doc1.FormatName, Is.EqualTo("PDF"));
                Assert.That(doc1.File, Is.EqualTo("Base64EncodedPDF1"));

                Assert.That(doc2.Name, Is.EqualTo("Doc2"));
                Assert.That(doc2.TypeId.Id, Is.EqualTo(2));
                Assert.That(doc2.TypeName, Is.EqualTo("Handelsfaktura"));
                Assert.That(doc2.TypeDescription, Is.EqualTo("Description2"));
                Assert.That(doc2.StateName, Is.EqualTo("State2"));
                Assert.That(doc2.StateDescription, Is.EqualTo("StateDescription2"));
                Assert.That(doc2.FormatName, Is.EqualTo("PDF"));
                Assert.That(doc2.File, Is.EqualTo("Base64EncodedPDF2"));
            });
        }
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ReturnsUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            var response = await ShippingDocumentsResponse.FromHttpResponse(httpResponseMessage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.False);
                Assert.That(response.ErrorMessage, Is.EqualTo("HttpResponseMessage was null"));
            });
        }

        [Test]
        public async Task FromHttpResponse_NonSuccessStatusCode_ReturnsUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error Content"),
            };

            // Act
            var response = await ShippingDocumentsResponse.FromHttpResponse(httpResponseMessage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.False);
                Assert.That(response.ErrorMessage, Is.EqualTo("Not successful response (BadRequest). Response Content: 'Error Content'."));
            });
        }

        [Test]
        [TestCase("<invalidXml>")]
        [TestCase("<shipping_documents></shipping_documents>")]
        public void FromXml_InvalidXml_ReturnsUnbindableResponse(string invalidXml)
        {
            // Arrange & Act
            var response = ShippingDocumentsResponse.FromXml(invalidXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.False);
                Assert.That(response.ErrorMessage, Does.StartWith("Invalid xml: "));
            });
        }
    }
}
