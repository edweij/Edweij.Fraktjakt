using System.Text;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    [TestFixture]
    public class XmlRequestObjectExtensionsTests
    {
        [Test]
        public void ToStringPeriodDecimalSeparator_ShouldFormatFloatWithPeriodSeparator()
        {
            // Arrange
            float value = 123.45f;

            // Act
            var result = value.ToStringPeriodDecimalSeparator();

            // Assert
            Assert.That(result, Is.EqualTo("123.45"));
        }

        [TestCase("valid.email@example.com")]
        [TestCase("another.valid.email@example.co.uk")]
        [TestCase("john.doe123@example.domain")]
        public void IsValidEmailAddress_ShouldReturnTrueForValidEmailAddresses(string emailAddress)
        {
            // Act & Assert
            Assert.That(emailAddress.IsValidEmailAddress(), Is.True);
        }

        [TestCase("invalid-email")]
        [TestCase("invalid@email@domain.com")]
        [TestCase("missing-at-sign.domain.com")]
        public void IsValidEmailAddress_ShouldReturnFalseForInvalidEmailAddresses(string emailAddress)
        {
            // Act & Assert
            Assert.That(emailAddress.IsValidEmailAddress(), Is.False);
        }

        
        [TestCase(1, "1")]
        [TestCase(1.0f, "1")]
        [TestCase(1.5f, "1.5")]
        [TestCase(1.555f, "1.555")]
        [TestCase(1.0001f, "1.0001")]        
        public void FloatToString(float val, string expected)
        {
            // Arrange & Act
            string result = val.ToStringPeriodDecimalSeparator();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
