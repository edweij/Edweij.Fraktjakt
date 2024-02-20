using System.Net.Mail;
using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public abstract class Party : XmlRequestObject
    {        

        public string? CompanyName { get; set; } = null;
        public string? PersonName { get; set; } = null;
        public string? Phone { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Eori { get; set; } = null;
        public string? Tin { get; set; } = null;

        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (!string.IsNullOrEmpty(CompanyName) && CompanyName.Length > 32) yield return new RuleViolation("CompanyName", "Max length 32");
            if (!string.IsNullOrEmpty(PersonName) && PersonName.Length > 32) yield return new RuleViolation("PersonName", "Max length 32");
            if (!string.IsNullOrEmpty(Phone) && Phone.Length > 32) yield return new RuleViolation("Phone", "Max length 32");
            if (!string.IsNullOrEmpty(Email))
            {
                if (Email.Length > 64) yield return new RuleViolation("Email", "Max length 64");
                if (!Email.IsValidEmailAddress()) yield return new RuleViolation("Email", "Not valid");
            }
            if (!string.IsNullOrEmpty(Eori) && Eori.Length > 32) yield return new RuleViolation("Eori", "Max length 32");
            if (!string.IsNullOrEmpty(Tin) && Tin.Length > 18) yield return new RuleViolation("Tin", "Max length 18");
            yield break;
        }

        
    }

    public class Dispatcher : Party
    {
        public string? Rex { get; set; } = null;

        public string? Voec { get; set; } = null;
        public string? GbVat { get; set; } = null;
        public string? Ioss { get; set; } = null;
        

        public override IEnumerable<RuleViolation> GetRuleViolations()
        {
            foreach (var err in base.GetRuleViolations())
            {
                yield return err;
            }
            if (!string.IsNullOrEmpty(Rex) && Rex.Length > 32) yield return new RuleViolation("Rex", "Max length 32");
            if (!string.IsNullOrEmpty(Voec) && Voec.Length > 32) yield return new RuleViolation("Voec", "Max length 32");
            if (!string.IsNullOrEmpty(GbVat) && GbVat.Length > 10) yield return new RuleViolation("GbVat", "Max length 10");
            if (!string.IsNullOrEmpty(Ioss) && Ioss.Length > 32) yield return new RuleViolation("Ioss", "Max length 32");
            yield break;
        }

        public override string ToXml()
        {
            if (IsValid)
            {
                var sb = new StringBuilder();
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
                {
                    w.WriteStartElement("sender");
                    if (!string.IsNullOrEmpty(CompanyName)) w.WriteElementString("company_from", CompanyName);
                    if (!string.IsNullOrEmpty(PersonName)) w.WriteElementString("name_from", PersonName);
                    if (!string.IsNullOrEmpty(Phone)) w.WriteElementString("telephone_from", Phone);
                    if (!string.IsNullOrEmpty(Email)) w.WriteElementString("email_from", Email);
                    if (!string.IsNullOrEmpty(Eori)) w.WriteElementString("eori", Eori);
                    if (!string.IsNullOrEmpty(Rex)) w.WriteElementString("rex", Rex);
                    if (!string.IsNullOrEmpty(Voec)) w.WriteElementString("voec", Voec);
                    if (!string.IsNullOrEmpty(GbVat)) w.WriteElementString("gb_vat", GbVat);
                    if (!string.IsNullOrEmpty(Ioss)) w.WriteElementString("ioss", Ioss);
                    if (!string.IsNullOrEmpty(Tin)) w.WriteElementString("tax_id", Tin);                    
                }                   
                return sb.ToString();
                
            }
            throw new ArgumentException("Dispatcher element is not valid");
        }
    }

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
