using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

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
            using (var w = CreateXmlWriter(sb))
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
