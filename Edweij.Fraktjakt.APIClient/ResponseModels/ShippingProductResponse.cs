using System.Globalization;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record ShippingProductResponse(int Id, string Name, string Description, string ArrivalTime, float price,
        float TaxClass, float InsuranceFee, float InsuranceTaxClass, bool ToAgent, string AgentInfo, string AgentLink)
{
    public static ShippingProductResponse FromXml(string xml)
    {
        XElement element = XElement.Parse(xml);
        return FromXml(element);
    }

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


    public string? AgentInInfo { get; init; } = null;
    public string? AgentInLink { get; init; } = null;
    public string? ServicePointLocatorApi { get; init; } = null;
    public int? ShipperId { get; init; } = null;
    public string? ShipperName { get; init; } = null;
    public string? ShipperLogoUrl { get; init; } = null;

}
