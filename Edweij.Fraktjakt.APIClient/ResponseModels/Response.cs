using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record Response<T>(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, T? Result)
{
    public bool HasResult => Result != null;


    public static Response<T> CreateErrorResponse(string message)
    {
        return new Response<T>("Server status unknown, invalid, or no response.", ResponseStatus.Error, string.Empty, message, default);
    }

    public static Response<T> CreateErrorResponseFromXml(XElement element)
    {
        return new Response<T>(
                    element.Element("server_status")!.Value,
                    (ResponseStatus)int.Parse(element.Element("code")!.Value),
                    element.Element("warning_message")!.Value,
                    element.Element("error_message")!.Value,
                    default);
    }
}

