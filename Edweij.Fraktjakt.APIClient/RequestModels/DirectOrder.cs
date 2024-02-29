using System.Text;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

/// <summary>
/// Anropstyp 2 - Skapa en order direkt utan att tidigare ha skapat en sändning
/// </summary>
public class DirectOrder : Order
{
    public DirectOrder(Sender sender, ToAddress toAddress, int shippingProductId, IEnumerable<ShipmentItem>? items = null, FromAddress? fromAddress = null, IEnumerable<Parcel>? parcels = null) : base(sender, shippingProductId, items)
    {
        if (!toAddress.IsValid) throw new ArgumentException("toAddress not valid");
        ToAddress = toAddress!;

        if (!fromAddress?.IsValid ?? false) throw new ArgumentException("fromAddress not valid");
        FromAddress = fromAddress;

        if (items != null)
        {
            if (items.Any(i => !i.IsValid)) throw new ArgumentException("items not valid");
            Items = items.ToList();
        }

        if (parcels != null)
        {
            if (parcels.Any(p => !p.IsValid)) throw new ArgumentException("parcels not valid");
            Parcels = parcels.ToList();
        }
    }

    public ToAddress ToAddress { get; init; }

    private FromAddress? fromAddress = null;
    public FromAddress? FromAddress
    {
        get { return fromAddress; }

        set
        {
            if (!value?.IsValid ?? false) throw new ArgumentException("FromAddress not valid");
            fromAddress = value;
        }
    }

    private List<Parcel> parcels = new();
    public IEnumerable<Parcel> Parcels
    {
        get { return parcels; }
        set
        {
            if (value == null) throw new ArgumentNullException("Parcels");
            if (value.Any(p => !p.IsValid)) throw new ArgumentException("Parcels not valid");
            parcels = value.ToList();
        }
    }


    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        foreach (var err in base.GetRuleViolations())
        {
            yield return err;
        }

        if (!ToAddress.IsValid)
        {
            foreach (var err in ToAddress.GetRuleViolations())
            {
                yield return err;
            }
        }

        if (fromAddress != null && !fromAddress.IsValid)
        {
            foreach (var err in fromAddress.GetRuleViolations())
            {
                yield return err;
            }
        }

        if (parcels != null && parcels.Any(p => !p.IsValid))
        {
            foreach (var p in parcels.Where(p => !p.IsValid))
            {
                foreach (var err in p.GetRuleViolations())
                {
                    yield return err;
                }
            }
        }

        yield break;
    }

    public override string ToXml()
    {
        if (IsValid)
        {
            var xml = XElement.Parse(base.ToXml());
            xml.Add(XElement.Parse(ToAddress.ToXml()));
            if (fromAddress != null)
            {
                xml.Add(XElement.Parse(fromAddress.ToXml()));
            }

            if (parcels != null && parcels.Any())
            {
                var parcelsElement = new XElement("parcels", parcels.Select(p => XElement.Parse(p.ToXml())));
                xml.Add(parcelsElement);
            }
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
            {
                xml.WriteTo(w);
            }
            return sb.ToString();
        }
        throw new ArgumentException("Order element is not valid");
    }
}
