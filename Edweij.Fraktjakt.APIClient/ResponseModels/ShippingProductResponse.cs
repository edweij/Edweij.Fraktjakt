using System.Globalization;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Shipping product, i.e. a shipping service that matches the search criteria that were sent in the Query.
/// </summary>
/// <param name="Id">The shipping product's ID in Fraktjakt. This can be used for purchasing the shipping product.</param>
/// <param name="Name">The name of the shipping product. The name presented on the service can be changed in the integration settings in Fraktjakt.</param>
/// <param name="Description">Description of the shipping product. This can be presented directly to your customers.</param>
/// <param name="ArrivalTime">The arrival time; either the number of days from now or the expected date/time of arrival.</param>
/// <param name="price">The shipping product's total price without VAT, but with any selected insurance if the company's settings or input determines that insurance should be used if necessary. In the requested currency.</param>
/// <param name="TaxClass">The VAT percentage that applies to this shipping product including the VAT on any insurance.</param>
/// <param name="InsuranceFee">The cost of any additional insurance. Included in the price tag if insurance is selected by settings or input data. Displayed in the requested currency</param>
/// <param name="InsuranceTaxClass">The insurance's VAT (percentage). As a rule, always zero.</param>
/// <param name="ToAgent">If the product is delivered by an agent or not. </param>
/// <param name="AgentInfo">The shipping agent location that is closest to the receiver's address (does not apply to direct delivery shipping products). If Fraktjakt is missing information about nearest agent it is announced with the text "Agent information is missing”. Empty for shipping products that don’t use agents.</param>
/// <param name="AgentLink">A link to a map that shows where the In this link, the consignee of the freight can be offered to choose a representative for the service. Does not appear if no_agents is specified. </param>
public record ShippingProductResponse(int Id, string Name, string Description, string ArrivalTime, float price,
        float TaxClass, float InsuranceFee, float InsuranceTaxClass, bool ToAgent, string AgentInfo, string AgentLink)
{
    /// <summary>
    /// Creates an instance of <see cref="ShippingProductResponse"/> from an XML string.
    /// </summary>
    /// <param name="xml">The XML representation to parse.</param>
    /// <returns>An instance of <see cref="ShippingProductResponse"/> representing the parsed response.</returns>
    public static ShippingProductResponse FromXml(string xml)
    {
        XElement element = XElement.Parse(xml);
        return FromXml(element);
    }

    /// <summary>
    /// Creates an instance of <see cref="ShippingProductResponse"/> from an <see cref="XElement"/>.
    /// </summary>
    /// <param name="xml">The XML representation to parse.</param>
    /// <returns>An instance of <see cref="ShippingProductResponse"/> representing the parsed response.</returns>
    public static ShippingProductResponse FromXml(XElement element)
    {
        var result = new ShippingProductResponse(
            int.Parse(element.Element("id")!.Value),
            element.Element("name")!.Value,
            element.Element("description")!.Value,
            element.Element("arrival_time")!.Value,
            float.Parse(element.Element("price")!.Value, CultureInfo.InvariantCulture),
            float.Parse(element.Element("tax_class")!.Value, CultureInfo.InvariantCulture),
            float.Parse(element.Element("insurance_fee")!.Value, CultureInfo.InvariantCulture),
            float.Parse(element.Element("insurance_tax_class")!.Value, CultureInfo.InvariantCulture),
            element.Element("to_agent")!.Value == "1" ? true : false,
            element.Element("agent_info")!.Value,
            element.Element("agent_link")!.Value)
        {
            AgentInInfo = element.Element("agent_in_info") != null ? element.Element("agent_in_info")!.Value : null,
            AgentInLink = element.Element("agent_in_link") != null ? element.Element("agent_in_link")!.Value : null,
            ServicePointLocatorApi = element.Element("service_point_locator_api") != null ? element.Element("service_point_locator_api")!.Value : null,
            ShipperId = element.Element("shipper") != null ? int.Parse(element.Element("shipper")!.Element("id")!.Value) : null,
            ShipperName = element.Element("shipper") != null ? element.Element("shipper")!.Element("name")!.Value : null,
            ShipperLogoUrl = element.Element("shipper") != null ? element.Element("shipper")!.Element("logo_url")!.Value : null,
        };


        return result;
    }

    /// <summary>
    /// The shipping agent location that is closest to the sender's address (does not apply to shipping products with pickup). 
    /// <br />
    /// Is only sent if AgentsIn in the Query is true.
    /// </summary>
    public string? AgentInInfo { get; init; } = null;

    /// <summary>
    /// A link to a map that shows where the shipping agent defined is located.
    /// <br />
    /// Is only sent if AgentsIn in the Query is true.
    /// <br />
    /// If you have your own Layout Settings in the integration settings, these are used on the page.In this way, the page can be easily adapted to your own page.
    /// </summary>
    public string? AgentInLink { get; init; } = null;

    /// <summary>
    /// An link to get Service Points for this service and the recipient address as JSON.
    /// </summary>
    public string? ServicePointLocatorApi { get; init; } = null;

    /// <summary>
    /// The id for the shipper delivering the shipping product.
    /// <br />
    /// This is only availabe if ShipperInfo is set to True in the Query
    /// </summary>
    public int? ShipperId { get; init; } = null;

    /// <summary>
    /// The name of the shipper delivering the shipping product.
    /// <br />
    /// This is only availabe if ShipperInfo is set to True in the Query
    /// </summary>
    public string? ShipperName { get; init; } = null;

    /// <summary>
    /// A url to a logo for the shipper delivering the shipping product.
    /// <br />
    /// This is only availabe if ShipperInfo is set to True in the Query
    /// </summary>
    public string? ShipperLogoUrl { get; init; } = null;

}
