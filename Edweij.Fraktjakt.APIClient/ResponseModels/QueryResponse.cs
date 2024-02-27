using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record QueryResponse(CurrencyCode Currency, int Id, string AccessCode, string AccessLink, string TrackingCode, string TrackingLink, IEnumerable<ShippingProductResponse> Products)
{
    public static async Task<Response<QueryResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<QueryResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<QueryResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    public static Response<QueryResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var queryResponse = new QueryResponse(
                element.Element("currency")!.Value,
                int.Parse(element.Element("id")!.Value),
                element.Element("access_code")!.Value,
                element.Element("access_link")!.Value,
                element.Element("tracking_code")!.Value,
                element.Element("tracking_link")!.Value,
                element.Element("shipping_products") != null ? ProductsFromXml(element.Element("shipping_products")!) : Enumerable.Empty<ShippingProductResponse>())
            {
                AgentSelectionLink = element.Element("agent_selection_link") != null ? element.Element("agent_selection_link")!.Value : null
            };
            

            var result = new Response<QueryResponse>(element.Element("server_status")!.Value,
                (ResponseStatus)int.Parse(element.Element("code")!.Value),
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                queryResponse);
            
            
            return result;
        }
        catch (Exception ex)
        {
            return Response<QueryResponse>.CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

    public string? AgentSelectionLink { get; init; } = null;
    
    private static IEnumerable<ShippingProductResponse> ProductsFromXml(XElement el)
    {
        var products = new List<ShippingProductResponse>();
        foreach (var shippingProduct in el.Elements("shipping_product"))
        {
            products.Add(ShippingProductResponse.FromXml(shippingProduct));
        }
        return products;
    }
}
