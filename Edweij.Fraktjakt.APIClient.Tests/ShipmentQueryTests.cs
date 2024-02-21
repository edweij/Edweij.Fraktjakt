using Edweij.Fraktjakt.APIClient.RequestModels;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    public class ShipmentQueryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShipmentQueryConstructorShouldThrowOnNullOrInvalidparameters()
        {
            Assert.Throws<ArgumentNullException>(() => new ShipmentQuery(null, null));
            Assert.Throws<ArgumentNullException>(() => new ShipmentQuery(new Sender(1, "key"), null)); // to address is null
            Assert.Throws<ArgumentNullException>(() => new ShipmentQuery(null, new ToAddress { PostalCode = "62141" })); // sender is null
            Assert.Throws<ArgumentException>(() => new ShipmentQuery(new Sender(-1, "   "), new ToAddress { PostalCode = "62141" })); // invalid sender
            Assert.Throws<ArgumentException>(() => new ShipmentQuery(new Sender(1, "key"), new ToAddress())); // invalid to adress
            Assert.Throws<ArgumentException>(() => new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141"}, fromAddress: new FromAddress())); // invalid from adress
            Assert.Throws<ArgumentException>(() => new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141" }, items: new List<ShipmentItem>() { new ShipmentItem()})); // invalid items
            Assert.Throws<ArgumentException>(() => new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141" }, parcels: new List<Parcel>() { new Parcel() })); // invalid items
        }

        [Test]
        public void ShipmentQuerySetPropertyWithInvalidValueShouldThrow()
        {
            var query = new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141" });
            Assert.Throws<ArgumentNullException>(() => query.FromAddress = null); // Set fromadress to null
            Assert.Throws<ArgumentException>(() => query.FromAddress = new FromAddress()); // Set fromadress to invalid address
            Assert.Throws<ArgumentNullException>(() => query.Items = null); // Set items to null
            Assert.Throws<ArgumentException>(() => query.Items = new List<ShipmentItem>() { new ShipmentItem() }); // Set items with invalid item
            Assert.Throws<ArgumentNullException>(() => query.Parcels = null); // Set parcels to null
            Assert.Throws<ArgumentException>(() => query.Parcels = new List<Parcel>() { new Parcel() }); // Set parcels with invalid item
        }

        [Test]
        public void ShipmentQueryValidation()
        {
            var query = new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141" });
            var errors = query.GetRuleViolations();
            Assert.That(errors.Count(), Is.EqualTo(2));
            Assert.That(errors.Any(v => v.PropertyName == "Items"), Is.True);
            Assert.That(errors.Any(v => v.PropertyName == "Parcels"), Is.True);
            query.Items = new List<ShipmentItem> { new ShipmentItem { Name = "test", Quantity = 1, TotalWeight = 2.45f, UnitPrice = 199.00f } };
            errors = query.GetRuleViolations();
            Assert.That(errors.Count(), Is.EqualTo(0));
            query.CallbackUrl = "   ";
            query.ShippingProductId = 0;
            query.Value = -100f;
            Assert.That(errors.Count(), Is.EqualTo(3));
            Assert.That(errors.Any(v => v.PropertyName == "CallbackUrl"), Is.True);
            Assert.That(errors.Any(v => v.PropertyName == "ShippingProductId"), Is.True);
            Assert.That(errors.Any(v => v.PropertyName == "Value"), Is.True);
            query.ToAddress.PostalCode = null;
            Assert.That(errors.Count(), Is.EqualTo(5));
        }





        [Test]
        public void ShipmentItemGeneratesCorrectXml()
        {
            var query = new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141" }, items: new List<ShipmentItem> { new ShipmentItem { Name = "test", Quantity = 1, TotalWeight = 2.45f, UnitPrice = 199.00f } });
            var element = XElement.Parse(query.ToXml());
            Assert.That(element.Elements().Count(), Is.EqualTo(17));
            Assert.That(element.Name.LocalName, Is.EqualTo("shipment"));
            Assert.That(element.Element("consignor"), Is.Not.Null);
            Assert.That(element.Element("insure_default"), Is.Not.Null);
            Assert.That(element.Element("insure_default").Value, Is.EqualTo("0"));
            Assert.That(element.Element("price_sort"), Is.Not.Null);
            Assert.That(element.Element("price_sort").Value, Is.EqualTo("1"));
            Assert.That(element.Element("express"), Is.Not.Null);
            Assert.That(element.Element("express").Value, Is.EqualTo("0"));
            Assert.That(element.Element("pickup"), Is.Not.Null);
            Assert.That(element.Element("pickup").Value, Is.EqualTo("0"));
            Assert.That(element.Element("dropoff"), Is.Not.Null);
            Assert.That(element.Element("dropoff").Value, Is.EqualTo("0"));
            Assert.That(element.Element("green"), Is.Not.Null);
            Assert.That(element.Element("green").Value, Is.EqualTo("0"));
            Assert.That(element.Element("quality"), Is.Not.Null);
            Assert.That(element.Element("quality").Value, Is.EqualTo("0"));
            Assert.That(element.Element("time_guarantee"), Is.Not.Null);
            Assert.That(element.Element("time_guarantee").Value, Is.EqualTo("0"));
            Assert.That(element.Element("cold"), Is.Not.Null);
            Assert.That(element.Element("cold").Value, Is.EqualTo("0"));
            Assert.That(element.Element("frozen"), Is.Not.Null);
            Assert.That(element.Element("frozen").Value, Is.EqualTo("0"));
            Assert.That(element.Element("no_agents"), Is.Not.Null);
            Assert.That(element.Element("no_agents").Value, Is.EqualTo("0"));
            Assert.That(element.Element("no_prices"), Is.Not.Null);
            Assert.That(element.Element("no_prices").Value, Is.EqualTo("0"));
            Assert.That(element.Element("agents_in"), Is.Not.Null);
            Assert.That(element.Element("agents_in").Value, Is.EqualTo("0"));
            Assert.That(element.Element("shipper_info"), Is.Not.Null);
            Assert.That(element.Element("shipper_info").Value, Is.EqualTo("0"));
            Assert.That(element.Element("commodities"), Is.Not.Null);
            Assert.That(element.Element("address_to"), Is.Not.Null);
        }

        [Test]
        public void ShipmentQueryXmlReplacesEntities()
        {
            var query = new ShipmentQuery(new Sender(1, "key"), new ToAddress { PostalCode = "62141" }, items: new List<ShipmentItem> { new ShipmentItem { Name = "test", Quantity = 1, TotalWeight = 2.45f, UnitPrice = 199.00f } });
            query.CallbackUrl = "<&'\">";
            var result = query.ToXml();
            Assert.That(result, Contains.Substring("<callback_url>&lt;&amp;'\"&gt;</callback_url>"));
        }

    }
}