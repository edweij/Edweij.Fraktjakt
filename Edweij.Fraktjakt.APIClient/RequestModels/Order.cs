using Edweij.Fraktjakt.APIClient.Enums;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public abstract partial class Order : XmlRequestObject
{
    public Order(Sender sender, int shippingProductId, IEnumerable<ShipmentItem>? items = null)
    {
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        if (!Sender.IsValid) throw new ArgumentException("Provided sender not valid");
        if (shippingProductId < 1) throw new ArgumentException("Provided shippingProductId not valid");
        ShippingProductId = shippingProductId;
        if (items != null)
        {
            if (items.Any(i => !i.IsValid)) throw new ArgumentException("Items contain invalid item");
            Items = items.ToList();
        }
    }

    public Sender Sender { get; init; }

    private ReferredSender? referredSender = null;
    public ReferredSender? ReferredSender
    {
        get { return referredSender; }
        set
        {
            if (!value?.IsValid ?? false) throw new ArgumentException("ReferredSender not valid");
            referredSender = value;
        }
    }

    private List<ShipmentItem> items = new();
    public IEnumerable<ShipmentItem> Items
    {
        get { return items; }
        set
        {
            if (value == null) throw new ArgumentNullException("Items");
            if (value.Any(i => !i.IsValid)) throw new ArgumentException("Items not valid");
            items = value.ToList();
        }
    }

    public float? Value { get; set; } = null;

    /// <summary>
    /// URL to your own webhook, if used replaces the webhook url in the integation settings
    /// </summary>
    public string? CallbackUrl { get; set; }
    /// <summary>
    /// Should the shipping be insured, if used replaces the integration settings
    /// </summary>
    public bool InsureDefault { get; set; } = false;
    public int ShippingProductId { get; init; }
    public int? AgentId { get; set; } = null;
    public string? Reference { get; set; } = null;
    public ExportReason ExportReason { get; set; } = ExportReason.SALE;
    /// <summary>
    /// Set to true to exclude postal agents from the result, use for faster queries
    /// </summary>
    public bool NoAgents { get; set; } = false;
    public Dispatcher? Dispatcher { get; set; } = null;
    public Recipient? Recipient { get; set; } = null;
    public string? SenderEmail { get; set; } = null;
    public PickupInfo? PickupInfo { get; set; } = null;


    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (!string.IsNullOrEmpty(Reference))
        {
            if (Reference.Length > 50) yield return new RuleViolation("Reference", "Max length 50");
            Regex r = ValidReferenceRegex();
            if (!r.IsMatch(Reference)) yield return new RuleViolation("Reference", "May only contain space, 0-9 and a-z or A-Z");
        }

        if (!Dispatcher?.IsValid ?? false)
        {
            foreach (var err in Dispatcher!.GetRuleViolations())
            {
                yield return err;
            }
        }

        if (!Recipient?.IsValid ?? false)
        {
            foreach (var err in Recipient!.GetRuleViolations())
            {
                yield return err;
            }
        }

        if (!string.IsNullOrEmpty(SenderEmail) && !SenderEmail.IsValidEmailAddress())
        {
            yield return new RuleViolation("SenderEmail", "Must be a valid email address");
        }

        if (!PickupInfo?.IsValid ?? false)
        {
            foreach (var err in PickupInfo!.GetRuleViolations())
            {
                yield return err;
            }
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
                w.WriteStartElement("OrderSpecification");
                if (Value.HasValue)
                {
                    w.WriteElementString("value", Value.Value.ToStringPeriodDecimalSeparator());
                }
                w.WriteRaw(Sender.ToXml());
                if (ReferredSender is not null)
                {
                    w.WriteRaw(ReferredSender.ToXml());
                }

                if (!string.IsNullOrEmpty(CallbackUrl))
                {
                    w.WriteElementString("callback_url", CallbackUrl);
                }
                w.WriteElementString("insure_default", InsureDefault ? "1" : "0");
                w.WriteElementString("shipping_product_id", ShippingProductId.ToString());

                if (AgentId.HasValue)
                {
                    w.WriteElementString("agent_id", AgentId.HasValue.ToString());
                }

                if (!string.IsNullOrEmpty(Reference))
                {
                    w.WriteElementString("reference", Reference);
                }

                w.WriteElementString("export_reason", ExportReason.ToString());
                w.WriteElementString("no_agents", NoAgents ? "1" : "0");

                if (Items != null && Items.Any())
                {
                    w.WriteStartElement("commodities");
                    foreach (var item in Items)
                    {
                        w.WriteRaw(item.ToXml());
                    }
                    w.WriteEndElement();
                }
                if (Dispatcher is not null) w.WriteRaw(Dispatcher.ToXml());
                
                if (Recipient is not null) w.WriteRaw(Recipient.ToXml());
                
                if (PickupInfo is not null) w.WriteRaw(PickupInfo.ToXml());
                
                if (!string.IsNullOrEmpty(SenderEmail)) w.WriteElementString("sender_email", SenderEmail);
                
            }
            return sb.ToString();
        }
        throw new ArgumentException("Order element is not valid");
    }

    [GeneratedRegex("^[ a-zA-Z0-9]*$")]
    private static partial Regex ValidReferenceRegex();
}




