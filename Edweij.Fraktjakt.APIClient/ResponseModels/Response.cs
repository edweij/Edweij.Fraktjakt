namespace Edweij.Fraktjakt.APIClient
{
    public record Response(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage)
    {
        protected static Response UnbindableResponse(string message)
        {
            return new Response("Server status unknown, no or invalid response.", ResponseStatus.Error, string.Empty, message);
        }
    }
}
