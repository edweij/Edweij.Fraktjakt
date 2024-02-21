

using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public abstract class Address : XmlRequestObject
    {        

        public string? StreetAddress1 { get; set; } = null;
        public string? StreetAddress2 { get; set; } = null;
        public string? StreetAddress3 { get; set; } = null;
        public string? PostalCode { get; set; } = null;
        public string? CityName { get; set; } = null;
        public bool IsResidental { get; set; } = true;
        public CountryCode CountryCode { get; set; } = new();
        public string? Instructions { get; set; } = null;
        public string? EntryCode { get; set; } = null;        

        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (string.IsNullOrEmpty(PostalCode))
            {
                yield return new RuleViolation("PostalCode", "PostalCode is required");
            }
           
            yield break;
        }

        public override string ToXml()
        {
            if (IsValid)
            {
                var settings = XmlWriterSettings.Clone();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                var sb = new StringBuilder();
                using (var w = XmlWriter.Create(sb, settings))
                {
                    if (!string.IsNullOrWhiteSpace(StreetAddress1)) w.WriteElementString("street_address_1", StreetAddress1);
                    if (!string.IsNullOrWhiteSpace(StreetAddress2)) w.WriteElementString("street_address_2", StreetAddress2);
                    if (!string.IsNullOrWhiteSpace(StreetAddress3)) w.WriteElementString("street_address_3", StreetAddress3);
                    w.WriteElementString("postal_code", PostalCode!);
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
}
