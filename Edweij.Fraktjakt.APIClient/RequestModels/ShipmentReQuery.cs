using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class ShipmentReQuery : XmlRequestObject
{
    public Sender Sender { get; init; }
    public int ShipmentId{ get; init; }
    public float? Value { get; set; }
    public bool ShipperInfo { get; set; } = false;

    public ShipmentReQuery(Sender sender, int shipmentId) 
    { 
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        if (shipmentId < 1) throw new ArgumentException(nameof(shipmentId));
        ShipmentId = shipmentId;
    }
    
    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (!Sender.IsValid)
        {
            yield return new RuleViolation("Sender", "Sender is not valid");
            foreach (var err in Sender.GetRuleViolations()) yield return err;
        }
        if (Value.HasValue)
        {
            if (Value.Value < 0f) yield return new RuleViolation("Value", "Don't use a negative value");
        }
    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("shipment");
                
                if (Value.HasValue)
                {
                    w.WriteElementString("value", Value.Value.ToStringPeriodDecimalSeparator());                    
                }
                w.WriteElementString("shipper_info", ShipperInfo ? "1" : "0");

                w.WriteRaw(Sender.ToXml());

                w.WriteElementString("shipment_id", ShipmentId.ToString());
            }
            return sb.ToString();                
        }
        throw new ArgumentException("Shipment element is not valid");
    }
}
