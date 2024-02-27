using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using System.Net;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests
{
    [TestFixture]
    public class CreateShipmentResponseTests
    {
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ShouldReturnErrorResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            var response = await CreateShipmentResponse.FromHttpResponse(httpResponseMessage);

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
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error Content")
            };

            // Act
            var response = await CreateShipmentResponse.FromHttpResponse(httpResponseMessage);

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
        public void FromXml_ValidXml_ShouldReturnCreateShipmentResponse()
        {
            // Arrange
            string validXml = "<createShipmentResponse><server_status>OK</server_status><code>0</code><warning_message>Warning</warning_message><error_message>Error</error_message><shipment_id>123</shipment_id><access_code>ABC</access_code><access_link>http://example.com</access_link><return_link>http://return.com</return_link><cancel_link>http://cancel.com</cancel_link><tracking_code>123456</tracking_code><tracking_link>http://tracking.com</tracking_link></createShipmentResponse>";

            // Act
            var response = CreateShipmentResponse.FromXml(validXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.HasResult, Is.True);
                Assert.That(response.ServerStatus, Is.EqualTo("OK"));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
                Assert.That(response.WarningMessage, Is.EqualTo("Warning"));
                Assert.That(response.ErrorMessage, Is.EqualTo("Error"));
                Assert.That(response.Result.ShipmentId, Is.EqualTo(123));
                Assert.That(response.Result.AccessCode, Is.EqualTo("ABC"));
                Assert.That(response.Result.AccessLink, Is.EqualTo("http://example.com"));
                Assert.That(response.Result.ReturnLink, Is.EqualTo("http://return.com"));
                Assert.That(response.Result.CancelLink, Is.EqualTo("http://cancel.com"));
                Assert.That(response.Result.TrackingCode, Is.EqualTo("123456"));
                Assert.That(response.Result.TrackingLink, Is.EqualTo("http://tracking.com"));
            });
        }

        [Test]
        public void FromXml_InvalidXml_ShouldReturnErrorResponse()
        {
            // Arrange
            string invalidXml = "<invalidXml></invalidXml>";

            // Act
            var response = CreateShipmentResponse.FromXml(invalidXml);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
                Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
                Assert.That(response.WarningMessage, Is.Empty);
                Assert.That(response.ErrorMessage, Is.EqualTo("Invalid xml: Object reference not set to an instance of an object."));
            });
        }
    }
}
