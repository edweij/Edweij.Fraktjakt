using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class ShipmentItem : XmlRequestObject
{
    /// <summary>
    /// Create a shipmentitem with required properties as parameters
    /// </summary>
    /// <param name="name">The name of the goods</param>
    /// <param name="quantity">The quantity of the item in the unit specified in QuantityUnit property, defaults to EA (each)</param>
    /// <param name="unitPrice">The item value per unit of this item type. The value the item was sold for. If you sell with VAT, the VAT must be included in the value, and if you sell without VAT, it must not be included.</param>
    /// <param name="totalWeight">Total weight in kg of the type of goods in the shipment.</param>
    /// <exception cref="ArgumentException">For invalid parameters</exception>
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

    /// <summary>
    /// The name of the goods
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The quantity of the item in the unit specified in QuantityUnit property, defaults to EA (each)
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// The item value per unit of this item type. The value the item was sold for. If you sell with VAT, the VAT must be included in the value, and if you sell without VAT, it must not be included.
    /// </summary>
    public float UnitPrice { get; init; }

    /// <summary>
    /// Total weight in kg of the type of goods in the shipment.
    /// </summary>
    public float TotalWeight { get; init; }

    /// <summary>
    /// Indicates whether the item should be included in the shipping at all. If not, it is only used to calculate possible free shipping and the like.
    /// Default value is true
    /// </summary>
    public bool Shipped { get; set; } = true;

    /// <summary>
    /// Code of the goods category in customs.
    /// </summary>
    public int? Taric { get; set; }

    /// <summary>
    /// The unit by which the item is counted or measured.
    /// Default value is EA (each)
    /// </summary>
    public QuantityUnit QuantityUnit { get; set; } = QuantityUnit.EA;

    /// <summary>
    /// Description of the product type (minimum 15 and maximum 128 characters). Mandatory for shipping to countries other than Sweden.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Country code
    /// Default value is SE.
    /// </summary>
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

    /// <summary>
    /// Currency for value (SEK is default)
    /// Default value is SEK
    /// </summary>
    public CurrencyCode Currency { get; set; } = "SEK";

    /// <summary>
    /// Indicates whether the item has its own package and should not be placed in another package.
    /// Default value is false.
    /// </summary>
    public bool InOwnParcel { get; set; } = false;

    /// <summary>
    /// Item number, used in the packing list. Conveniently, the SKU (Stock Keeping Unit) is entered here if used.
    /// </summary>
    public string? ArticleNumber { get; set; }

    /// <summary>
    /// Where the item can be found when packing it.
    /// </summary>
    public string? ShelfPosition { get; set; }

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
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

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ShipmentItem other = (ShipmentItem)obj;

        // Compare the relevant properties for equality
        return Name == other.Name
            && Quantity == other.Quantity
            && UnitPrice == other.UnitPrice
            && TotalWeight == other.TotalWeight
            && Shipped == other.Shipped
            && Taric == other.Taric
            && QuantityUnit == other.QuantityUnit
            && Description == other.Description
            && CountryOfManufacture.Equals(other.CountryOfManufacture)
            && UnitLength == other.UnitLength
            && UnitWidth == other.UnitWidth
            && UnitHeight == other.UnitHeight
            && Currency.Equals(other.Currency)
            && InOwnParcel == other.InOwnParcel
            && ArticleNumber == other.ArticleNumber
            && ShelfPosition == other.ShelfPosition;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;

            // Combine hash codes for relevant properties
            hash = hash * 23 + (Name?.GetHashCode() ?? 0);
            hash = hash * 23 + Quantity.GetHashCode();
            hash = hash * 23 + UnitPrice.GetHashCode();
            hash = hash * 23 + TotalWeight.GetHashCode();
            hash = hash * 23 + Shipped.GetHashCode();
            hash = hash * 23 + (Taric?.GetHashCode() ?? 0);
            hash = hash * 23 + QuantityUnit.GetHashCode();
            hash = hash * 23 + (Description?.GetHashCode() ?? 0);
            hash = hash * 23 + CountryOfManufacture.GetHashCode();
            hash = hash * 23 + (UnitLength?.GetHashCode() ?? 0);
            hash = hash * 23 + (UnitWidth?.GetHashCode() ?? 0);
            hash = hash * 23 + (UnitHeight?.GetHashCode() ?? 0);
            hash = hash * 23 + Currency.GetHashCode();
            hash = hash * 23 + InOwnParcel.GetHashCode();
            hash = hash * 23 + (ArticleNumber?.GetHashCode() ?? 0);
            hash = hash * 23 + (ShelfPosition?.GetHashCode() ?? 0);

            return hash;
        }
    }
}
