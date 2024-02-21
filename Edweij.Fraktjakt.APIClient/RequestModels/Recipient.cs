using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public class Recipient : Party
    {
        public string? Mobile { get; set; } = null;
        public string? Message { get; set; } = null;
        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            foreach (var err in base.GetRuleViolations())
            {
                yield return err;
            }
            if (!string.IsNullOrEmpty(Mobile) && Mobile.Length > 32) yield return new RuleViolation("Mobile", "Max length 32");

            yield break;
        }

        public override string ToXml()
        {
            if (IsValid)
            {
                var sb = new StringBuilder();
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
                {
                    w.WriteStartElement("recipient");
                    if (!string.IsNullOrEmpty(CompanyName)) w.WriteElementString("company_to", CompanyName);
                    if (!string.IsNullOrEmpty(PersonName)) w.WriteElementString("name_to", PersonName);
                    if (!string.IsNullOrEmpty(Phone)) w.WriteElementString("telephone_to", Phone);
                    if (!string.IsNullOrEmpty(Mobile)) w.WriteElementString("mobile_to", Mobile);
                    if (!string.IsNullOrEmpty(Email)) w.WriteElementString("email_to", Email);
                    if (!string.IsNullOrEmpty(Eori)) w.WriteElementString("eori", Eori);
                    if (!string.IsNullOrEmpty(Tin)) w.WriteElementString("tax_id", Tin);
                    if (!string.IsNullOrEmpty(Message)) w.WriteElementString("message_to", Message);

                }
                return sb.ToString();

            }
            throw new ArgumentException("Recipient element is not valid");
        }
    }
}
