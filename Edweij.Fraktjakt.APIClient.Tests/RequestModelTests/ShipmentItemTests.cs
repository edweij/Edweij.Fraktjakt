using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;



[TestFixture]
public class ShipmentItemTests
{
    [Test]
    public void Constructor_ValidParameters_ShouldNotThrowException()
    {
        // Arrange, Act & Assert
        Assert.DoesNotThrow(() => new ShipmentItem("ItemName", 2, 10.5f, 5.5f));
    }

    [TestCase(null, 2, 10.5f, 5.5f)]
    [TestCase("", 2, 10.5f, 5.5f)]
    [TestCase("ItemName", 0, 10.5f, 5.5f)]
    [TestCase("ItemName", -1, 10.5f, 5.5f)]
    [TestCase("ItemName", 2, -10.5f, 5.5f)]
    [TestCase("ItemName", 2, 10.5f, -5.5f)]
    public void Constructor_InvalidParameters_ShouldThrowArgumentException(string name, int quantity, float unitPrice, float totalWeight)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ShipmentItem(name, quantity, unitPrice, totalWeight));
    }

    [Test]
    public void ToXml_ValidItem_ShouldReturnXmlString()
    {
        // Arrange
        var shipmentItem = new ShipmentItem("ItemName", 2, 10.5f, 5.5f)
        {
            Shipped = true,
            Taric = 123456,
            QuantityUnit = QuantityUnit.EA,
            Description = "Sample description",
            CountryOfManufacture = "SE",
            UnitLength = 20.5f,
            UnitWidth = 15.2f,
            UnitHeight = 8.3f,
            Currency = "SEK",
            InOwnParcel = false,
            ArticleNumber = "ABC123",
            ShelfPosition = "A12B34"
        };

        // Act
        string xml = shipmentItem.ToXml();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(xml, Is.Not.Null.And.Not.Empty);
            Assert.That(xml, Does.Contain("<name>ItemName</name>"));
            Assert.That(xml, Does.Contain("<quantity>2</quantity>"));
        });


    }

    [TestCase("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901", 2, 10.5f, 5.5f)]
    [TestCase("ItemName", -1, 10.5f, 5.5f)]
    [TestCase("ItemName", 2, -10.5f, 5.5f)]
    [TestCase("ItemName", 2, 10.5f, -5.5f)]
    public void ToXml_InvalidItem_ShouldThrowArgumentException(string name, int quantity, float unitPrice, float totalWeight)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ShipmentItem(name, quantity, unitPrice, totalWeight));
    }

    [Test]
    public void GetRuleViolations_InvalidDescription_ShouldReturnRuleViolation()
    {
        // Arrange
        var shipmentItem = new ShipmentItem("ItemName", 2, 10.5f, 5.5f)
        {
            Description = "Short"
        };

        // Act
        var ruleViolations = shipmentItem.GetRuleViolations();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ruleViolations, Has.Exactly(1).Items);
            Assert.That(ruleViolations, Has.Some.Property("PropertyName").EqualTo("Description"));
            Assert.That(ruleViolations, Has.Some.Property("Error").Contains("too short or too long"));
        });
    }

    [Test]
    public void GetRuleViolations_InvalidArticleNumber_ShouldReturnRuleViolation()
    {
        // Arrange
        var shipmentItem = new ShipmentItem("ItemName", 2, 10.5f, 5.5f)
        {
            ArticleNumber = "TooLongArticleNumber12345678901234567890123456789012345678901234567890123456789012345678901"
        };

        // Act
        var ruleViolations = shipmentItem.GetRuleViolations();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ruleViolations, Has.Exactly(1).Items);
            Assert.That(ruleViolations, Has.Some.Property("PropertyName").EqualTo("ArticleNumber"));
            Assert.That(ruleViolations, Has.Some.Property("Error").Contains("too long"));
        });

    }

    [Test]
    public void GetRuleViolations_InvalidShelfPosition_ShouldReturnRuleViolation()
    {
        // Arrange
        var shipmentItem = new ShipmentItem("ItemName", 2, 10.5f, 5.5f)
        {
            ShelfPosition = "TooLongShelfPosition12345678901234567890123456789012345678901234567890123456789012345678901"
        };

        // Act
        var ruleViolations = shipmentItem.GetRuleViolations();

        // Assert
        Assert.That(ruleViolations, Has.Exactly(1).Items);
        Assert.That(ruleViolations, Has.Some.Property("PropertyName").EqualTo("ShelfPosition"));
        Assert.That(ruleViolations, Has.Some.Property("Error").Contains("too long"));
    }

    [Test]
    public void Equals_SameInstance_ReturnsTrue()
    {
        // Arrange
        var shipmentItem = new ShipmentItem("Item1", 1, 10.0f, 2.5f);

        // Act
        var result = shipmentItem.Equals(shipmentItem);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var shipmentItem1 = new ShipmentItem("Item1", 2, 15.0f, 3.5f)
        {
            Shipped = false,
            Taric = 123,
            QuantityUnit = QuantityUnit.KG,
            Description = "Description",
            CountryOfManufacture = "SE",
            UnitLength = 10.5f,
            UnitWidth = 5.0f,
            UnitHeight = 2.0f,
            Currency = "USD",
            InOwnParcel = true,
            ArticleNumber = "12345",
            ShelfPosition = "A12"
        };

        var shipmentItem2 = new ShipmentItem("Item1", 2, 15.0f, 3.5f)
        {
            Shipped = false,
            Taric = 123,
            QuantityUnit = QuantityUnit.KG,
            Description = "Description",
            CountryOfManufacture = "SE",
            UnitLength = 10.5f,
            UnitWidth = 5.0f,
            UnitHeight = 2.0f,
            Currency = "USD",
            InOwnParcel = true,
            ArticleNumber = "12345",
            ShelfPosition = "A12"
        };

        // Act
        var result = shipmentItem1.Equals(shipmentItem2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var shipmentItem1 = new ShipmentItem("Item1", 2, 15.0f, 3.5f)
        {
            Shipped = false,
            Taric = 123,
            QuantityUnit = QuantityUnit.KG,
            Description = "Description",
            CountryOfManufacture = "SE",
            UnitLength = 10.5f,
            UnitWidth = 5.0f,
            UnitHeight = 2.0f,
            Currency = "USD",
            InOwnParcel = true,
            ArticleNumber = "12345",
            ShelfPosition = "A12"
        };

        var shipmentItem2 = new ShipmentItem("Item2", 3, 20.0f, 4.0f)
        {
            Shipped = true,
            Taric = 456,
            QuantityUnit = QuantityUnit.EA,
            Description = "Different Description",
            CountryOfManufacture = "US",
            UnitLength = 15.0f,
            UnitWidth = 6.0f,
            UnitHeight = 3.0f,
            Currency = "EUR",
            InOwnParcel = false,
            ArticleNumber = "67890",
            ShelfPosition = "B34"
        };

        // Act
        var result = shipmentItem1.Equals(shipmentItem2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        // Arrange
        var shipmentItem1 = new ShipmentItem("Item1", 2, 15.0f, 3.5f)
        {
            Shipped = false,
            Taric = 123,
            QuantityUnit = QuantityUnit.KG,
            Description = "Description",
            CountryOfManufacture = "SE",
            UnitLength = 10.5f,
            UnitWidth = 5.0f,
            UnitHeight = 2.0f,
            Currency = "USD",
            InOwnParcel = true,
            ArticleNumber = "12345",
            ShelfPosition = "A12"
        };

        var shipmentItem2 = new ShipmentItem("Item1", 2, 15.0f, 3.5f)
        {
            Shipped = false,
            Taric = 123,
            QuantityUnit = QuantityUnit.KG,
            Description = "Description",
            CountryOfManufacture = "SE",
            UnitLength = 10.5f,
            UnitWidth = 5.0f,
            UnitHeight = 2.0f,
            Currency = "USD",
            InOwnParcel = true,
            ArticleNumber = "12345",
            ShelfPosition = "A12"
        };

        // Act
        var hashCode1 = shipmentItem1.GetHashCode();
        var hashCode2 = shipmentItem2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        // Arrange
        var shipmentItem1 = new ShipmentItem("Item1", 2, 15.0f, 3.5f)
        {
            Shipped = false,
            Taric = 123,
            QuantityUnit = QuantityUnit.KG,
            Description = "Description",
            CountryOfManufacture = "SE",
            UnitLength = 10.5f,
            UnitWidth = 5.0f,
            UnitHeight = 2.0f,
            Currency = "USD",
            InOwnParcel = true,
            ArticleNumber = "12345",
            ShelfPosition = "A12"
        };

        var shipmentItem2 = new ShipmentItem("Item2", 3, 20.0f, 4.0f)
        {
            Shipped = true,
            Taric = 456,
            QuantityUnit = QuantityUnit.EA,
            Description = "Different Description",
            CountryOfManufacture = "US",
            UnitLength = 15.0f,
            UnitWidth = 6.0f,
            UnitHeight = 3.0f,
            Currency = "EUR",
            InOwnParcel = false,
            ArticleNumber = "67890",
            ShelfPosition = "B34"
        };

        // Act
        var hashCode1 = shipmentItem1.GetHashCode();
        var hashCode2 = shipmentItem2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
    }
}