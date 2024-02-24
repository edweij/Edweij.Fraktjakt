using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests.RequestModelTests
{
    public class AddressTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MinimalFromAdressGeneratesCorrectXml()
        {
            var address = new FromAddress("62141");
            var element = XElement.Parse(address.ToXml());
            Assert.Multiple(() =>
            {
                Assert.That(element.Elements().Count(), Is.EqualTo(3));
                Assert.That(element.Name.LocalName, Is.EqualTo("address_from"));
                Assert.That(element.Element("postal_code"), Is.Not.Null);
                Assert.That(element.Element("postal_code").Value, Is.EqualTo("62141"));
                Assert.That(element.Element("country_code"), Is.Not.Null);
                Assert.That(element.Element("country_code").Value, Is.EqualTo("SE"));
                Assert.That(element.Element("residental"), Is.Not.Null);
                Assert.That(element.Element("residental").Value, Is.EqualTo("1"));
            });

        }

        [Test]
        public void MinimalToAdressGeneratesCorrectXml()
        {
            var address = new ToAddress("62141");
            var element = XElement.Parse(address.ToXml());
            Assert.Multiple(() =>
            {
                Assert.That(element.Elements().Count(), Is.EqualTo(4));
                Assert.That(element.Name.LocalName, Is.EqualTo("address_to"));
                Assert.That(element.Element("postal_code"), Is.Not.Null);
                Assert.That(element.Element("postal_code").Value, Is.EqualTo("62141"));
                Assert.That(element.Element("country_code"), Is.Not.Null);
                Assert.That(element.Element("country_code").Value, Is.EqualTo("SE"));
                Assert.That(element.Element("residental"), Is.Not.Null);
                Assert.That(element.Element("residental").Value, Is.EqualTo("1"));
                Assert.That(element.Element("language"), Is.Not.Null);
                Assert.That(element.Element("language").Value, Is.EqualTo("sv"));
            });

        }

        [Test]
        public void MaximumFromAdressGeneratesCorrectXml()
        {
            var address = new FromAddress("62141")
            {
                CityName = "city",
                CountryCode = "SE",
                EntryCode = "1234",
                Instructions = "instructions",
                IsResidental = false,
                StreetAddress1 = "street1",
                StreetAddress2 = "street2",
                StreetAddress3 = "street3"
            };
            var element = XElement.Parse(address.ToXml());
            Assert.Multiple(() =>
            {
                Assert.That(element.Elements().Count(), Is.EqualTo(9));
                Assert.That(element.Name.LocalName, Is.EqualTo("address_from"));
                Assert.That(element.Element("postal_code"), Is.Not.Null);
                Assert.That(element.Element("postal_code").Value, Is.EqualTo("62141"));
                Assert.That(element.Element("country_code"), Is.Not.Null);
                Assert.That(element.Element("country_code").Value, Is.EqualTo("SE"));
                Assert.That(element.Element("residental"), Is.Not.Null);
                Assert.That(element.Element("residental").Value, Is.EqualTo("0"));
                Assert.That(element.Element("city_name"), Is.Not.Null);
                Assert.That(element.Element("city_name").Value, Is.EqualTo("city"));
                Assert.That(element.Element("entry_code"), Is.Not.Null);
                Assert.That(element.Element("entry_code").Value, Is.EqualTo("1234"));
                Assert.That(element.Element("instructions"), Is.Not.Null);
                Assert.That(element.Element("instructions").Value, Is.EqualTo("instructions"));
                Assert.That(element.Element("street_address_1"), Is.Not.Null);
                Assert.That(element.Element("street_address_1").Value, Is.EqualTo("street1"));
                Assert.That(element.Element("street_address_2"), Is.Not.Null);
                Assert.That(element.Element("street_address_2").Value, Is.EqualTo("street2"));
                Assert.That(element.Element("street_address_3"), Is.Not.Null);
                Assert.That(element.Element("street_address_3").Value, Is.EqualTo("street3"));
            });

        }

        [Test]
        public void MaximumToAdressGeneratesCorrectXml()
        {
            var address = new ToAddress("62141")
            {
                CityName = "city",
                CountryCode = "SE",
                EntryCode = "1234",
                Instructions = "instructions",
                IsResidental = false,
                StreetAddress1 = "street1",
                StreetAddress2 = "street2",
                StreetAddress3 = "street3",
                Language = Language6391.en
            };
            var element = XElement.Parse(address.ToXml());
            Assert.Multiple(() =>
            {
                Assert.That(element.Elements().Count(), Is.EqualTo(10));
                Assert.That(element.Name.LocalName, Is.EqualTo("address_to"));
                Assert.That(element.Element("postal_code"), Is.Not.Null);
                Assert.That(element.Element("postal_code").Value, Is.EqualTo("62141"));
                Assert.That(element.Element("country_code"), Is.Not.Null);
                Assert.That(element.Element("country_code").Value, Is.EqualTo("SE"));
                Assert.That(element.Element("residental"), Is.Not.Null);
                Assert.That(element.Element("residental").Value, Is.EqualTo("0"));
                Assert.That(element.Element("city_name"), Is.Not.Null);
                Assert.That(element.Element("city_name").Value, Is.EqualTo("city"));
                Assert.That(element.Element("entry_code"), Is.Not.Null);
                Assert.That(element.Element("entry_code").Value, Is.EqualTo("1234"));
                Assert.That(element.Element("instructions"), Is.Not.Null);
                Assert.That(element.Element("instructions").Value, Is.EqualTo("instructions"));
                Assert.That(element.Element("street_address_1"), Is.Not.Null);
                Assert.That(element.Element("street_address_1").Value, Is.EqualTo("street1"));
                Assert.That(element.Element("street_address_2"), Is.Not.Null);
                Assert.That(element.Element("street_address_2").Value, Is.EqualTo("street2"));
                Assert.That(element.Element("street_address_3"), Is.Not.Null);
                Assert.That(element.Element("street_address_3").Value, Is.EqualTo("street3"));
                Assert.That(element.Element("language"), Is.Not.Null);
                Assert.That(element.Element("language").Value, Is.EqualTo("en"));
            });

        }

        [Test]
        [TestCase("   ")]
        [TestCase("12345678901234567")]
        public void InvalidConstructor_ToAddress_ShouldThrow(string postalCode)
        {
            Assert.Throws<ArgumentException>(() => new ToAddress(postalCode));
        }

        [Test]
        [TestCase("   ")]
        [TestCase("12345678901234567")]
        public void InvalidConstructor_FromAddress_ShouldThrow(string postalCode)
        {
            Assert.Throws<ArgumentException>(() => new FromAddress(postalCode));
        }

        [Test]
        public void Invalid_ToAddress_ToXml_ShouldThrow()
        {
            var address = new ToAddress("12345") { StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam" };
            Assert.Throws<ArgumentException>(() => address.ToXml());
        }

        [Test]
        public void Invalid_FromAddress_ToXml_ShouldThrow()
        {
            var address = new FromAddress("12345") { StreetAddress1 = "Lorem ipsum dolor sit amet orci aliquam" };
            Assert.Throws<ArgumentException>(() => address.ToXml());
        }




    }
}