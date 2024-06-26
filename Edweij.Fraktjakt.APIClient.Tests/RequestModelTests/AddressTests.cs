using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;

[TestFixture]
public class AddressTests
{
    [Test]
    public void Constructor_ValidPostalCode_PropertiesSetCorrectly()
    {
        // Arrange & Act
        var address = new TestAddress("12345");

        // Assert
        Assert.That(address.PostalCode, Is.EqualTo("12345"));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Constructor_InvalidPostalCode_ThrowsArgumentException(string invalidPostalCode)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new TestAddress(invalidPostalCode));
    }

    [Test]
    public void Constructor_LongPostalCode_ThrowsArgumentException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new TestAddress("12345678901234567"));
    }

    [Test]
    public void RuleViolations_LongStreetAddress1_ReturnsRuleViolation()
    {
        // Arrange
        var address = new TestAddress("12345")
        {
            StreetAddress1 = new string('a', 36)
        };

        // Act
        var violations = address.GetRuleViolations();

        // Assert
        Assert.That(violations, Has.Some.Matches<RuleViolation>(v => v.PropertyName == "StreetAddress1" && v.Error == "Max length 35"));
    }

    // Add similar tests for StreetAddress2, StreetAddress3, CityName, and other properties as needed

    [Test]
    public void ToXml_ValidAddress_ReturnsXmlString()
    {
        // Arrange
        var address = new TestAddress("12345")
        {
            StreetAddress1 = "Main St",
            CityName = "City",
            CountryCode = new CountryCode("SE")
        };

        // Act
        var xml = address.ToXml();
        var element = XElement.Parse(xml);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(xml, Is.Not.Empty);
            Assert.That(element.Elements().Count(), Is.EqualTo(5));
            Assert.That(element.Name.LocalName, Is.EqualTo("address"));
            Assert.That(element.Element("postal_code"), Is.Not.Null);
            Assert.That(element.Element("postal_code").Value, Is.EqualTo("12345"));
            Assert.That(element.Element("country_code"), Is.Not.Null);
            Assert.That(element.Element("country_code").Value, Is.EqualTo("SE"));
            Assert.That(element.Element("residential"), Is.Not.Null);
            Assert.That(element.Element("residential").Value, Is.EqualTo("1"));
            Assert.That(element.Element("city_name"), Is.Not.Null);
            Assert.That(element.Element("city_name").Value, Is.EqualTo("City"));
            Assert.That(element.Element("street_address_1"), Is.Not.Null);
            Assert.That(element.Element("street_address_1").Value, Is.EqualTo("Main St"));
        });
    }

   
}

// Creating a TestAddress class to expose protected members for testing
public class TestAddress : Address
{
    public TestAddress(string postalCode) : base(postalCode) { }

    public IEnumerable<RuleViolation> GetRuleViolationsPublic() => base.GetRuleViolations();

    public override string ToXml()
    {
        return $"<address>{base.ToXml()}</address>";
    }
}

