using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.ResponseModels;

namespace Edweij.Fraktjakt.APIClient.Tests.ResponseModelTests;

[TestFixture]
public class ResponseTests
{
    [Test]
    public void CreateErrorResponse_ShouldReturnErrorObject()
    {
        // Arrange
        var errorMessage = "Invalid input";

        // Act
        var errorResponse = Response<int?>.CreateErrorResponse(errorMessage);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(errorResponse.ServerStatus, Is.EqualTo("Server status unknown, invalid, or no response."));
            Assert.That(errorResponse.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(errorResponse.WarningMessage, Is.EqualTo(string.Empty));
            Assert.That(errorResponse.ErrorMessage, Is.EqualTo(errorMessage));
            Assert.That(errorResponse.Result, Is.Null);
            Assert.That(errorResponse.HasResult, Is.False);
        });
    }

    [Test]
    public void CreateSuccessResponse_ShouldReturnSuccessObject()
    {
        // Arrange
        var resultValue = 42;

        // Act
        var successResponse = new Response<int>("Online", ResponseStatus.Ok, string.Empty, string.Empty, resultValue);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(successResponse.ServerStatus, Is.EqualTo("Online"));
            Assert.That(successResponse.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            Assert.That(successResponse.WarningMessage, Is.EqualTo(string.Empty));
            Assert.That(successResponse.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(successResponse.Result, Is.EqualTo(resultValue));
            Assert.That(successResponse.HasResult, Is.True);
        });
    }

    [Test]
    public void CreateSuccessResponse_WithDifferentType_ShouldReturnSuccessObject()
    {
        // Arrange
        var resultValue = "Success string";

        // Act
        var successResponse = new Response<string>("Online", ResponseStatus.Ok, string.Empty, string.Empty, resultValue);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(successResponse.ServerStatus, Is.EqualTo("Online"));
            Assert.That(successResponse.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            Assert.That(successResponse.WarningMessage, Is.EqualTo(string.Empty));
            Assert.That(successResponse.ErrorMessage, Is.EqualTo(string.Empty));
            Assert.That(successResponse.Result, Is.EqualTo(resultValue));
            Assert.That(successResponse.HasResult, Is.True);
        });
    }
}