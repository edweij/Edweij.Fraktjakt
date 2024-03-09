using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents a generic response with server status, response status, messages, and a result.
/// </summary>
/// <typeparam name="T">Type of the result data.</typeparam>
public record Response<T>(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, T? Result)
{
    /// <summary>
    /// Indicates whether the response has a non-null result.
    /// </summary>
    public bool HasResult => Result != null;

    /// <summary>
    /// Creates an error response with the specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>An instance of <see cref="Response{T}"/> representing an error response.</returns>
    public static Response<T> CreateErrorResponse(string message)
    {
        return new Response<T>("Server status unknown, invalid, or no response.", ResponseStatus.Error, string.Empty, message, default);
    }

    /// <summary>
    /// Creates an error response from an XML element containing server status, response code, warning message, and error message.
    /// </summary>
    /// <param name="element">The XML element representing the response.</param>
    /// <returns>An instance of <see cref="Response{T}"/> created from the XML element.</returns>
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

