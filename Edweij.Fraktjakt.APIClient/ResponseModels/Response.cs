using Edweij.Fraktjakt.APIClient.Enums;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record Response(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage)
{
    protected static Response CreateErrorResponse(string message)
    {
        return new Response("Server status unknown, no or invalid response.", ResponseStatus.Error, string.Empty, message);
    }
}
