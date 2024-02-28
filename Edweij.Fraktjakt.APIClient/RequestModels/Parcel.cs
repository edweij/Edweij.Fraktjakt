using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class Parcel : XmlRequestObject
{
    public Parcel(float weight) 
    {
        if (weight <= 0) throw new ArgumentException("Weight must be larger than 0");
        Weight = weight;
    }


    /// <summary>
    /// Parcel weight in kilograms
    /// </summary>
    public float Weight { get; init; }
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
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("parcel");
                w.WriteElementString("weight", Weight.ToStringPeriodDecimalSeparator());
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
        if (Length.HasValue && Length.Value <= 0)
        {
            yield return new RuleViolation("Length", "Length must be larger than 0");
        }

        if (Width.HasValue && Width.Value <= 0)
        {
            yield return new RuleViolation("Width", "Width must be larger than 0");
        }

        if (Height.HasValue && Height.Value <= 0)
        {
            yield return new RuleViolation("Height", "Height must be larger than 0");
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Parcel other = (Parcel)obj;
        return Weight == other.Weight
            && Nullable.Equals(Length, other.Length)
            && Nullable.Equals(Width, other.Width)
            && Nullable.Equals(Height, other.Height);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Weight.GetHashCode();
            hash = hash * 23 + (Length?.GetHashCode() ?? 0);
            hash = hash * 23 + (Width?.GetHashCode() ?? 0);
            hash = hash * 23 + (Height?.GetHashCode() ?? 0);

            return hash;
        }
    }
}
