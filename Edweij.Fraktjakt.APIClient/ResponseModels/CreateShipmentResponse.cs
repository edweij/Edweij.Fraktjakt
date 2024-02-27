using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record CreateShipmentResponse(int ShipmentId, string AccessCode, string AccessLink, string ReturnLink, string CancelLink, string TrackingCode, string TrackingLink)
{
    public static async Task<Response<CreateShipmentResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<CreateShipmentResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<CreateShipmentResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    public static Response<CreateShipmentResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var createShipmentResponse = new CreateShipmentResponse(
                int.Parse(element.Element("shipment_id")!.Value),
                element.Element("access_code")!.Value,
                element.Element("access_link")!.Value,
                element.Element("return_link")!.Value,
                element.Element("cancel_link")!.Value,
                element.Element("tracking_code")!.Value,
                element.Element("tracking_link")!.Value);
            
            var result = new Response<CreateShipmentResponse>(element.Element("server_status")!.Value,
                (ResponseStatus)int.Parse(element.Element("code")!.Value),
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
