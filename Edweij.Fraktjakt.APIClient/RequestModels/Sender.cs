using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Text;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class Sender : ReferredSender
{
    public Sender(int id, string key) : base(id, key)
    {
         
    }

    /// <summary>
    /// Selected currency, Fraktjakt will return the price results in the specified currency. 
    /// Used when an integration wants to show prices in other than SEK (however, the freight will be paid in SEK in Fraktjakt).
    /// Default is SEK
    /// </summary>
    public CurrencyCode Currency { get; set; } = "SEK";

    /// <summary>
    /// Sets language for the web store's administrator / user.Warnings and Errors are returned in that language.
    /// Default is sv (swedish)
    /// </summary>
    public SwedishOrEnglish Language { get; set; } = "sv";

    /// <summary>
    /// Name of system calling the API.
    /// If the using DI helper AddFraktjaktClient with SenderId and SenderKey is used, this will insert this assemlys name and version
    /// </summary>
    public string? SystemName { get; set; } = null;

    /// <summary>
    /// Version number of SystemName.
    /// If the using DI helper AddFraktjaktClient with SenderId and SenderKey is used, this will insert this assemlys name and version
    /// </summary>
    public string? SystemVersion { get; set; } = null;

    /// <summary>
    /// Version number of the Fraktjakt shipping module in use.
    /// </summary>
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
