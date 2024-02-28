using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;

[TestFixture]
public class FromAddressTests
{
    [Test]
    public void Constructor_ValidPostalCode_PropertiesSetCorrectly()
    {
        // Arrange & Act
        var fromAddress = new FromAddress("12345");

        // Assert
        Assert.That(fromAddress.PostalCode, Is.EqualTo("12345"));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Constructor_InvalidPostalCode_ThrowsArgumentException(string invalidPostalCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new FromAddress(invalidPostalCode));
    }

    [Test]
    public void Constructor_LongPostalCode_ThrowsArgumentException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new FromAddress("12345678901234567"));
    }

    [Test]
    public void ToXml_ValidAddress_ReturnsXmlString()
    {
        // Arrange
        var fromAddress = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            CountryCode = new CountryCode("SE")
        };

        // Act
        var xml = fromAddress.ToXml();

        // Assert
        Assert.That(xml, Is.Not.Empty);
        Assert.That(xml, Contains.Substring("<address_from>"));
        Assert.That(xml, Contains.Substring("<street_address_1>Main St</street_address_1>"));
        Assert.That(xml, Contains.Substring("<city_name>City</city_name>"));
        Assert.That(xml, Contains.Substring("<country_code>SE</country_code>"));
    }

    [Test]
    public void Equals_SameInstance_ReturnsTrue()
    {
        // Arrange
        var fromAddress = new FromAddress("12345");

        // Act
        var result = fromAddress.Equals(fromAddress);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var fromAddress1 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City"
        };

        var fromAddress2 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City"
        };

        // Act
        var result = fromAddress1.Equals(fromAddress2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var fromAddress1 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City1"
        };

        var fromAddress2 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City2"
        };

        // Act
        var result = fromAddress1.Equals(fromAddress2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        // Arrange
        var fromAddress1 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City"
        };

        var fromAddress2 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City"
        };

        // Act
        var hashCode1 = fromAddress1.GetHashCode();
        var hashCode2 = fromAddress2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        // Arrange
        var fromAddress1 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City1"
        };

        var fromAddress2 = new FromAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City2"
        };

        // Act
        var hashCode1 = fromAddress1.GetHashCode();
        var hashCode2 = fromAddress2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
    }
}