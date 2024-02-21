using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient
{
    public record ShippingState(int ShipmentId, string Name, ShippingStateId StateId, FraktjaktStateId FraktjaktStateId)
    {
        public static ShippingState FromXml(XElement element)
        {
            return new ShippingState(int.Parse(element.Element("shipment_id")!.Value),
                element.Element("name")!.Value,
                int.Parse(element.Element("id")!.Value),
                int.Parse(element.Element("fraktjakt_id")!.Value));
        }
    }
}
