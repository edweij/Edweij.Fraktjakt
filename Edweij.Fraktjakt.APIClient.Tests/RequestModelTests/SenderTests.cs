using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests;

public class SenderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SenderIdZeroShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Sender(0, "my key"));
    }

    [Test]
    public void NegativeSenderIdShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Sender(-1, "my key"));
    }

    [Test]
    public void SenderKeyNullShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Sender(1, null));
    }

    [Test]
    public void SenderKeyOnlyWhitespaceShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Sender(1, "    "));
    }

    [Test]
    public void SenderGeneratesCorrectXml()
    {
        var sender = new Sender(1, "mykey");
        var element = XElement.Parse(sender.ToXml());
        Assert.Multiple(() =>
        {
            Assert.That(element.Elements().Count(), Is.EqualTo(6));
            Assert.That(element.Name.LocalName, Is.EqualTo("consignor"));
            Assert.That(element.Element("id"), Is.Not.Null);
            Assert.That(element.Element("id").Value, Is.EqualTo("1"));
            Assert.That(element.Element("key"), Is.Not.Null);
            Assert.That(element.Element("key").Value, Is.EqualTo("mykey"));
            Assert.That(element.Element("currency"), Is.Not.Null);
            Assert.That(element.Element("currency").Value, Is.EqualTo("SEK"));
            Assert.That(element.Element("language"), Is.Not.Null);
            Assert.That(element.Element("language").Value, Is.EqualTo("sv"));
            Assert.That(element.Element("encoding"), Is.Not.Null);
            Assert.That(element.Element("encoding").Value, Is.EqualTo("utf-8"));
            Assert.That(element.Element("api_version"), Is.Not.Null);
            Assert.That(element.Element("api_version").Value, Is.EqualTo("4.5.0"));
        });


        sender.SystemName = "My System";
        sender.SystemVersion = "1.0";
        sender.ModuleVersion = "1.0";
        element = XElement.Parse(sender.ToXml());
        Assert.Multiple(() =>
        {
            Assert.That(element.Elements().Count(), Is.EqualTo(9));
            Assert.That(element.Element("system_name"), Is.Not.Null);
            Assert.That(element.Element("system_name").Value, Is.EqualTo("My System"));
            Assert.That(element.Element("system_version"), Is.Not.Null);
            Assert.That(element.Element("system_version").Value, Is.EqualTo("1.0"));
            Assert.That(element.Element("module_version"), Is.Not.Null);
            Assert.That(element.Element("module_version").Value, Is.EqualTo("1.0"));
        });

    }

    [Test]
    public void SenderXmlReplacesEntities()
    {
        var sender = new Sender(1, "<>&'\"");
        sender.SystemName = "<>&'\"";
        sender.SystemVersion = "<>&'\"";
        sender.ModuleVersion = "<>&'\"";
        var result = sender.ToXml();

        Assert.Multiple(() =>
        {
            Assert.That(result, Contains.Substring("<key>&lt;&gt;&amp;'\"</key>"));
            Assert.That(result, Contains.Substring("<system_name>&lt;&gt;&amp;'\"</system_name>"));
            Assert.That(result, Contains.Substring("<system_version>&lt;&gt;&amp;'\"</system_version>"));
            Assert.That(result, Contains.Substring("<module_version>&lt;&gt;&amp;'\"</module_version>"));
        });
    }       

    [Test]
    public void Constructor_ReferredSender_ValidArguments_PropertiesSetCorrectly()
    {
        // Arrange
        int id = 1;
        string key = "key";

        // Act
        var referredSender = new ReferredSender(id, key);

        // Assert
        Assert.That(referredSender.Id, Is.EqualTo(id));
        Assert.That(referredSender.Key, Is.EqualTo(key));
    }

    [Test]
    public void Constructor_ReferredSender_InvalidId_ThrowsArgumentException()
    {
        // Arrange
        int invalidId = 0;
        string key = "key";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ReferredSender(invalidId, key));
    }

    [Test]
    public void Constructor_ReferredSender_NullOrWhitespaceKey_ThrowsArgumentException()
    {
        // Arrange
        int id = 1;
        string invalidKey = null;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ReferredSender(id, invalidKey));
    }

    [Test]
    public void Constructor_ReferredSender_WhitespaceKey_ThrowsArgumentException()
    {
        // Arrange
        int id = 1;
        string invalidKey = " ";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ReferredSender(id, invalidKey));
    }
}