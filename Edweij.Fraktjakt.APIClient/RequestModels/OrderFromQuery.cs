﻿using System.Text;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

/// <summary>
/// Anropstyp 1 - Skapa en order från en tidigare skapat sändning
/// </summary>
public class OrderFromQuery : Order
{
    public OrderFromQuery(Sender sender, int shippingProductId, int shipmentId, IEnumerable<ShipmentItem>? items = null) : base(sender, shippingProductId, items)
    {
        if (shipmentId < 1) throw new ArgumentException(nameof(shipmentId));
        ShipmentId = shipmentId;
    }

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
