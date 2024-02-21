using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public class PickupInfo : XmlRequestObject
    {
        public string? DrivingInstructions { get; set; } = null;
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
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
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
}
