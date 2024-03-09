using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Globalization;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents the response from placing an order.
/// </summary>
public record OrderResponse(int ShipmentId, string AccessCode, string AccessLink,
    string ReturnLink, string CancelLink, string TrackingCode, string TrackingLink, float Amount, CurrencyCode Currency, string AgentInfo, string AgentLink)
{
    /// <summary>
    /// Gets or sets the service point locator API link.
    /// </summary>
    public string? ServicePointLocatorApi { get; init; } = null;

    /// <summary>
    /// Gets or sets the payment link.
    /// </summary>
    public string? PaymentLink { get; init; } = null;

    /// <summary>
    /// Gets or sets the sender email link.
    /// </summary>
    public string? SenderEmailLink { get; init; } = null;

    /// <summary>
    /// Creates an instance of <see cref="Response{OrderResponse}"/> from an HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
    /// <returns>An instance of <see cref="Response{OrderResponse}"/> representing the parsed response.</returns>
    public static async Task<Response<OrderResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<OrderResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<OrderResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    /// <summary>
    /// Creates an instance of <see cref="Response{OrderResponse}"/> from an XML string.
    /// </summary>
    /// <param name="xml">The XML string to parse.</param>
    /// <returns>An instance of <see cref="Response{OrderResponse}"/> representing the parsed response.</returns>
    public static Response<OrderResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var status = (ResponseStatus)int.Parse(element.Element("code")!.Value);

            if (status == ResponseStatus.Error)
            {
                return Response<OrderResponse>.CreateErrorResponseFromXml(element);
            }

            var orderResponse = new OrderResponse(
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

            var result = new Response<OrderResponse>(element.Element("server_status")!.Value,
                status,
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                orderResponse);

            return result;
        }
        catch (Exception ex)
        {
            return Response<OrderResponse>.CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }




}
