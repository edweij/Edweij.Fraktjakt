using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class PickupInfo : XmlRequestObject
{
    /// <summary>
    /// Instructions on how to find the place where the shipment is to be picked up for delivery.
    /// Max length 50 characters
    /// </summary>
    public string? DrivingInstructions { get; set; } = null;

    /// <summary>
    /// The date when the shipment is to be picked up.
    /// This must be a date in the future.
    /// </summary>
    public DateTime? PickupDate { get; set; } = null;

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (!string.IsNullOrEmpty(DrivingInstructions) && DrivingInstructions.Length > 50) yield return new RuleViolation("DrivingInstructions", "Max length 50");
        if (PickupDate.HasValue && PickupDate.Value.Date < DateTime.Now.Date) yield return new RuleViolation("PickupDate", "Must be today or greater");
        yield break;
    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("booking");
                if (!string.IsNullOrEmpty(DrivingInstructions)) w.WriteElementString("driving_instructions", DrivingInstructions);
                if (PickupDate.HasValue) w.WriteElementString("pickup_date", PickupDate.Value.ToString("yyyy-MM-dd"));                    
            }
            return sb.ToString();                
        }
        throw new ArgumentException("PickupInfo element is not valid");
    }
}
