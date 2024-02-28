using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.Tests.StructTests
{
    [TestFixture]
    public class SwedishOrEnglishTests
    {
        [TestCase("SV")]
        [TestCase("sv")]
        [TestCase("EN")]
        [TestCase("en")]
        public void ParameterizedConstructor_ValidCode_ShouldNotThrowException(string validCode)
        {
            // Arrange & Act
            SwedishOrEnglish languageCode = new SwedishOrEnglish(validCode);

            // Assert
            Assert.That(languageCode.Code, Is.EqualTo(validCode.ToLower()));
        }

        [TestCase("SE")]
        [TestCase("se")]
        [TestCase("US")]
        [TestCase("us")]
        public void ParameterizedConstructor_InvalidCode_ShouldThrowException(string invalidCode)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new SwedishOrEnglish(invalidCode));
        }

        [TestCase("SV", "sv")]
        [TestCase("sv", "sv")]
        [TestCase("EN", "en")]
        [TestCase("en", "en")]
        public void ToString_ShouldReturnCorrectCode(string inputCode, string expectedCode)
        {
            // Arrange
            SwedishOrEnglish languageCode = new SwedishOrEnglish(inputCode);

            // Act
            string result = languageCode.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedCode));
        }

        [Test]
        public void ImplicitConversionFromString_ValidCode_ShouldNotThrowException()
        {
            // Arrange & Act
            SwedishOrEnglish languageCode = "SV";

            // Assert
            Assert.That(languageCode.Code, Is.EqualTo("sv"));
        }

        [Test]
        public void ImplicitConversionFromString_InvalidCode_ShouldThrowException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = (SwedishOrEnglish)"SE"; });
        }
    }
}
