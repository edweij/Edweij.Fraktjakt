using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

/// <summary>
/// If you've previously called Query API and got a response containing a shipmentId you can call the ReQuery API to get a cached response which is much quicker.
/// </summary>
public class ShipmentReQuery : XmlRequestObject
{
    /// <summary>
    /// Information about who's making the call to the API
    /// </summary>
    public Sender Sender { get; init; }
    public int ShipmentId{ get; init; }

    /// <summary>
    /// Value of content in the shipment.
    /// </summary>
    public float? Value { get; set; }
    public bool ShipperInfo { get; set; } = false;

    /// <summary>
    /// Create a instance of ShipmentRequery
    /// </summary>
    /// <param name="sender">Sender object with information about your integrations customer id and API key, use the sender from your client object</param>
    /// <param name="shipmentId">The shipmentId from previous Query</param>
    /// <exception cref="ArgumentNullException">If the sender parameter is null</exception>
    /// <exception cref="ArgumentException">For invalid shipmentId</exception>
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
