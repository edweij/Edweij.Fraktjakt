using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class ShipmentItem : XmlRequestObject
{
    public ShipmentItem(string name, int quantity, float unitPrice, float totalWeight) 
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name can not be null or whitespace only");
        if (name.Length > 64) throw new ArgumentException("Name is too long (max 64 characters)");
        Name = name;

        if (quantity < 1) throw new ArgumentException("Quantity should be greater than 0");
        Quantity = quantity;

        if (unitPrice < 0) throw new ArgumentException("UnitPrice can't be negative");
        UnitPrice = unitPrice;

        if (totalWeight < 0) throw new ArgumentException("TotalWeight can't be negative");
        TotalWeight = totalWeight;
    }

    public string Name { get; init; }
    public int Quantity { get; init; }
    public float UnitPrice { get; init; }
    /// <summary>
    /// Total weight of this items units in kilograms
    /// </summary>
    public float TotalWeight { get; init; }

    public bool Shipped { get; set; } = true;
    public int? Taric { get; set; }
    public QuantityUnit QuantityUnit { get; set; } = QuantityUnit.EA;
    public string? Description { get; set; }
    public CountryCode CountryOfManufacture { get; set; } = "SE";
   
    /// <summary>
    /// Unit length in centimeters
    /// </summary>
    public float? UnitLength { get; set; }
    /// <summary>
    /// Unit width in centimeters
    /// </summary>
    public float? UnitWidth { get; set; }
    /// <summary>
    /// Unit height in centimeters
    /// </summary>
    public float? UnitHeight { get; set; }
    
    public CurrencyCode Currency { get; set; } = "SEK";
    public bool InOwnParcel { get; set; } = false;
    public string? ArticleNumber { get; set; }
    public string? ShelfPosition { get; set; }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb, XmlWriterSettings))
            {
                w.WriteStartElement("commodity");
                w.WriteElementString("name", Name);
                w.WriteElementString("quantity", Quantity.ToString());
                w.WriteElementString("shipped", Shipped ? "1" : "0");
                if (Taric.HasValue) w.WriteElementString("taric", Taric.Value.ToString());
                w.WriteElementString("quantity_units", QuantityUnit.ToString());
                if (!string.IsNullOrWhiteSpace(Description)) w.WriteElementString("description", Description);
                w.WriteElementString("country_of_manufacture", CountryOfManufacture.ToString());
                w.WriteElementString("weight", TotalWeight.ToStringPeriodDecimalSeparator());
                if (UnitLength.HasValue) w.WriteElementString("length", UnitLength!.Value.ToStringPeriodDecimalSeparator());
                if (UnitWidth.HasValue) w.WriteElementString("width", UnitWidth!.Value.ToStringPeriodDecimalSeparator());
                if (UnitHeight.HasValue) w.WriteElementString("height", UnitHeight!.Value.ToStringPeriodDecimalSeparator());
                w.WriteElementString("unit_price", UnitPrice.ToStringPeriodDecimalSeparator());
                w.WriteElementString("currency", Currency.ToString());
                w.WriteElementString("in_own_parcel", InOwnParcel ? "1" : "0");
                if (!string.IsNullOrWhiteSpace(ArticleNumber)) w.WriteElementString("article_number", ArticleNumber);
                if (!string.IsNullOrWhiteSpace(ShelfPosition)) w.WriteElementString("shelf_position", ShelfPosition);
            }                    
            
            return sb.ToString();
        }
        throw new ArgumentException("Shipment item element is not valid");
    }

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {   
        if (!string.IsNullOrWhiteSpace(Description) && (Description.Length < 15 || Description.Length > 128))
        {
            yield return new RuleViolation("Description", "Description is too short or too long (15-128 characters)");
        }          

        if (!string.IsNullOrWhiteSpace(ArticleNumber) && ArticleNumber.Length > 64)
        {
            yield return new RuleViolation("ArticleNumber", "ArticleNumber is too long (max 64 characters)");
        }

        if (!string.IsNullOrWhiteSpace(ShelfPosition) && ShelfPosition.Length > 64)
        {
            yield return new RuleViolation("ShelfPosition", "ShelfPosition is too long (max 64 characters)");
        }
    }
}
