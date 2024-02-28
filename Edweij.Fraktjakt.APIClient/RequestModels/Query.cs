using Edweij.Fraktjakt.APIClient.Structs;
using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class Query : XmlRequestObject
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
    public Query(Sender sender, ToAddress toAddress, FromAddress? fromAddress = null, IEnumerable<ShipmentItem>? items = null, IEnumerable<Parcel>? parcels = null) 
    { 
        Sender = sender ?? throw new ArgumentNullException(nameof(sender));
        ToAddress = toAddress ?? throw new ArgumentNullException(nameof(toAddress));

        if (!Sender.IsValid) throw new ArgumentException("Provided sender not valid");
        if (!ToAddress.IsValid) throw new ArgumentException("Provided toAddress not valid");

        if (fromAddress != null) FromAddress = fromAddress;
        if (items != null) Items = items.ToList();
        if (parcels != null) Parcels = parcels.ToList();            
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

    /// <summary>
    /// What is to be transported. If not specified, but there are parcel tags, and the integration has a default Commodity Template in Fraktjakt and it specified to be used in searches, the default Commodity Template is used to create content.
    /// A valid ShipmentQuery requires at least one parcel or one ShipmentItem
    /// </summary>
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
    private List<Parcel> parcels = new();

    /// <summary>
    /// The parcels that will be sent. Can be specified if commodities are not known, or if you already have decided how big the packages should be and do not want to use Fraktjakt’s package calculations.
    /// A valid ShipmentQuery requires at least one parcel or one ShipmentItem
    /// </summary>
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

    /// <summary>
    /// Add a ShipmentItem to this ShipmentQuey, an invalid ShipmentItem will not be added
    /// </summary>
    /// <param name="item">The ShipmentItem to add</param>
    public void AddShipmentItem(ShipmentItem item)
    {
        if (item != null && item.IsValid) items.Add(item);
    }

    /// <summary>
    /// Add a Parcel to this ShipmentQuey, an invalid parcel will not be added
    /// </summary>
    /// <param name="item">The ShipmentItem to add</param>
    public void AddParcel(Parcel parcel)
    {
        if (parcel != null && parcel.IsValid) parcels.Add(parcel);
    }


    /// <summary>
    /// The URL of the server that will receive the call from Fraktjakt's webhook.
    /// </summary>
    public string? CallbackUrl { get; set; }
    /// <summary>
    /// Should the shipping be insured
    /// Default is false
    /// </summary>
    public bool InsureDefault { get; set; } = false;
    /// <summary>
    /// Value of all items in the shipment, use this field if the value isn't specified in commodities element
    /// </summary>
    public float? Value { get; set; }
    /// <summary>
    /// ISO 4217, the currency for the value tag
    /// Default is SEK
    /// </summary>
    public CurrencyCode Currency { get; set; } = "SEK";
    /// <summary>
    /// Use if specify sorting instead of the integration setting    
    /// Default is true
    /// </summary>
    public bool PriceSort { get; set; } = true;
    /// <summary>
    /// Set to true if the result should only return express options
    /// Default is false
    /// </summary>
    public bool Express { get; set; } = false;
    /// <summary>
    /// Set to true if the result should only return freights with pickup
    /// Default is false
    /// </summary>
    public bool Pickup { get; set; } = false;
    /// <summary>
    /// Set to true if the result should only return freights with dropoff/home delivery
    /// Default is false
    /// </summary>
    public bool Dropoff { get; set; } = false;
    /// <summary>
    /// Set to true if the result should only return freights with environmental labelling
    /// Default is false
    /// </summary>
    public bool Green { get; set; } = false;
    /// <summary>
    /// Set to true if the result should only return freights with quality label
    /// Default is false
    /// </summary>
    public bool Quality { get; set; } = false;
    /// <summary>
    /// Set to true if the result should only return freights with delivery time guarantee
    /// Default is false
    /// </summary>
    public bool TimeGuarantee { get; set; } = false;
    /// <summary>
    /// Does the freight contains refrigerated goods
    /// Default is false
    /// </summary>
    public bool ColdContent { get; set; } = false;
    /// <summary>
    /// Does the freight contains frozen goods
    /// Default is false
    /// </summary>
    public bool FrozenContent { get; set; } = false;
    /// <summary>
    /// Use to specify a single product
    /// </summary>
    public int? ShippingProductId { get; set; } = null;
    /// <summary>
    /// Set to true to exclude postal agents from the result, use for faster queries
    /// Default is false
    /// </summary>
    public bool NoAgents { get; set; } = false;
    /// <summary>
    /// Set to true to exclude freight price from the result, use for faster queries
    /// Default is false
    /// </summary>
    public bool NoPrices { get; set; } = false;
    /// <summary>
    /// Set to true to include information about closest agent for submission, will result in slower queries
    /// Default is false
    /// </summary>
    public bool AgentsIn { get; set; } = false;
    /// <summary>
    /// Set to true to include id, name and logourl for shipping options in the result
    /// Default is false
    /// </summary>
    public bool ShipperInfo { get; set; } = false;
            
    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
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

        if (Value.HasValue)
        {
            if (Value.Value < 0f) yield return new RuleViolation("Value", "Don't use a negative value for Value");
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
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("shipment");
                if (!string.IsNullOrEmpty(CallbackUrl))
                {
                    w.WriteElementString("callback_url", CallbackUrl);
                }
                if (Value.HasValue)
                {
                    w.WriteElementString("value", Value.Value.ToStringPeriodDecimalSeparator());
                    w.WriteElementString("currency", Currency.ToString());
                }

                if (ShippingProductId.HasValue)
                {
                    w.WriteElementString("shipping_product_id", ShippingProductId.Value.ToString());
                }

                w.WriteRaw(Sender.ToXml());

                // Should we include elements with default value or not? Best practices?
                w.WriteElementString("insure_default", InsureDefault ? "1" : "0");
                w.WriteElementString("price_sort", PriceSort ? "1" : "0");
                w.WriteElementString("express", Express ? "1" : "0");
                w.WriteElementString("pickup", Pickup ? "1" : "0");
                w.WriteElementString("dropoff", Dropoff ? "1" : "0");
                w.WriteElementString("green", Green ? "1" : "0");
                w.WriteElementString("quality", Quality ? "1" : "0");
                w.WriteElementString("time_guarantee", TimeGuarantee ? "1" : "0");
                w.WriteElementString("cold", ColdContent ? "1" : "0");
                w.WriteElementString("frozen", FrozenContent ? "1" : "0");
                w.WriteElementString("no_agents", NoAgents ? "1" : "0");
                w.WriteElementString("no_prices", NoPrices ? "1" : "0");
                w.WriteElementString("agents_in", AgentsIn ? "1" : "0");
                w.WriteElementString("shipper_info", ShipperInfo ? "1" : "0");

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

                if (FromAddress != null) w.WriteRaw(FromAddress.ToXml());
                w.WriteRaw(ToAddress.ToXml());
            }
            return sb.ToString();                
        }
        throw new ArgumentException("Shipment element is not valid");
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Query other = (Query)obj;

        return Equals(Sender, other.Sender) &&
               Equals(ToAddress, other.ToAddress) &&
               Equals(FromAddress, other.FromAddress) &&
               Enumerable.SequenceEqual(Items, other.Items) &&
               Enumerable.SequenceEqual(Parcels, other.Parcels) &&
               CallbackUrl == other.CallbackUrl &&
               InsureDefault == other.InsureDefault &&
               Value == other.Value &&
               Equals(Currency, other.Currency) &&
               PriceSort == other.PriceSort &&
               Express == other.Express &&
               Pickup == other.Pickup &&
               Dropoff == other.Dropoff &&
               Green == other.Green &&
               Quality == other.Quality &&
               TimeGuarantee == other.TimeGuarantee &&
               ColdContent == other.ColdContent &&
               FrozenContent == other.FrozenContent &&
               ShippingProductId == other.ShippingProductId &&
               NoAgents == other.NoAgents &&
               NoPrices == other.NoPrices &&
               AgentsIn == other.AgentsIn &&
               ShipperInfo == other.ShipperInfo;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Sender?.GetHashCode() ?? 0;
            hash = hash * 23 + ToAddress?.GetHashCode() ?? 0;
            hash = hash * 23 + FromAddress?.GetHashCode() ?? 0;
            hash = hash * 23 + (Items?.GetHashCode() ?? 0);
            hash = hash * 23 + (Parcels?.GetHashCode() ?? 0);
            hash = hash * 23 + (CallbackUrl?.GetHashCode() ?? 0);
            hash = hash * 23 + InsureDefault.GetHashCode();
            hash = hash * 23 + Value?.GetHashCode() ?? 0;
            hash = hash * 23 + Currency.GetHashCode();
            hash = hash * 23 + PriceSort.GetHashCode();
            hash = hash * 23 + Express.GetHashCode();
            hash = hash * 23 + Pickup.GetHashCode();
            hash = hash * 23 + Dropoff.GetHashCode();
            hash = hash * 23 + Green.GetHashCode();
            hash = hash * 23 + Quality.GetHashCode();
            hash = hash * 23 + TimeGuarantee.GetHashCode();
            hash = hash * 23 + ColdContent.GetHashCode();
            hash = hash * 23 + FrozenContent.GetHashCode();
            hash = hash * 23 + ShippingProductId?.GetHashCode() ?? 0;
            hash = hash * 23 + NoAgents.GetHashCode();
            hash = hash * 23 + NoPrices.GetHashCode();
            hash = hash * 23 + AgentsIn.GetHashCode();
            hash = hash * 23 + ShipperInfo.GetHashCode();
            return hash;
        }
    }
}
