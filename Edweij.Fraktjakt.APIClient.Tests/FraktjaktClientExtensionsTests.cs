using Microsoft.Extensions.DependencyInjection;

namespace Edweij.Fraktjakt.APIClient.Tests;

[TestFixture]
public class FraktjaktServiceExtensionsTests
{
    [Test]
    public void AddFraktjaktClient_ShouldConfigureServiceCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();        
        int senderId = 123;
        string senderKey = "your_sender_key";

        // Act
        services.AddFraktjaktClient(senderId, senderKey);

        // You may want to manually create a service provider for testing
        using (var serviceProvider = services.BuildServiceProvider())
        {
            // Assert
            var fraktjaktClient = serviceProvider.GetRequiredService<IFraktjaktClient>();

            
            Assert.That(fraktjaktClient, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(fraktjaktClient.Sender.SystemName), Is.False);
            Assert.That(string.IsNullOrEmpty(fraktjaktClient.Sender.SystemVersion), Is.False);
        }
    }
}