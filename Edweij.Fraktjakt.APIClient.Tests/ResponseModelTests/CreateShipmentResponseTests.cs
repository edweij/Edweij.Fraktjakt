using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests
{
    [TestFixture]
    public class CreateShipmentResponseTests
    {
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            Response response = await CreateShipmentResponse.FromHttpResponse(httpResponseMessage);

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
            Response response = await CreateShipmentResponse.FromHttpResponse(httpResponseMessage);

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
        public void FromXml_ValidXml_ShouldReturnCreateShipmentResponse()
        {
            // Arrange
            string validXml = "<createShipmentResponse><server_status>OK</server_status><code>0</code><warning_message>Warning</warning_message><error_message>Error</error_message><shipment_id>123</shipment_id><access_code>ABC</access_code><access_link>http://example.com</access_link><return_link>http://return.com</return_link><cancel_link>http://cancel.com</cancel_link><tracking_code>123456</tracking_code><tracking_link>http://tracking.com</tracking_link></createShipmentResponse>";

            // Act
            CreateShipmentResponse shipmentResponse = CreateShipmentResponse.FromXml(validXml) as CreateShipmentResponse;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shipmentResponse, Is.Not.Null);
                Assert.That(shipmentResponse.ServerStatus, Is.EqualTo("OK"));
                Assert.That(shipmentResponse.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(shipmentResponse.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(shipmentResponse.ErrorMessage, Is.EqualTo("Error"));
                Assert.That(shipmentResponse.ShipmentId, Is.EqualTo(123));
                Assert.That(shipmentResponse.AccessCode, Is.EqualTo("ABC"));
                Assert.That(shipmentResponse.AccessLink, Is.EqualTo("http://example.com"));
                Assert.That(shipmentResponse.ReturnLink, Is.EqualTo("http://return.com"));
                Assert.That(shipmentResponse.CancelLink, Is.EqualTo("http://cancel.com"));
                Assert.That(shipmentResponse.TrackingCode, Is.EqualTo("123456"));
                Assert.That(shipmentResponse.TrackingLink, Is.EqualTo("http://tracking.com"));
            });
        }

        [Test]
        public void FromXml_InvalidXml_ShouldReturnUnbindableResponse()
        {
            // Arrange
            string invalidXml = "<invalidXml></invalidXml>";

            // Act
            Response response = CreateShipmentResponse.FromXml(invalidXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("Invalid xml: Object reference not set to an instance of an object."));
            });
        }
    }
}
