using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public class CreateShipment : XmlRequestObject
    {
        public Sender Sender { get; init; }
        public ToAddress ToAddress { get; init; }
        

        /// <summary>
        /// Create a shipment to query available freights for your integration
        /// </summary>
        /// <param name="sender">Required parameter with your integration id, API key and other settings</param>
        /// <param name="toAddress">Required parameter with the receivers address</param>
        /// <param name="fromAddress">Optional from address, if not provided uses from address in integration settings</param>
        /// <param name="items">Optional parameter with shipment items. A shipment should contain at least one shipment item or parcel</param>
        /// <param name="parcels">Optional parameter with parcels. A shipment should contain at least one parcel or shipment item</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CreateShipment(Sender sender, ToAddress toAddress, FromAddress? fromAddress = null, IEnumerable<ShipmentItem>? items = null, IEnumerable<Parcel>? parcels = null) 
        { 
            Sender = sender ?? throw new ArgumentNullException(nameof(sender));
            ToAddress = toAddress ?? throw new ArgumentNullException(nameof(toAddress));

            if (!Sender.IsValid) throw new ArgumentException("Provided sender not valid");
            if (!ToAddress.IsValid) throw new ArgumentException("Provided toAddress not valid");

            if (fromAddress != null) FromAddress = fromAddress;
            if (items != null) Items = items.ToList();
            if (parcels != null) Parcels = parcels.ToList();            
        }

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
        private List<Parcel> parcels = new List<Parcel>();
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

        public void AddShipmentItem(ShipmentItem item)
        {
            if (item != null && item.IsValid) items.Add(item);
        }

        public void AddParcel(Parcel parcel)
        {
            if (parcel != null && parcel.IsValid) parcels.Add(parcel);
        }

        /// <summary>
        /// Use to specify a favorite shipping service
        /// </summary>
        public int? ShippingProductId { get; set; } = null;

        /// <summary>
        /// Use to specify a service point
        /// </summary>
        public int? AgentId { get; set; } = null;

        /// <summary>
        /// URL to your own webhook, if used replaces the webhook url in the integation settings
        /// </summary>
        public string? CallbackUrl { get; set; }
        /// <summary>
        /// Should the shipping be insured, if used replaces the integration settings
        /// </summary>
        public bool InsureDefault { get; set; } = false;
        /// <summary>
        /// String to identify shipment, e.g. your own order id
        /// </summary>
        public string? Reference { get; set; } = null;
        /// <summary>
        /// Use if specify sorting instead of the integration setting
        /// </summary>
        public bool PriceSort { get; set; } = true;
        public ExportReason ExportReason { get; set; } = ExportReason.SALE;

        public Dispatcher? Dispatcher { get; set; } = null;
        public Recipient? Recipient { get; set; } = null;
        public string? SenderEmail { get; set; } = null;


        
                
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

            if (!Sender.IsValid)
            {
                yield return new RuleViolation("Sender", "Sender is not valid");
                foreach (var err in Sender.GetRuleViolations()) yield return err;
            }

            if (!ToAddress.IsValid)
            {
                yield return new RuleViolation("ToAddress", "ToAddress is not valid");
                foreach (var err in ToAddress.GetRuleViolations()) yield return err;
            }

            if (FromAddress != null && !FromAddress.IsValid)
            {
                yield return new RuleViolation("FromAddress", "FromAddress is not valid");
                foreach (var err in FromAddress.GetRuleViolations()) yield return err;
            }

            if (!Items.Any() && !Parcels.Any())
            {
                yield return new RuleViolation("Items", "Provide either a shipment item or a parcel");
                yield return new RuleViolation("Parcels", "Provide either a shipment item or a parcel");
            }

            if (Items.Any(i => !i.IsValid))
            {
                yield return new RuleViolation("Items", "Items contain an invalid item");
                foreach (var item in Items)
                {
                    foreach (var err in item.GetRuleViolations()) yield return err;
                }
            }

            if (Parcels.Any(p => !p.IsValid))
            {
                yield return new RuleViolation("Parcels", "Parcels contain an invalid parcel");
                foreach (var parcel in Parcels)
                {
                    foreach (var err in parcel.GetRuleViolations()) yield return err;
                }
            }


            if (!string.IsNullOrEmpty(CallbackUrl))
            {
                if (string.IsNullOrWhiteSpace(CallbackUrl)) yield return new RuleViolation("CallbackUrl", "If a callbackurl is provided it should not be whitespace only");
            }

            

            if (ShippingProductId.HasValue)
            {
                if (ShippingProductId.Value < 1) yield return new RuleViolation("ShippingProductId", "Don't use a negative value for ShippingProductId");
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
                    w.WriteStartElement("CreateShipment");
                    w.WriteRaw(Sender.ToXml());
                    if (ReferredSender != null)
                    {
                        w.WriteRaw(ReferredSender.ToXml());
                    }
                    if (ShippingProductId.HasValue)
                    {
                        w.WriteElementString("shipping_product_id", ShippingProductId.Value.ToString());
                    }
                    if (AgentId.HasValue)
                    {
                        w.WriteElementString("agent_id", AgentId.Value.ToString());
                    }
                    if (!string.IsNullOrEmpty(CallbackUrl))
                    {
                        w.WriteElementString("callback_url", CallbackUrl);
                    }
                    w.WriteElementString("insure_default", InsureDefault ? "1" : "0");
                    if (!string.IsNullOrEmpty(Reference))
                    {
                        w.WriteElementString("reference", Reference);
                    }
                    w.WriteElementString("price_sort", PriceSort ? "1" : "0");
                    if (FromAddress != null) w.WriteRaw(FromAddress.ToXml());
                    w.WriteRaw(ToAddress.ToXml());

                    w.WriteElementString("export_reason", ExportReason.ToString());
                    if (!string.IsNullOrEmpty(SenderEmail))
                    {
                        w.WriteElementString("sender_email", SenderEmail);
                    }
                    if (Dispatcher != null)
                    {
                        w.WriteRaw(Dispatcher.ToXml());
                    }
                    if (Recipient != null)
                    {
                        w.WriteRaw(Recipient.ToXml());
                    }
                    if (Items != null && Items.Any())
                    {
                        w.WriteStartElement("commodities");
                        foreach (var item in Items)
                        {
                            w.WriteRaw(item.ToXml());
                        }
                        w.WriteEndElement();
                    }

                    if (Parcels != null && Parcels.Any())
                    {
                        w.WriteStartElement("parcels");
                        foreach (var p in Parcels)
                        {
                            w.WriteRaw(p.ToXml());
                        }
                        w.WriteEndElement();
                    }
                }
                return sb.ToString();                
            }
            throw new ArgumentException("Shipment element is not valid");
        }
    }
}
