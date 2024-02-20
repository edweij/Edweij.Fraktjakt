using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient
{
    public abstract class Order : XmlRequestObject
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
                if (referredSender == null) throw new ArgumentNullException(nameof(value));
                if (referredSender != null && !value!.IsValid) throw new ArgumentException("ReferredSender not valid");
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
                Regex r = new Regex("^[ a-zA-Z0-9]*$");
                if (!r.IsMatch(Reference)) yield return new RuleViolation("Reference", "May only contain space, 0-9 and a-z or A-Z");
            }

            if (Dispatcher != null && !Dispatcher.IsValid)
            {
                foreach (var err in Dispatcher.GetRuleViolations())
                {
                    yield return err;
                }
            }

            if (Recipient != null && !Recipient.IsValid)
            {
                foreach (var err in Recipient.GetRuleViolations())
                {
                    yield return err;
                }
            }

            if (!string.IsNullOrEmpty(SenderEmail) && !SenderEmail.IsValidEmailAddress())
            {
                yield return new RuleViolation("SenderEmail", "Must be a valid email address");
            }

            if (PickupInfo != null && !PickupInfo.IsValid)
            {
                foreach (var err in PickupInfo.GetRuleViolations())
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
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
                {
                    w.WriteStartElement("OrderSpecification");
                    if (Value.HasValue)
                    {
                        w.WriteElementString("value", Value.Value.ToStringPeriodDecimalSeparator());
                    }
                    w.WriteRaw(Sender.ToXml());
                    if (ReferredSender != null)
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
                    if (Dispatcher != null)
                    {
                        w.WriteRaw(Dispatcher.ToXml());
                    }
                    if (Recipient != null)
                    {
                        w.WriteRaw(Recipient.ToXml());
                    }

                    if (PickupInfo != null)
                    {
                        w.WriteRaw(PickupInfo.ToXml());
                    }

                    if (!string.IsNullOrEmpty(SenderEmail))
                    {
                        w.WriteElementString("sender_email", SenderEmail);
                    }
                }
                return sb.ToString();
            }
            throw new ArgumentException("Order element is not valid");
        }
    }

    /// <summary>
    /// Anropstyp 2 - Skapa en order direkt utan att tidigare ha skapat en sändning
    /// </summary>
    public class DirectOrder : Order
    {
        public DirectOrder(Sender sender, ToAddress toAddress, int shippingProductId, IEnumerable<ShipmentItem>? items = null, FromAddress? fromAddress = null, IEnumerable<Parcel>? parcels = null) : base(sender, shippingProductId, items)
        {
            if (toAddress == null) throw new ArgumentNullException(nameof(toAddress));
            if (toAddress != null && !toAddress.IsValid) throw new ArgumentException(nameof(toAddress));
            ToAddress = toAddress!;
            if (fromAddress != null)
            {
                if (!fromAddress!.IsValid) throw new ArgumentException("fromAddress not valid");
                FromAddress = fromAddress;
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
                if (value == null) throw new ArgumentNullException("FromAddress");
                if (!value.IsValid) throw new ArgumentException("FromAddress not valid");
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
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
                {
                    xml.WriteTo(w);
                }
                return sb.ToString();
            }
            throw new ArgumentException("Order element is not valid");
        }
    }

    /// <summary>
    /// Anropstyp 1 - Skapa en order från en tidigare skapat sändning
    /// </summary>
    public class OrderFromQuery : Order
    {
        public OrderFromQuery(Sender sender, int shippingProductId, int shipmentId, IEnumerable<ShipmentItem>? items = null) : base(sender, shippingProductId, items)
        {
            if (shipmentId < 1) throw new ArgumentException(nameof(shipmentId));
            ShipmentId = shipmentId;
        }

        public int ShipmentId { get; init; }

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
                var xml = XElement.Parse(base.ToXml());
                xml.Add(new XElement("shipment_id", ShipmentId));
                var sb = new StringBuilder();
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
                {
                    xml.WriteTo(w);
                }
                return sb.ToString();
            }
            throw new ArgumentException("Order element is not valid");
        }
    }
}
