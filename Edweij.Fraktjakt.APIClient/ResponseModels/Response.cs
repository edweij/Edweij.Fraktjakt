using Edweij.Fraktjakt.APIClient.Enums;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record Response<T>(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, T? Result)
{
    public bool HasResult => Result != null;


    public static Response<T> CreateErrorResponse(string message)
    {
        return new Response<T>("Server status unknown, invalid, or no response.", ResponseStatus.Error, string.Empty, message, default);
    }
}

