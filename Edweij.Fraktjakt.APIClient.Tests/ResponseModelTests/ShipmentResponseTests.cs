using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using System.Net;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests
{
    [TestFixture]
    public class ShipmentResponseTests
    {
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ShouldReturnErrorResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            var response = await QueryResponse.FromHttpResponse(httpResponseMessage!);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("HttpResponseMessage was null"));
            });

        }

        [Test]
        public async Task FromHttpResponse_NonSuccessStatusCode_ShouldReturnErrorResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = new(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error Content")
            };

            // Act
            var response = await QueryResponse.FromHttpResponse(httpResponseMessage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("Not successful response (BadRequest). Response Content: 'Error Content'."));
            });

        }

        [Test]
        public void FromXml_ValidXml_ShouldReturnShipmentResponse()
        {
            // Arrange
            string validXml = "<shipmentResponse><server_status>OK</server_status><code>0</code><warning_message>Warning</warning_message><error_message>Error</error_message><currency>USD</currency><id>123</id><access_code>ABC</access_code><access_link>http://example.com</access_link><tracking_code>123456</tracking_code><tracking_link>http://tracking.com</tracking_link><shipping_products></shipping_products></shipmentResponse>";

            // Act
            var response = QueryResponse.FromXml(validXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.True);
                Assert.That(response.ServerStatus, Is.EqualTo("OK"));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(response.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(response.ErrorMessage, Is.EqualTo("Error"));
                Assert.That(response.Result.Currency.ToString(), Is.EqualTo("USD"));
                Assert.That(response.Result.Id, Is.EqualTo(123));
                Assert.That(response.Result.AccessCode, Is.EqualTo("ABC"));
                Assert.That(response.Result.AccessLink, Is.EqualTo("http://example.com"));
                Assert.That(response.Result.TrackingCode, Is.EqualTo("123456"));
                Assert.That(response.Result.TrackingLink, Is.EqualTo("http://tracking.com"));
                Assert.That(response.Result.AgentSelectionLink, Is.Null);
                Assert.That(response.Result.Products, Is.Empty);
            });

        }


    }
}
