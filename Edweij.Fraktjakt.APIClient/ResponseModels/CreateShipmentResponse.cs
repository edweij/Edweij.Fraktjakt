using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents the response from creating a shipment.
/// </summary>
public record CreateShipmentResponse(int ShipmentId, string AccessCode, string AccessLink, string ReturnLink, string CancelLink, string TrackingCode, string TrackingLink)
{
    /// <summary>
    /// Creates an instance of <see cref="Response{CreateShipmentResponse}"/> from an HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
    /// <returns>An instance of <see cref="Response{CreateShipmentResponse}"/> representing the parsed response.</returns>
    public static async Task<Response<CreateShipmentResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<CreateShipmentResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<CreateShipmentResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    /// <summary>
    /// Creates an instance of <see cref="Response{CreateShipmentResponse}"/> from an XML string.
    /// </summary>
    /// <param name="xml">The XML string to parse.</param>
    /// <returns>An instance of <see cref="Response{CreateShipmentResponse}"/> representing the parsed response.</returns>
    public static Response<CreateShipmentResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var status = (ResponseStatus)int.Parse(element.Element("code")!.Value);

            if (status == ResponseStatus.Error)
            {
                return Response<CreateShipmentResponse>.CreateErrorResponseFromXml(element);
            }


            var createShipmentResponse = new CreateShipmentResponse(
                int.Parse(element.Element("shipment_id")!.Value),
                element.Element("access_code")!.Value,
                element.Element("access_link")!.Value,
                element.Element("return_link")!.Value,
                element.Element("cancel_link")!.Value,
                element.Element("tracking_code")!.Value,
                element.Element("tracking_link")!.Value);
            
            var result = new Response<CreateShipmentResponse>(element.Element("server_status")!.Value,
                status,
                element.Element("warning_message") != null ? element.Element("warning_message")!.Value : string.Empty,
                element.Element("error_message") != null ? element.Element("error_message")!.Value : string.Empty,
                createShipmentResponse);

            return result;
        }
        catch (Exception ex)
        {
            return Response<CreateShipmentResponse>.CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

}
