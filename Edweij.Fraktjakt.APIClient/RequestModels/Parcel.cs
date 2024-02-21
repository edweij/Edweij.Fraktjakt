using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class Parcel : XmlRequestObject
{
    /// <summary>
    /// Parcel weight in kilograms
    /// </summary>
    public float? Weight { get; set; }
    /// <summary>
    /// Parcel length in centimeters
    /// </summary>
    public float? Length { get; set; }
    /// <summary>
    /// Parcel width in centimeters
    /// </summary>
    public float? Width { get; set; }
    /// <summary>
    /// Parcel height in centimeters
    /// </summary>
    public float? Height { get; set; }
    
    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb, XmlWriterSettings))
            {
                w.WriteStartElement("parcel");
                w.WriteElementString("weight", Weight!.Value.ToStringPeriodDecimalSeparator());
                if (Length.HasValue) w.WriteElementString("length", Length!.Value.ToStringPeriodDecimalSeparator());
                if (Width.HasValue) w.WriteElementString("width", Width!.Value.ToStringPeriodDecimalSeparator());
                if (Height.HasValue) w.WriteElementString("height", Height!.Value.ToStringPeriodDecimalSeparator());
            }
            return sb.ToString();
        }
        throw new ArgumentException("Parcel element is not valid");
    }

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (!Weight.HasValue)
        {
            yield return new RuleViolation("Weight", "Weight is required");
        }
        else if (Weight.Value <= 0)
        {
            yield return new RuleViolation("Weight", "Weight must be larger than 0");
        }

        if (Length.HasValue)
        {
            if (Length.Value <= 0)
            {
                yield return new RuleViolation("Length", "Length must be larger than 0");
            }
        }

        if (Width.HasValue)
        {
            if (Width.Value <= 0)
            {
                yield return new RuleViolation("Width", "Width must be larger than 0");
            }
        }

        if (Height.HasValue)
        {
            if (Height.Value <= 0)
            {
                yield return new RuleViolation("Height", "Height must be larger than 0");
            }
        }


        yield break;
    }

    
}
