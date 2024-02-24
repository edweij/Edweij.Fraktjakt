using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Globalization;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record OrderResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, int ShipmentId, string AccessCode, string AccessLink,
    string ReturnLink, string CancelLink, string TrackingCode, string TrackingLink, float Amount, CurrencyCode Currency, string AgentInfo, string AgentLink) : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
{
    public static async Task<Response> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    public static Response FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var result = new OrderResponse(element.Element("server_status")!.Value,
                (ResponseStatus)int.Parse(element.Element("code")!.Value),
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                int.Parse(element.Element("shipment_id")!.Value),
                element.Element("access_code")!.Value,
                element.Element("access_link")!.Value,
                element.Element("return_link")!.Value,
                element.Element("cancel_link")!.Value,
                element.Element("tracking_code")!.Value,
                element.Element("tracking_link")!.Value,
                float.Parse(element.Element("amount")!.Value, CultureInfo.InvariantCulture),
                element.Element("currency")!.Value,
                element.Element("agent_info")!.Value,
                element.Element("agent_link")!.Value)
            {
                PaymentLink = element.Element("payment_link") != null ? element.Element("payment_link")!.Value : null,
                SenderEmailLink = element.Element("sender_email_link") != null ? element.Element("sender_email_link")!.Value : null,
                ServicePointLocatorApi = element.Element("service_point_locator_api") != null ? element.Element("service_point_locator_api")!.Value : null
            };

            return result;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

    public string? ServicePointLocatorApi { get; init; } = null;
    public string? PaymentLink { get; init; } = null;
    public string? SenderEmailLink { get; init; } = null;



}
