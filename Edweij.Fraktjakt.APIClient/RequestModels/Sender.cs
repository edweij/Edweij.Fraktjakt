using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class Sender : ReferredSender
{
    public Sender(int id, string key) : base(id, key)
    {
         
    }
    
    public CurrencyCode Currency { get; set; } = "SEK";
    public Language6391 Language { get; set; } = Language6391.sv;

    public string? SystemName { get; set; } = null;
    public string? SystemVersion { get; set; } = null;
    public string? ModuleVersion { get; set; } = null;
    public float ApiVersion { get; private set; } = 4.5f; // Current version 2024-01-25, implement version handling later
    
    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        foreach (var err in base.GetRuleViolations())
        {
            yield return err;
        }
        yield break;
    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();                
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("consignor");
                w.WriteElementString("id", Id.ToString());
                w.WriteElementString("key", Key);
                w.WriteElementString("currency", Currency.ToString());
                w.WriteElementString("language", Language.ToString());
                w.WriteElementString("encoding", "UTF-8");// this client only use utf-8 although the API also supports ISO-8859-1
                if (!string.IsNullOrEmpty(SystemName)) w.WriteElementString("system_name", SystemName);
                if (!string.IsNullOrEmpty(SystemVersion)) w.WriteElementString("system_version", SystemVersion);
                if (!string.IsNullOrEmpty(ModuleVersion)) w.WriteElementString("module_version", ModuleVersion);
                w.WriteElementString("api_version", ApiVersion.ToStringPeriodDecimalSeparator());
            }
            return sb.ToString();
        }
        throw new ArgumentException("Sender element is not valid");
    }
}
