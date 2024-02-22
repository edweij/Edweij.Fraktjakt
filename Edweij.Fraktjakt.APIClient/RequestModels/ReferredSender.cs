using System.Text;
using System.Xml;

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

    public int Id { get; private set; }
    public string? Key { get; private set; } = null;

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (string.IsNullOrWhiteSpace(Key)) yield return new RuleViolation("Key", "Key is required");
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
