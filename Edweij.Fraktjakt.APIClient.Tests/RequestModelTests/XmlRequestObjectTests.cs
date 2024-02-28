using Edweij.Fraktjakt.APIClient.RequestModels;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;

[TestFixture]
public class XmlRequestObjectTests
{


    [Test]
    public void ToXml_ShouldGenerateValidXml()
    {
        // Arrange
        var xmlRequestObject = new MockXmlRequestObject();

        // Act
        var generatedXml = xmlRequestObject.ToXml();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(generatedXml, Is.Not.Null);
            Assert.That(xmlRequestObject.IsValidXml(generatedXml), Is.True);
        });
    }

    [Test]
    public void IsValidXml_ShouldReturnTrueForValidXml()
    {
        // Arrange
        var xmlRequestObject = new MockXmlRequestObject();
        string validXml = "<testElement><childElement>TestValue</childElement></testElement>";

        // Act & Assert
        Assert.That(xmlRequestObject.IsValidXml(validXml), Is.True);
    }

    [Test]
    public void IsValidXml_ShouldReturnFalseForInvalidXml()
    {
        // Arrange
        var xmlRequestObject = new MockXmlRequestObject();
        string invalidXml = "<testElement><childElement>TestValue</testElement>";

        // Act & Assert
        Assert.That(xmlRequestObject.IsValidXml(invalidXml), Is.False);
    }

    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        // Arrange
        var obj1 = new MyXmlRequestObject();
        var obj2 = new AnotherXmlRequestObject(); // Ensure a different type
        var obj3 = new MyXmlRequestObject();

        // Act
        var result = obj1.Equals(obj2);

        // Assert
        Assert.Multiple(() => {
            Assert.That(result, Is.False);
            Assert.That(obj1.Equals(obj3), Is.True);
        });
        
    }

    [Test]
    public void GetHashCode_ReturnsHashCode()
    {
        // Arrange
        var obj = new MyXmlRequestObject();

        // Act
        var hashCode = obj.GetHashCode();

        // Assert
        Assert.That(hashCode, Is.EqualTo(obj.GetHashCode())); // Ensure consistent hash code
    }
}

public class MockXmlRequestObject : XmlRequestObject
{
    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        throw new NotImplementedException();
    }

    public override string ToXml()
    {
        // Implement the ToXml method for testing purposes
        return "<testElement><childElement>TestValue</childElement></testElement>";
    }
}

// Define concrete classes for testing
public class MyXmlRequestObject : XmlRequestObject
{
    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        throw new NotImplementedException();
    }

    public override string ToXml()
    {
        // Implement the method for testing
        return "";
    }
}

public class AnotherXmlRequestObject : XmlRequestObject
{
    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        throw new NotImplementedException();
    }

    public override string ToXml()
    {
        // Implement the method for testing
        return "";
    }
}
