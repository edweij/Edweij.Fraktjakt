using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;

[TestFixture]
public class ParcelTests
{
    [Test]
    public void ToXml_WithValidParcel_ShouldGenerateValidXml()
    {
        // Arrange
        var validParcel = new Parcel(1.5f)
        {
            Length = 10.0f,
            Width = 5.0f,
            Height = 3.0f
        };

        // Act
        var generatedXml = validParcel.ToXml();

        // Assert
        Assert.Multiple(() =>
        {
            // String contains assertions
            Assert.That(generatedXml, Does.Contain("<parcel>"));
            Assert.That(generatedXml, Does.Contain("<weight>1.5</weight>"));
            Assert.That(generatedXml, Does.Contain("<length>10</length>"));
            Assert.That(generatedXml, Does.Contain("<width>5</width>"));
            Assert.That(generatedXml, Does.Contain("<height>3</height>"));
            Assert.That(generatedXml, Does.Contain("</parcel>"));

            // XDocument assertions
            var xDocument = XDocument.Parse(generatedXml);
            Assert.That(xDocument.Root!.Name.LocalName, Is.EqualTo("parcel"));
            Assert.That(xDocument.Root.Elements().Count(), Is.EqualTo(4));
            Assert.That(xDocument.Descendants("weight").Single().Value, Is.EqualTo("1.5"));
            Assert.That(xDocument.Descendants("length").Single().Value, Is.EqualTo("10"));
            Assert.That(xDocument.Descendants("width").Single().Value, Is.EqualTo("5"));
            Assert.That(xDocument.Descendants("height").Single().Value, Is.EqualTo("3"));
        });
    }

    [Test]
    public void ToXml_WithInvalidParcel_ShouldThrowException()
    {
        // Arrange
        var invalidParcel = new Parcel(1.5f) { Length = 0 }; // invalid length

        // Act & Assert
        Assert.That(invalidParcel.ToXml, Throws.TypeOf<ArgumentException>().With.Message.EqualTo("Parcel element is not valid"));
    }

    [Test]
    public void GetRuleViolations_WithValidParcel_ShouldReturnEmptyList()
    {
        // Arrange
        var validParcel = new Parcel(1.5f)
        {
            Length = 10.0f,
            Width = 5.0f,
            Height = 3.0f
        };

        // Act
        var ruleViolations = validParcel.GetRuleViolations();

        // Assert
        Assert.That(ruleViolations, Is.Empty);
    }

    [TestCase(1.5f, 0.0f, null, null, "Length must be larger than 0")]
    [TestCase(1.5f, -1.0f, null, null, "Length must be larger than 0")]
    [TestCase(1.5f, 10.0f, 0.0f, null, "Width must be larger than 0")]
    [TestCase(1.5f, 10.0f, -1.0f, null, "Width must be larger than 0")]
    [TestCase(1.5f, 10.0f, 5.0f, 0.0f, "Height must be larger than 0")]
    [TestCase(1.5f, 10.0f, 5.0f, -1.0f, "Height must be larger than 0")]
    public void GetRuleViolations_WithInvalidParcel_ShouldReturnExpectedViolations(float weight, float? length, float? width, float? height, params string[] expectedMessages)
    {
        // Arrange
        var invalidParcel = new Parcel(weight)
        {
            Length = length,
            Width = width,
            Height = height
        };

        // Act
        var ruleViolations = invalidParcel.GetRuleViolations();

        // Assert
        Assert.That(ruleViolations.Select(v => v.Error), Is.EquivalentTo(expectedMessages));
    }        
}