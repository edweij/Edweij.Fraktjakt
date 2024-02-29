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
}