using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient.Tests.StructTests
{
    public class CountryCodeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CountryCodeEmptyConstructorDefaultsToSE()
        {
            CountryCode cc = new();
            Assert.That(cc.ToString(), Is.EqualTo("SE"));
        }

        [Test]
        public void CountryCodeConstructorForValidCountry()
        {
            var cc = new CountryCode("GB");
            Assert.That(cc.ToString(), Is.EqualTo("GB"));
            cc = new CountryCode("us");
            Assert.That(cc.ToString(), Is.EqualTo("US"));
        }

        [Test]
        public void CountryCodeToUpper()
        {
            var cc = new CountryCode("us");
            Assert.That(cc.ToString(), Is.EqualTo("US"));
        }

        [Test]
        public void CountryCodeImplicitConversionFromString()
        {
            CountryCode cc = "DK";
            Assert.That(cc.ToString(), Is.EqualTo("DK"));
        }

        [Test]
        public void InvalidCountryShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CountryCode("xxx"));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                CountryCode cc = "xxx";
            });
        }





    }
}