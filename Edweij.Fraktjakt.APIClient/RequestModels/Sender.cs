using Edweij.Fraktjakt.APIClient.Structs;
using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

/// <summary>
/// Information about the sender, i.e. the integration that is sending the order
/// </summary>
public class Sender : ReferredSender
{
    /// <summary>
    /// Information about the sender, i.e. the integration that is sending the order
    /// </summary>
    /// <param name="id">The integrations customer id in Fraktjakt.</param>
    /// <param name="key">The integrations api key in Fraktjakt.</param>
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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Sender other = (Sender)obj;

        return base.Equals(obj) &&
               Equals(Currency, other.Currency) &&
               Equals(Language, other.Language) &&
               SystemName == other.SystemName &&
               SystemVersion == other.SystemVersion &&
               ModuleVersion == other.ModuleVersion &&
               ApiVersion == other.ApiVersion;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = base.GetHashCode();
            hash = hash * 23 + Currency.GetHashCode();
            hash = hash * 23 + Language.GetHashCode();
            hash = hash * 23 + (SystemName?.GetHashCode() ?? 0);
            hash = hash * 23 + (SystemVersion?.GetHashCode() ?? 0);
            hash = hash * 23 + (ModuleVersion?.GetHashCode() ?? 0);
            hash = hash * 23 + ApiVersion.GetHashCode();
            return hash;
        }
    }
}
