using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents the state information of a shipment in the shipping process.
/// </summary>
public record ShippingState(int ShipmentId, string Name, ShippingStateId StateId, FraktjaktStateId FraktjaktStateId)
{
    /// <summary>
    /// Creates an instance of <see cref="ShippingState"/> from an XML representation.
    /// </summary>
    /// <param name="element">The XML element containing the shipping state information.</param>
    /// <returns>An instance of <see cref="ShippingState"/> representing the parsed shipping state.</returns>
    public static ShippingState FromXml(XElement element)
    {
        return new ShippingState(int.Parse(element.Element("shipment_id")!.Value),
            element.Element("name")!.Value,
            int.Parse(element.Element("id")!.Value),
            int.Parse(element.Element("fraktjakt_id")!.Value));
    }
}
