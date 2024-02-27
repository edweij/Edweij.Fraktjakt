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
    public class OrderResponseTests
    {
        [Test]
        public async Task FromHttpResponse_NullHttpResponseMessage_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = null;

            // Act
            var response = await OrderResponse.FromHttpResponse(httpResponseMessage);

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
        public async Task FromHttpResponse_NonSuccessStatusCode_ShouldReturnUnbindableResponse()
        {
            // Arrange
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error Content")
            };

            // Act
            var response = await OrderResponse.FromHttpResponse(httpResponseMessage);

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
        public void FromXml_ValidXml_ShouldReturnOrderResponse()
        {
            // Arrange
            string validXml = "<orderResponse><server_status>OK</server_status><code>0</code><warning_message>Warning</warning_message><error_message>Error</error_message><shipment_id>123</shipment_id><access_code>ABC</access_code><access_link>http://example.com</access_link><return_link>http://return.com</return_link><cancel_link>http://cancel.com</cancel_link><tracking_code>123456</tracking_code><tracking_link>http://tracking.com</tracking_link><amount>123.45</amount><currency>USD</currency><agent_info>Agent Info</agent_info><agent_link>http://agent.com</agent_link></orderResponse>";

            // Act
            var response = OrderResponse.FromXml(validXml);

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
                Assert.That(response.Result.Amount, Is.EqualTo(123.45f));
                Assert.That(response.Result.Currency.ToString(), Is.EqualTo("USD"));
                Assert.That(response.Result.AgentInfo, Is.EqualTo("Agent Info"));
                Assert.That(response.Result.AgentLink, Is.EqualTo("http://agent.com"));
                Assert.That(response.Result.PaymentLink, Is.Null);
                Assert.That(response.Result.SenderEmailLink, Is.Null);
                Assert.That(response.Result.ServicePointLocatorApi, Is.Null);
            });

        }

    }
}
