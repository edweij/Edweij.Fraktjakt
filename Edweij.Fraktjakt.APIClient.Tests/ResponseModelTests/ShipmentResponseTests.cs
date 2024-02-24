using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using System.Net;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests
{
    [TestFixture]
    public class ShipmentResponseTests
    {
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            Response response = await ShipmentResponse.FromHttpResponse(httpResponseMessage!);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("HttpResponseMessage was null"));
            });

        }

        [Test]
        public async Task FromHttpResponse_NonSuccessStatusCode_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error Content")
            };

            // Act
            Response response = await ShipmentResponse.FromHttpResponse(httpResponseMessage);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid or no response."));
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
            ShipmentResponse shipmentResponse = ShipmentResponse.FromXml(validXml) as ShipmentResponse;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shipmentResponse, Is.Not.Null);
                Assert.That(shipmentResponse.ServerStatus, Is.EqualTo("OK"));
                Assert.That(shipmentResponse.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(shipmentResponse.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(shipmentResponse.ErrorMessage, Is.EqualTo("Error"));
                Assert.That(shipmentResponse.Currency.ToString(), Is.EqualTo("USD"));
                Assert.That(shipmentResponse.Id, Is.EqualTo(123));
                Assert.That(shipmentResponse.AccessCode, Is.EqualTo("ABC"));
                Assert.That(shipmentResponse.AccessLink, Is.EqualTo("http://example.com"));
                Assert.That(shipmentResponse.TrackingCode, Is.EqualTo("123456"));
                Assert.That(shipmentResponse.TrackingLink, Is.EqualTo("http://tracking.com"));
                Assert.That(shipmentResponse.AgentSelectionLink, Is.Null);
                Assert.That(shipmentResponse.Products, Is.Empty);
            });

        }


    }
}
