using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class ReferredSender : XmlRequestObject
{
    public ReferredSender(int id, string key)
    {
        if (id < 1) throw new ArgumentException("Id must be a positive integer");
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key can't be null or whitespace");

        Id = id;
        Key = key;
    }

    /// <summary>
    /// Integrations id in Fraktjakt.
    /// Required
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// The consignor's login key in Fraktjakt.
    /// Required
    /// </summary>
    public string Key { get; private set; }

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        yield break;
    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("<referred_consignor>");
                w.WriteElementString("id", Id.ToString() ?? "");
                w.WriteElementString("key", Key!);
            }
            return sb.ToString();
        }
        throw new ArgumentException("ReferredSender element is not valid");
    }
}
