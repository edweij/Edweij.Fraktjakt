using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void UnbindableResponse_ShouldCreateErrorResponse()
        {
            // Arrange
            string errorMessage = "Sample error message";

            // Act
            Response response = MockResponse.Exposed(errorMessage);

            // Assert
            Assert.That(response.ServerStatus, Is.EqualTo("Server status unknown, no or invalid response."));
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.WarningMessage, Is.EqualTo(string.Empty));
            Assert.That(response.ErrorMessage, Is.EqualTo(errorMessage));
        }
    }

    public record MockResponse : Response
    {
        public MockResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage) : base(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
        {
            
        }

        public static Response Exposed(string message) => Response.UnbindableResponse(message);
    }
}
