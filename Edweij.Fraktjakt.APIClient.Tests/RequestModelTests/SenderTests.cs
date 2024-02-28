using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests
{
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
                Assert.That(element.Element("encoding").Value, Is.EqualTo("UTF-8"));
                Assert.That(element.Element("api_version"), Is.Not.Null);
                Assert.That(element.Element("api_version").Value, Is.EqualTo("4.5"));
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
        public void Equals_Sender_SameInstance_ReturnsTrue()
        {
            // Arrange
            var sender = new Sender(1, "key");

            // Act
            var result = sender.Equals(sender);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_Sender_SameValues_ReturnsTrue()
        {
            // Arrange
            var sender1 = new Sender(1, "key");
            var sender2 = new Sender(1, "key");

            // Act
            var result = sender1.Equals(sender2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_Sender_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var sender1 = new Sender(1, "key1");
            var sender2 = new Sender(2, "key2");

            // Act
            var result = sender1.Equals(sender2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_Sender_DifferentType_ReturnsFalse()
        {
            // Arrange
            var sender = new Sender(1, "key");
            var otherObject = new object();

            // Act
            var result = sender.Equals(otherObject);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetHashCode_Sender_SameValues_ReturnsSameHashCode()
        {
            // Arrange
            var sender1 = new Sender(1, "key");
            var sender2 = new Sender(1, "key");

            // Act
            var hashCode1 = sender1.GetHashCode();
            var hashCode2 = sender2.GetHashCode();

            // Assert
            Assert.That(hashCode1, Is.EqualTo(hashCode2));
        }

        [Test]
        public void GetHashCode_Sender_DifferentValues_ReturnsDifferentHashCode()
        {
            // Arrange
            var sender1 = new Sender(1, "key1");
            var sender2 = new Sender(2, "key2");

            // Act
            var hashCode1 = sender1.GetHashCode();
            var hashCode2 = sender2.GetHashCode();

            // Assert
            Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
        }

        [Test]
        public void GetHashCode_Sender_NullProperties_ReturnsHashCode()
        {
            // Arrange
            var sender = new Sender(1, "key")
            {
                SystemName = null,
                SystemVersion = null,
                ModuleVersion = null
            };

            // Act
            var hashCode = sender.GetHashCode();

            // Assert
            Assert.That(hashCode, Is.Not.EqualTo(0)); // Ensure non-zero hash code
        }

        [Test]
        public void EqualityOperator_Sender_SameInstance_ReturnsTrue()
        {
            // Arrange
            var sender = new Sender(1, "ApiKey1");

            // Act & Assert
            Assert.That(sender, Is.EqualTo(sender));
        }

        [Test]
        public void EqualityOperator_Sender_SameValues_ReturnsTrue()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey1");
            var sender2 = new Sender(1, "ApiKey1");

            // Act & Assert
            Assert.That(sender1, Is.EqualTo(sender2));
        }

        [Test]
        public void EqualityOperator_Sender_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey1");
            var sender2 = new Sender(2, "ApiKey2");

            // Act & Assert
            Assert.That(sender1, Is.Not.EqualTo(sender2));
        }

        [Test]
        public void InequalityOperator_Sender_NullObject_ReturnsFalse()
        {
            // Arrange
            var sender = new Sender(1, "ApiKey1");

            // Act & Assert
            Assert.That(sender == null, Is.False);
        }

        [Test]
        public void InequalityOperator_Sender_SameValues_ReturnsFalse()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey1");
            var sender2 = new Sender(1, "ApiKey1");

            // Act
            var result = sender1 != sender2;

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InequalityOperator_Sender_DifferentValues_ReturnsTrue()
        {
            // Arrange
            var sender1 = new Sender(1, "ApiKey1");
            var sender2 = new Sender(2, "ApiKey2");

            // Act
            var result = sender1 != sender2;

            // Act & Assert
            Assert.That(result, Is.True);
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

        [Test]
        public void Equals_ReferredSender_SameInstance_ReturnsTrue()
        {
            // Arrange
            var referredSender = new ReferredSender(1, "key");

            // Act
            var result = referredSender.Equals(referredSender);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_ReferredSender_SameValues_ReturnsTrue()
        {
            // Arrange
            var referredSender1 = new ReferredSender(1, "key");
            var referredSender2 = new ReferredSender(1, "key");

            // Act
            var result = referredSender1.Equals(referredSender2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_ReferredSender_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var referredSender1 = new ReferredSender(1, "key1");
            var referredSender2 = new ReferredSender(2, "key2");

            // Act
            var result = referredSender1.Equals(referredSender2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_ReferredSender_DifferentType_ReturnsFalse()
        {
            // Arrange
            var referredSender = new ReferredSender(1, "key");
            var otherObject = new object();

            // Act
            var result = referredSender.Equals(otherObject);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetHashCode_ReferredSender_SameValues_ReturnsSameHashCode()
        {
            // Arrange
            var referredSender1 = new ReferredSender(1, "key");
            var referredSender2 = new ReferredSender(1, "key");

            // Act
            var hashCode1 = referredSender1.GetHashCode();
            var hashCode2 = referredSender2.GetHashCode();

            // Assert
            Assert.That(hashCode1, Is.EqualTo(hashCode2));
        }

        [Test]
        public void GetHashCode_ReferredSender_DifferentValues_ReturnsDifferentHashCode()
        {
            // Arrange
            var referredSender1 = new ReferredSender(1, "key1");
            var referredSender2 = new ReferredSender(2, "key2");

            // Act
            var hashCode1 = referredSender1.GetHashCode();
            var hashCode2 = referredSender2.GetHashCode();

            // Assert
            Assert.That(hashCode1, Is.Not.EqualTo(hashCode2));
        }

        [Test]
        public void EqualityOperator_ReferredSender_SameValues_ReturnsTrue()
        {
            // Arrange
            var sender1 = new ReferredSender(1, "Key1");
            var sender2 = new ReferredSender(1, "Key1");

            // Act & Assert
            Assert.That(sender1, Is.EqualTo(sender2));
        }

        [Test]
        public void EqualityOperator_ReferredSender_DifferentValues_ReturnsFalse()
        {
            // Arrange
            var sender1 = new ReferredSender(1, "Key1");
            var sender2 = new ReferredSender(2, "Key2");

            // Act & Assert
            Assert.That(sender1, Is.Not.EqualTo(sender2));
        }

        [Test]
        public void InequalityOperator_ReferredSender_SameValues_ReturnsFalse()
        {
            // Arrange
            var sender1 = new ReferredSender(1, "Key1");
            var sender2 = new ReferredSender(1, "Key1");

            // Act 
            var result = sender1 != sender2;

            // Act & Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void InequalityOperator_ReferredSender_DifferentValues_ReturnsTrue()
        {
            // Arrange
            var sender1 = new ReferredSender(1, "Key1");
            var sender2 = new ReferredSender(2, "Key2");

            // Act 
            var result = sender1 != sender2;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void EqualityOperator_ReferredSender_NullObject_ReturnsFalse()
        {
            // Arrange
            ReferredSender sender = new ReferredSender(1, "Key1");

            // Act & Assert
            Assert.That(sender == null, Is.False);
        }

    }
}