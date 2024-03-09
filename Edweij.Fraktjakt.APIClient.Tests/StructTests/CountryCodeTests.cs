using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.Tests.StructTests;

[TestFixture]
public class CountryCodeTests
{
    [Test]
    public void Constructor_ValidCode_PropertiesSetCorrectly()
    {
        // Arrange & Act
        var countryCode = new CountryCode("SE");

        // Assert
        Assert.That(countryCode.Code, Is.EqualTo("SE"));
    }

    [TestCase("invalid")]
    [TestCase("US ")] // With trailing space
    public void Constructor_InvalidCode_ThrowsArgumentOutOfRangeException(string invalidCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new CountryCode(invalidCode));
    }

    [TestCase(null)]
    [TestCase("")]
    public void Constructor_InvalidCode_ThrowsArgumentNullException(string invalidCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CountryCode(invalidCode));
    }

    [Test]
    public void ImplicitConversionFromString_ValidCode_ReturnsCountryCodeInstance()
    {
        // Arrange & Act
        CountryCode countryCode = "SE";

        // Assert
        Assert.That(countryCode.Code, Is.EqualTo("SE"));
    }

    [Test]
    public void Equals_SameInstance_ReturnsTrue()
    {
        // Arrange
        var countryCode = new CountryCode("SE");

        // Act
        var result = countryCode.Equals(countryCode);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var countryCode1 = new CountryCode("SE");
        var countryCode2 = new CountryCode("SE");

        // Act
        var result = countryCode1.Equals(countryCode2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var countryCode1 = new CountryCode("SE");
        var countryCode2 = new CountryCode("US");

        // Act
        var result = countryCode1.Equals(countryCode2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var countryCode = new CountryCode("SE");
        var otherObject = new object();

        // Act
        var result = countryCode.Equals(otherObject);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        // Arrange
        var countryCode1 = new CountryCode("SE");
        var countryCode2 = new CountryCode("SE");

        // Act
        var hashCode1 = countryCode1.GetHashCode();
        var hashCode2 = countryCode2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        // Arrange
        var countryCode1 = new CountryCode("SE");
        var countryCode2 = new CountryCode("US");

        // Act
        var hashCode1 = countryCode1.GetHashCode();
        var hashCode2 = countryCode2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
    }

    [Test]
    public void ToString_ReturnsCode()
    {
        // Arrange
        var countryCode = new CountryCode("SE");

        // Act
        var result = countryCode.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("SE"));
    }
}