using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record QueryResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, CurrencyCode Currency,
        int Id, string AccessCode, string AccessLink, string TrackingCode, string TrackingLink) : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
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
            var result = new QueryResponse(element.Element("server_status")!.Value,
                (ResponseStatus)int.Parse(element.Element("code")!.Value),
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                element.Element("currency")!.Value,
                int.Parse(element.Element("id")!.Value),
                element.Element("access_code")!.Value,
                element.Element("access_link")!.Value,
                element.Element("tracking_code")!.Value,
                element.Element("tracking_link")!.Value)
            {
                AgentSelectionLink = element.Element("agent_selection_link") != null ? element.Element("agent_selection_link")!.Value : null
            };
            var products = new List<ShippingProductResponse>();
            foreach (var shippingProduct in element.Element("shipping_products")!.Elements("shipping_product"))
            {
                products.Add(ShippingProductResponse.FromXml(shippingProduct));
            }
            result.Products = products;
            return result;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

    public string? AgentSelectionLink { get; init; } = null;
    public IEnumerable<ShippingProductResponse> Products { get; private set; } = Enumerable.Empty<ShippingProductResponse>();
}
