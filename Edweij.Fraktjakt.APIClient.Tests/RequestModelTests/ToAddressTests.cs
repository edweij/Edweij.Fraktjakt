using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;

[TestFixture]
public class ToAddressTests
{
    [Test]
    public void Constructor_ValidPostalCode_PropertiesSetCorrectly()
    {
        // Arrange & Act
        var toAddress = new ToAddress("12345");

        // Assert
        Assert.That(toAddress.PostalCode, Is.EqualTo("12345"));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Constructor_InvalidPostalCode_ThrowsArgumentException(string invalidPostalCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ToAddress(invalidPostalCode));
    }

    [Test]
    public void Constructor_LongPostalCode_ThrowsArgumentException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new ToAddress("12345678901234567"));
    }

    [Test]
    public void ToXml_ValidAddress_ReturnsXmlString()
    {
        // Arrange
        var toAddress = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            CountryCode = new CountryCode("SE"),
            Language = Language6391.en
        };

        // Act
        var xml = toAddress.ToXml();

        // Assert
        Assert.That(xml, Is.Not.Empty);
        Assert.That(xml, Contains.Substring("<address_to>"));
        Assert.That(xml, Contains.Substring("<street_address_1>Main St</street_address_1>"));
        Assert.That(xml, Contains.Substring("<city_name>City</city_name>"));
        Assert.That(xml, Contains.Substring("<country_code>SE</country_code>"));
        Assert.That(xml, Contains.Substring("<language>en</language>"));
    }
}
