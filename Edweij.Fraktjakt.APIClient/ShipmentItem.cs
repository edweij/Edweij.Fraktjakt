using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public class ShipmentItem : XmlRequestObject
    {
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public bool Shipped { get; set; } = true;
        public int? Taric { get; set; }
        public QuantityUnit QuantityUnit { get; set; } = QuantityUnit.EA;
        public string? Description { get; set; }
        public CountryCode CountryOfManufacture { get; set; } = "SE";
        /// <summary>
        /// Total weight of this items units in kilograms
        /// </summary>
        public float? TotalWeight { get; set; }
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
        public float? UnitPrice { get; set; }
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
                    w.WriteElementString("name", Name!);
                    w.WriteElementString("quantity", Quantity!.Value.ToString());
                    w.WriteElementString("shipped", Shipped ? "1" : "0");
                    if (Taric.HasValue) w.WriteElementString("taric", Taric.Value.ToString());
                    w.WriteElementString("quantity_units", QuantityUnit.ToString());
                    if (!string.IsNullOrWhiteSpace(Description)) w.WriteElementString("description", Description);
                    w.WriteElementString("country_of_manufacture", CountryOfManufacture.ToString());
                    w.WriteElementString("weight", TotalWeight!.Value.ToStringPeriodDecimalSeparator());
                    if (UnitLength.HasValue) w.WriteElementString("length", UnitLength!.Value.ToStringPeriodDecimalSeparator());
                    if (UnitWidth.HasValue) w.WriteElementString("width", UnitWidth!.Value.ToStringPeriodDecimalSeparator());
                    if (UnitHeight.HasValue) w.WriteElementString("height", UnitHeight!.Value.ToStringPeriodDecimalSeparator());
                    w.WriteElementString("unit_price", UnitPrice!.Value.ToStringPeriodDecimalSeparator());
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
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new RuleViolation("Name", "Name is required");
            }
            else if (Name.Length > 64)
            {
                yield return new RuleViolation("Name", "Name is too long (max 64 characters)");
            }

            if (!Quantity.HasValue)
            {
                yield return new RuleViolation("Quantity", "Quantity is required");
            }
            else if (Quantity.Value < 1)
            {
                yield return new RuleViolation("Quantity", "Quantity must be larger than 0");
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                if (Description.Length < 15)
                {
                    yield return new RuleViolation("Description", "Description is too short (min 15 characters)");
                }

                if (Description.Length > 128)
                {
                    yield return new RuleViolation("Description", "Description is too long (max 128 characters)");
                }
            }

            if (!TotalWeight.HasValue)
            {
                yield return new RuleViolation("TotalItemWeight", "TotalItemWeight is required");
            }
            else if (TotalWeight.Value <= 0)
            {
                yield return new RuleViolation("TotalItemWeight", "TotalItemWeight must be larger than 0");
            }

            if (!UnitPrice.HasValue)
            {
                yield return new RuleViolation("UnitPrice", "UnitPrice is required");
            }
            else if (UnitPrice.Value < 0)
            {
                yield return new RuleViolation("UnitPrice", "UnitPrice can't be negative");
            }

            if (!string.IsNullOrWhiteSpace(ArticleNumber))
            {
                if (ArticleNumber.Length > 64)
                {
                    yield return new RuleViolation("ArticleNumber", "ArticleNumber is too long (max 64 characters)");
                }
            }

            if (!string.IsNullOrWhiteSpace(ShelfPosition))
            {
                if (ShelfPosition.Length > 64)
                {
                    yield return new RuleViolation("ShelfPosition", "ShelfPosition is too long (max 64 characters)");
                }
            }

            yield break;
        }

        
    }
}
