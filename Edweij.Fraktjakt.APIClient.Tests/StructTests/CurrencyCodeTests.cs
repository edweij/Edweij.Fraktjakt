using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.Tests.StructTests;

[TestFixture]
public class CurrencyCodeTests
{
    [Test]
    public void Constructor_ValidCode_PropertiesSetCorrectly()
    {
        // Arrange & Act
        var currencyCode = new CurrencyCode("SEK");

        // Assert
        Assert.That(currencyCode.Code, Is.EqualTo("SEK"));
    }

    [TestCase("invalid")]
    [TestCase("USD ")] // With trailing space
    public void Constructor_InvalidCode_ThrowsArgumentOutOfRangeException(string invalidCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new CurrencyCode(invalidCode));
    }

    [TestCase(null)]
    [TestCase("")]
    public void Constructor_InvalidCode_ThrowsArgumentNullException(string invalidCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CurrencyCode(invalidCode));
    }

    [Test]
    public void ImplicitConversionFromString_ValidCode_ReturnsCurrencyCodeInstance()
    {
        // Arrange & Act
        CurrencyCode currencyCode = "SEK";

        // Assert
        Assert.That(currencyCode.Code, Is.EqualTo("SEK"));
    }

    [Test]
    public void Equals_SameInstance_ReturnsTrue()
    {
        // Arrange
        var currencyCode = new CurrencyCode("SEK");

        // Act
        var result = currencyCode.Equals(currencyCode);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var currencyCode1 = new CurrencyCode("SEK");
        var currencyCode2 = new CurrencyCode("SEK");

        // Act
        var result = currencyCode1.Equals(currencyCode2);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var currencyCode1 = new CurrencyCode("SEK");
        var currencyCode2 = new CurrencyCode("USD");

        // Act
        var result = currencyCode1.Equals(currencyCode2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var currencyCode = new CurrencyCode("SEK");
        var otherObject = new object();

        // Act
        var result = currencyCode.Equals(otherObject);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        // Arrange
        var currencyCode1 = new CurrencyCode("SEK");
        var currencyCode2 = new CurrencyCode("SEK");

        // Act
        var hashCode1 = currencyCode1.GetHashCode();
        var hashCode2 = currencyCode2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.EqualTo(hashCode2));
    }

    [Test]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
    {
        // Arrange
        var currencyCode1 = new CurrencyCode("SEK");
        var currencyCode2 = new CurrencyCode("USD");

        // Act
        var hashCode1 = currencyCode1.GetHashCode();
        var hashCode2 = currencyCode2.GetHashCode();

        // Assert
        Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
    }

    [Test]
    public void ToString_ReturnsCode()
    {
        // Arrange
        var currencyCode = new CurrencyCode("SEK");

        // Act
        var result = currencyCode.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("SEK"));
    }
}