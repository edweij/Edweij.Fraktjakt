using Edweij.Fraktjakt.APIClient.Structs;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public abstract class Address : XmlRequestObject
{
    public Address(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentException("Invalid postalcode");
        if (postalCode.Length > 16) throw new ArgumentException("postalCode max length 16");
        PostalCode = postalCode;
    }

    /// <summary>
    /// Max length 35 characters, If the shipment is to a 'C/O' address should be in this element.
    /// </summary>
    public string? StreetAddress1 { get; set; } = null;
    
    /// <summary>
    /// Max length 35 characters, Supplementary/further shippinginformation can be given within parentheses in this element. E.g. (Has unloading agreements)
    /// </summary>
    public string? StreetAddress2 { get; set; } = null;
    
    /// <summary>
    /// Max length 35 characters, For countries that require three address lines, such as Saudi Arabia, there is a third address line.
    /// </summary>
    public string? StreetAddress3 { get; set; } = null;
    
    /// <summary>
    /// Max length 16 characters
    /// </summary>
    public string PostalCode { get; init; }

    /// <summary>
    /// Max length 32 characters, required if not the PostalCode line contains a unique postal code for that city.
    /// </summary>
    public string? CityName { get; set; } = null;

    /// <summary>
    /// Specifies whether the address is residential or not (private/residential or commercial)
    /// Default is true (residental)
    /// </summary>
    public bool IsResidental { get; set; } = true;

    /// <summary>
    /// Default SE
    /// </summary>
    public CountryCode CountryCode { get; set; } = new();

    /// <summary>
    /// Directions to the driver
    /// </summary>
    public string? Instructions { get; set; } = null;

    /// <summary>
    /// Entry code / Door Code
    /// </summary>
    public string? EntryCode { get; set; } = null;

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (!string.IsNullOrEmpty(StreetAddress1))
        {
            if (StreetAddress1.Length > 35)
            {
                yield return new RuleViolation("StreetAddress1", "Max length 35");
            }
        }
        if (!string.IsNullOrEmpty(StreetAddress2))
        {
            if (StreetAddress2.Length > 35)
            {
                yield return new RuleViolation("StreetAddress2", "Max length 35");
            }
        }
        if (!string.IsNullOrEmpty(StreetAddress3))
        {
            if (StreetAddress3.Length > 35)
            {
                yield return new RuleViolation("StreetAddress3", "Max length 35");
            }
        }
        if (!string.IsNullOrEmpty(CityName))
        {
            if (CityName.Length > 32)
            {
                yield return new RuleViolation("CityName", "Max length 32");
            }
        }

    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb, ConformanceLevel.Fragment))
            {
                if (!string.IsNullOrWhiteSpace(StreetAddress1)) w.WriteElementString("street_address_1", StreetAddress1);
                if (!string.IsNullOrWhiteSpace(StreetAddress2)) w.WriteElementString("street_address_2", StreetAddress2);
                if (!string.IsNullOrWhiteSpace(StreetAddress3)) w.WriteElementString("street_address_3", StreetAddress3);
                w.WriteElementString("postal_code", PostalCode);
                if (!string.IsNullOrWhiteSpace(CityName)) w.WriteElementString("city_name", CityName);
                w.WriteElementString("residental", IsResidental ? "1" : "0");
                w.WriteElementString("country_code", CountryCode.ToString());
                if (!string.IsNullOrWhiteSpace(Instructions)) w.WriteElementString("instructions", Instructions);
                if (!string.IsNullOrWhiteSpace(EntryCode)) w.WriteElementString("entry_code", EntryCode);
            }
            return sb.ToString();

        }
        throw new ArgumentException("Address element is not valid");
    }

}
