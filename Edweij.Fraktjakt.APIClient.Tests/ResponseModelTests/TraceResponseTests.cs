using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using System.Net;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests
{
    [TestFixture]
    public class TraceResponseTests
    {
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            var response = await TraceResponse.FromHttpResponse(httpResponseMessage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.False);
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("HttpResponseMessage was null"));
            });
        }

        [Test]
        public async Task FromHttpResponse_NonSuccessStatusCode_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = new(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error Content")
            };

            // Act
            var response = await TraceResponse.FromHttpResponse(httpResponseMessage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.False);
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("Not successful response (BadRequest). Response Content: 'Error Content'."));
            });
        }

        [Test]
        public void FromXml_ValidXml_ShouldReturnTraceResponse()
        {
            // Arrange
            string validXml = @"
            <traceResponse>
                <server_status>OK</server_status>
                <code>0</code>
                <warning_message>Warning</warning_message>
                <error_message>Error</error_message>
                <tracking_code>123456</tracking_code>
                <tracking_link>tracking.com</tracking_link>
                <shipping_states>
                    <shipping_state>
                        <shipment_id>123</shipment_id>
                        <name>Status1</name>
                        <id>0</id>
                        <fraktjakt_id>0</fraktjakt_id>
                    </shipping_state>
                    <shipping_state>
                        <shipment_id>123</shipment_id>
                        <name>Status2</name>
                        <id>1</id>
                        <fraktjakt_id>1</fraktjakt_id>
                    </shipping_state>
                </shipping_states>
                <tracking_number>T123</tracking_number>
                <shipping_company>CompanyABC</shipping_company>
                <shipping_documents>
                    <shipping_document>Document1</shipping_document>
                    <shipping_document>Document2</shipping_document>
                </shipping_documents>
            </traceResponse>";

            // Act
            var response = TraceResponse.FromXml(validXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.True);
                Assert.That(response.ServerStatus, Is.EqualTo("OK"));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(response.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(response.ErrorMessage, Is.EqualTo("Error"));
                Assert.That(response.Result.TrackingCode, Is.EqualTo("123456"));
                Assert.That(response.Result.TrackingLink, Is.EqualTo("tracking.com"));

                Assert.That(response.Result.ShippingStates, Is.Not.Null);
                Assert.That(response.Result.ShippingStates.ToList(), Has.Count.EqualTo(2));
                Assert.That(response.Result.ShippingStates.Any(s => s.Name == "Status1"), Is.True);
                Assert.That(response.Result.ShippingStates.Any(s => s.Name == "Status2"), Is.True);

                Assert.That(response.Result.TrackingNumber, Is.EqualTo("T123"));
                Assert.That(response.Result.ShippingCompany, Is.EqualTo("CompanyABC"));

                Assert.That(response.Result.ShippingDocuments, Is.Not.Null);
                Assert.That(response.Result.ShippingDocuments.ToList(), Has.Count.EqualTo(2));
                Assert.That(response.Result.ShippingDocuments, Contains.Item("Document1"));
                Assert.That(response.Result.ShippingDocuments, Contains.Item("Document2"));
            });
        }

        [Test]
        public void FromXml_InvalidXml_ShouldReturnUnbindableResponse()
        {
            // Arrange
            string invalidXml = "<invalidXml></invalidXml>";

            // Act
            var response = TraceResponse.FromXml(invalidXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.False);
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("Invalid xml: Object reference not set to an instance of an object."));
            });
        }
    }
}
