using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    [Test]
    public void Equals_SameInstance_ReturnsTrue()
    {
        // Arrange
        var toAddress = new ToAddress("12345");

        // Act
        var result = toAddress.Equals(toAddress);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var toAddress1 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            Language = Language6391.sv
        };

        var toAddress2 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            Language = Language6391.sv
        };

        // Act
        var result = toAddress1.Equals(toAddress2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var toAddress1 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City1",
            Language = Language6391.sv
        };

        var toAddress2 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City2",
            Language = Language6391.sv
        };

        // Act
        var result = toAddress1.Equals(toAddress2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        // Arrange
        var toAddress1 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            Language = Language6391.sv
        };

        var toAddress2 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            Language = Language6391.sv
        };

        // Act
        var hashCode1 = toAddress1.GetHashCode();
        var hashCode2 = toAddress2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        // Arrange
        var toAddress1 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City1",
            Language = Language6391.sv
        };

        var toAddress2 = new ToAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City2",
            Language = Language6391.sv
        };

        // Act
        var hashCode1 = toAddress1.GetHashCode();
        var hashCode2 = toAddress2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
    }

    [Test]
    public void EqualityOperator_SameToAddresses_ReturnsTrue()
    {
        // Arrange
        var toAddress1 = new ToAddress("12345");
        var toAddress2 = new ToAddress("12345");

        // Act & Assert
        Assert.That(toAddress1 == toAddress2, Is.True);
    }

    [Test]
    public void EqualityOperator_DifferentToAddresses_ReturnsFalse()
    {
        // Arrange
        var toAddress1 = new ToAddress("12345");
        var toAddress2 = new ToAddress("67890");

        // Act & Assert
        Assert.That(toAddress1 != toAddress2, Is.True);
    }
}
