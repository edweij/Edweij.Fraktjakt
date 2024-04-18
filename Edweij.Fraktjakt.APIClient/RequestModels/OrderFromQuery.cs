using System.Text;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

/// <summary>
/// Create an order from a previously created shipment (Query)
/// </summary>
public class OrderFromQuery : Order
{
    public OrderFromQuery(Sender sender, int shippingProductId, int shipmentId, IEnumerable<ShipmentItem>? items = null) : base(sender, shippingProductId, items)
    {
        if (shipmentId < 1) throw new ArgumentException(nameof(shipmentId));
        ShipmentId = shipmentId;
    }
    /// <summary>
    /// This must be an ID that you have received in a response from the Query API. <br/>
    /// If you have already used the same shipment ID in a previous call to the Order API, <br />
    /// Fraktjakt will create a new copy of the existing shipment record with a new shipment ID (but with the same shipment details).
    /// </summary>
    public int ShipmentId { get; init; }

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        foreach (var err in base.GetRuleViolations())
        {
            yield return err;
        }
        yield break;
    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var xml = XElement.Parse(base.ToXml());
            xml.Add(new XElement("shipment_id", ShipmentId));
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
            {
                xml.WriteTo(w);
            }
            return sb.ToString();
        }
        throw new ArgumentException("Order element is not valid");
    }
}
