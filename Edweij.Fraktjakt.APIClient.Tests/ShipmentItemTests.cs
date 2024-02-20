using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{


    public class ShipmentItemTests
    {
        [Test]
        public void ValidationOfEmptyItem()
        {
            var item = new ShipmentItem();
            var errors = item.GetRuleViolations();
            Assert.Multiple(() =>
            {
                Assert.That(errors.Count(), Is.EqualTo(4));
                Assert.That(errors.Any(v => v.Error == "Name is required"), Is.True);
                Assert.That(errors.Any(v => v.Error == "Quantity is required"), Is.True);
                Assert.That(errors.Any(v => v.Error == "TotalItemWeight is required"), Is.True);
                Assert.That(errors.Any(v => v.Error == "UnitPrice is required"), Is.True);
            });            
        }

        [Test]
        public void IsValid_WithValidShipmentItem_ShouldReturnTrue()
        {
            // Arrange
            var validShipmentItem = new ShipmentItem
            {
                Name = "TestItem",
                Quantity = 2,
                TotalWeight = 1.5f,
                UnitPrice = 10.0f
            };

            // Act
            var isValid = validShipmentItem.IsValid;

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void IsValid_WithMissingRequiredProperties_ShouldReturnFalseAndHaveCorrectViolations()
        {
            // Arrange
            var invalidShipmentItem = new ShipmentItem
            {
                // Not setting all required properties to make the object invalid
                Quantity = 0, // Invalid quantity
                TotalWeight = 0.0f, // Invalid total weight
                UnitPrice = -1.0f, // Invalid unit price                
            };

            // Act
            var isValid = invalidShipmentItem.IsValid;
            var ruleViolations = invalidShipmentItem.GetRuleViolations();

            // Assert
            Assert.Multiple(() =>
            {
                // Check IsValid property
                Assert.That(isValid, Is.False);

                // Check rule violations
                Assert.That(ruleViolations, Is.Not.Empty);
                Assert.That(ruleViolations, Has.Exactly(4).Items); // Assuming four required properties are not set

                // Example assertions for specific rule violations
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "Name" && v.Error == "Name is required"));
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "Quantity" && v.Error == "Quantity must be larger than 0"));
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "TotalItemWeight" && v.Error == "TotalItemWeight must be larger than 0"));
                Assert.That(ruleViolations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "UnitPrice" && v.Error == "UnitPrice can't be negative"));
                
            });
        }

        [Test]
        public void NotValidStringLengths()
        {
            var item = new ShipmentItem
            {
                Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit volutpat.",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam luctus convallis ullamcorper. Pellentesque condimentum nibh in nunc.",
                ArticleNumber = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam sed.",
                ShelfPosition = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam sed.",
                Quantity = 1,
                TotalWeight = 1,
                UnitPrice = 1
            };

            var errors = item.GetRuleViolations();
            Assert.Multiple(() => {
                Assert.That(errors.Count(), Is.EqualTo(4));
                Assert.That(errors.Any(v => v.Error == "Name is too long (max 64 characters)"), Is.True);
                Assert.That(errors.Any(v => v.Error == "Description is too long (max 128 characters)"), Is.True);
                Assert.That(errors.Any(v => v.Error == "ArticleNumber is too long (max 64 characters)"), Is.True);
                Assert.That(errors.Any(v => v.Error == "ShelfPosition is too long (max 64 characters)"), Is.True);
            });
            

            var item2 = new ShipmentItem
            {
                Name = "Lorem ipsum",
                Description = "To short",
                Quantity = 1,
                TotalWeight = 1,
                UnitPrice = 1
            };
            errors = item2.GetRuleViolations();
            Assert.Multiple(() => {
                Assert.That(errors.Count(), Is.EqualTo(1));
                Assert.That(errors.Any(v => v.Error == "Description is too short (min 15 characters)"), Is.True);
            });            
        }

        [Test]
        public void NotValidNegativeQuantatyOrZero()
        {
            var item1 = new ShipmentItem
            {
                Name = "Lorem ipsum",
                Quantity = 0,
                TotalWeight = 1,
                UnitPrice = 1
            };

            var errors = item1.GetRuleViolations();
            Assert.Multiple(() => {
                Assert.That(errors.Count(), Is.EqualTo(1));
                Assert.That(errors.Any(v => v.Error == "Quantity must be larger than 0"), Is.True);
            });
            

            var item2 = new ShipmentItem
            {
                Name = "Lorem ipsum",
                Quantity = -1,
                TotalWeight = 1,
                UnitPrice = 1
            };
            errors = item2.GetRuleViolations();
            Assert.Multiple(() => {
                Assert.That(errors.Count(), Is.EqualTo(1));
                Assert.That(errors.Any(v => v.Error == "Quantity must be larger than 0"), Is.True);
            });
            
        }

        [Test]
        public void NotValidNegativeTotalWeightOrZero()
        {
            var item1 = new ShipmentItem
            {
                Name = "Lorem ipsum",
                Quantity = 1,
                TotalWeight = -1,
                UnitPrice = 1
            };

            var errors = item1.GetRuleViolations();
            Assert.Multiple(() => {
                Assert.That(errors.Count(), Is.EqualTo(1));
                Assert.That(errors.Any(v => v.Error == "TotalItemWeight must be larger than 0"), Is.True);
            });
            

            var item2 = new ShipmentItem
            {
                Name = "Lorem ipsum",
                Quantity = 1,
                TotalWeight = 0,
                UnitPrice = 1
            };
            errors = item2.GetRuleViolations();
            Assert.That(errors.Count(), Is.EqualTo(1));
            Assert.That(errors.Any(v => v.Error == "TotalItemWeight must be larger than 0"), Is.True);
        }

        [Test]
        public void NotValidNegativeUnitPrice()
        {
            var item = new ShipmentItem
            {
                Name = "Lorem ipsum",
                Quantity = 1,
                TotalWeight = 1,
                UnitPrice = -1
            };

            var errors = item.GetRuleViolations();
            Assert.That(errors.Count(), Is.EqualTo(1));
            Assert.That(errors.Any(v => v.Error == "UnitPrice can't be negative"), Is.True);


        }



        [Test]
        public void ShipmentItemGeneratesCorrectXml()
        {
            var item = new ShipmentItem
            {
                ArticleNumber = "SKU",
                Currency = "SEK",
                Description = "Description Description",
                Name = "Name",
                Quantity = 1,
                TotalWeight = 12.3f,
                ShelfPosition = "ShelfPosition",
                Taric = 1234,
                UnitHeight = 5.5f,
                UnitLength = 5.5f,
                UnitWidth = 5.5f,
                QuantityUnit = QuantityUnit.EA,
                CountryOfManufacture = "SE",
                InOwnParcel = false,
                Shipped = true,
                UnitPrice = 99.99f
            };
            var element = XElement.Parse(item.ToXml());
            Assert.That(element.Elements().Count(), Is.EqualTo(16));
            Assert.That(element.Name.LocalName, Is.EqualTo("commodity"));
            Assert.That(element.Element("article_number"), Is.Not.Null);
            Assert.That(element.Element("article_number").Value, Is.EqualTo("SKU"));
            Assert.That(element.Element("currency"), Is.Not.Null);
            Assert.That(element.Element("currency").Value, Is.EqualTo("SEK"));
            Assert.That(element.Element("description"), Is.Not.Null);
            Assert.That(element.Element("description").Value, Is.EqualTo("Description Description"));
            Assert.That(element.Element("name"), Is.Not.Null);
            Assert.That(element.Element("name").Value, Is.EqualTo("Name"));
            Assert.That(element.Element("quantity"), Is.Not.Null);
            Assert.That(element.Element("quantity").Value, Is.EqualTo("1"));
            Assert.That(element.Element("weight"), Is.Not.Null);
            Assert.That(element.Element("weight").Value, Is.EqualTo("12.3"));
            Assert.That(element.Element("shelf_position"), Is.Not.Null);
            Assert.That(element.Element("shelf_position").Value, Is.EqualTo("ShelfPosition"));
            Assert.That(element.Element("taric"), Is.Not.Null);
            Assert.That(element.Element("taric").Value, Is.EqualTo("1234"));
            Assert.That(element.Element("height"), Is.Not.Null);
            Assert.That(element.Element("height").Value, Is.EqualTo("5.5"));
            Assert.That(element.Element("width"), Is.Not.Null);
            Assert.That(element.Element("width").Value, Is.EqualTo("5.5"));
            Assert.That(element.Element("length"), Is.Not.Null);
            Assert.That(element.Element("length").Value, Is.EqualTo("5.5"));
            Assert.That(element.Element("quantity_units"), Is.Not.Null);
            Assert.That(element.Element("quantity_units").Value, Is.EqualTo("EA"));
            Assert.That(element.Element("country_of_manufacture"), Is.Not.Null);
            Assert.That(element.Element("country_of_manufacture").Value, Is.EqualTo("SE"));
            Assert.That(element.Element("in_own_parcel"), Is.Not.Null);
            Assert.That(element.Element("in_own_parcel").Value, Is.EqualTo("0"));
            Assert.That(element.Element("shipped"), Is.Not.Null);
            Assert.That(element.Element("shipped").Value, Is.EqualTo("1"));
            Assert.That(element.Element("unit_price"), Is.Not.Null);
            Assert.That(element.Element("unit_price").Value, Is.EqualTo("99.99"));
        }

        [Test]
        public void ShipmentItemXmlReplacesEntities()
        {
            var item = new ShipmentItem
            {
                Name = "<Lorem \"&\" ipsum>",
                Description = "<Description & Description's>",
                ShelfPosition = "<shelfposition>",
                ArticleNumber = "<articlenumber>",
                Quantity = 1,
                TotalWeight = 1,
                UnitPrice = 1
            };
            var result = item.ToXml();
            Assert.That(result, Contains.Substring("<name>&lt;Lorem \"&amp;\" ipsum&gt;</name>"));
            Assert.That(result, Contains.Substring("<description>&lt;Description &amp; Description's&gt;</description>"));
            Assert.That(result, Contains.Substring("<shelf_position>&lt;shelfposition&gt;</shelf_position>"));
            Assert.That(result, Contains.Substring("<article_number>&lt;articlenumber&gt;</article_number>"));
        }

    }
}