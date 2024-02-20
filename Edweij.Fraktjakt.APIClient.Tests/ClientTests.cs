using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.Tests
{
    public class ClientTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void InvalidConstructorShouldThrow()
        {
            Assert.Throws<ArgumentException>(() => { new Client(0, null); });
            Assert.Throws<ArgumentException>(() => { new Client(0, "key"); });
            Assert.Throws<ArgumentException>(() => { new Client(123, null); });
            Assert.Throws<ArgumentException>(() => { new Client(123, "    "); });
        }


        



    }
}