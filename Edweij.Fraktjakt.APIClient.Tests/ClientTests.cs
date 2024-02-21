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
            Assert.Throws<ArgumentException>(() => { new FraktjaktClient(0, null); });
            Assert.Throws<ArgumentException>(() => { new FraktjaktClient(0, "key"); });
            Assert.Throws<ArgumentException>(() => { new FraktjaktClient(123, null); });
            Assert.Throws<ArgumentException>(() => { new FraktjaktClient(123, "    "); });
        }


        



    }
}