﻿using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents the response from querying a shipment. Query or ReQuery
/// </summary>
/// <param name="Currency">Currency</param>
/// <param name="Id">Fraktjakt's Id. This is used in all future references to this particular query result.</param>
/// <param name="AccessCode">A code for accessing and managing the shipment without previously being logged in.</param>
/// <param name="AccessLink">A link to the shipment using the above Id and AccessCode.</param>
/// <param name="TrackingCode">A code to track this shipment without previously being logged in.</param>
/// <param name="TrackingLink">A link to track the shipment using the TrackingCode</param>
/// <param name="Products">Array of shipping products available for this shipment, sorted in the order defined on your configuration page at Fraktjakt.se (default is price sort).</param>
public record QueryResponse(CurrencyCode Currency, int Id, string AccessCode, string AccessLink, string TrackingCode, string TrackingLink, IEnumerable<ShippingProductResponse> Products)
{
    /// <summary>
    /// Creates an instance of <see cref="Response{QueryResponse}"/> from an HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
    /// <returns>An instance of <see cref="Response{QueryResponse}"/> representing the parsed response.</returns>
    public static async Task<Response<QueryResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<QueryResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<QueryResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    /// <summary>
    /// Creates an instance of <see cref="Response{QueryResponse}"/> from an XML string.
    /// </summary>
    /// <param name="xml">The XML string to parse.</param>
    /// <returns>An instance of <see cref="Response{QueryResponse}"/> representing the parsed response.</returns>
    public static Response<QueryResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var status = (ResponseStatus)int.Parse(element.Element("code")!.Value);
            if (status == ResponseStatus.Error)
            {
                return Response<QueryResponse>.CreateErrorResponseFromXml(element);
            }

            var products = element.Element("shipping_products") != null ? ProductsFromXml(element.Element("shipping_products")!) : Enumerable.Empty<ShippingProductResponse>();

            var queryResponse = new QueryResponse(
                element.Element("currency")!.Value,
                int.Parse(element.Element("id")!.Value),
                element.Element("access_code")!.Value,
                element.Element("access_link")!.Value,
                element.Element("tracking_code")!.Value,
                element.Element("tracking_link")!.Value,
                products)
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
            return Response<QueryResponse>.CreateErrorResponse($"Unable to bind response XML: {ex.GetType().Name} {ex.Message}");
        }
    }

    /// <summary>
    /// Via this link, the recipient of the freight can be offered to request an agent.
    /// <br />
    /// Use this link unless AgentLink can be presented for the various shipping products.
    /// <br />
    /// It shows the nearest agents for all shipping companies that have services with agents in the result.
    /// <br />
    /// If the recipient does not choose an agent who belongs to the service that is then selected, it will of course not be the chosen agent either.
    /// <br />
    /// Does not appear if no_agents is specified.
    /// </summary>
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
