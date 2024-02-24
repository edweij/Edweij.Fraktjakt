using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using Edweij.Fraktjakt.APIClient.Structs;
using Moq;
using Moq.Protected;
using System.Net;

namespace Edweij.Fraktjakt.APIClient.Tests;

[TestFixture]
public class FraktjaktClientTests
{
    private FraktjaktClient _fraktjaktClient;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;

    [SetUp]
    public void Setup()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _fraktjaktClient = new FraktjaktClient(123, "key", mockHttpClient, false);
    }    

    [Test]
    public void UrlEncode_ValidInput_EncodesCorrectly()
    {
        // Arrange
        string input = "Hello World!";

        // Act
        var encoded = FraktjaktClient.UrlEncode(input);

        // Assert
        Assert.That(encoded, Is.EqualTo("Hello+World!"));
    }

    [Test]
    public void MD5_ValidInput_ComputesHashCorrectly()
    {
        // Arrange
        string input = "Hello World!";

        // Act
        var md5Hash = FraktjaktClient.MD5(input);

        // Assert
        Assert.That(md5Hash, Is.EqualTo("ed076287532e86365e841e92bfc50d8c").IgnoreCase);
    }

    [Test]
    public async Task Trace_ValidInput_ReturnsResponse()
    {
        // Arrange
        int shipmentId = 456;
        SwedishOrEnglish lang = "SV";

        // Configure the handler to return a response
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"<?xml version=""1.0"" encoding=""UTF-8""?><result><server_status>ok</server_status><code>0</code><warning_message></warning_message><error_message></error_message>
<shipping_states><shipping_state><shipment_id>456</shipment_id><name>Levererat</name><id>2</id><fraktjakt_id>5</fraktjakt_id></shipping_state></shipping_states><tracking_code>b6dfc12fc04ec98132da2eb1c1739272cc646ed9</tracking_code>
<tracking_link>https://www.fraktjakt.se/trace/shipment/b6dfc12fc04ec98132da2eb1c1739272cc646ed9&amp;locale=sv</tracking_link><tracking_number>BG9700003016</tracking_number><shipping_company>DB Schenker</shipping_company></result>")
            });

        // Act
        var response = await _fraktjaktClient.Trace(shipmentId, lang);

        
        // Assert
        Assert.Multiple(() => {
            Assert.IsNotNull(response as TraceResponse);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            Assert.That(((TraceResponse)response).ShippingStates.ToList(), Has.Count.EqualTo(1));
            Assert.That(((TraceResponse)response).TrackingCode, Is.EqualTo("b6dfc12fc04ec98132da2eb1c1739272cc646ed9"));
            Assert.That(((TraceResponse)response).TrackingLink, Is.EqualTo("https://www.fraktjakt.se/trace/shipment/b6dfc12fc04ec98132da2eb1c1739272cc646ed9&locale=sv"));
            Assert.That(((TraceResponse)response).TrackingNumber, Is.EqualTo("BG9700003016"));
            Assert.That(((TraceResponse)response).ShippingCompany, Is.EqualTo("DB Schenker"));
        });        
    }

    [Test]
    public async Task Trace_Returns_ErrorResponse_On_NonSuccessfullStatus()
    {
        // Arrange
        int shipmentId = 456;
        SwedishOrEnglish lang = "SV";

        // Configure the handler to return a response
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(@"Error")
            });

        // Act
        var response = await _fraktjaktClient.Trace(shipmentId, lang);


        // Assert
        Assert.Multiple(() => {
            Assert.IsNotNull(response);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successfull response"));
        });
    }

    //[Test]
    //public async Task ShippingDocuments_ValidInput_ReturnsResponse()
    //{
    //    // Arrange
    //    int shipmentId = 789;
    //    SwedishOrEnglish lang = SwedishOrEnglish.SV;

    //    // Act
    //    var response = await _fraktjaktClient.ShippingDocuments(shipmentId, lang);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Success, response.ResponseStatus);
    //    Assert.AreEqual("Mocked shipping documents response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    //[Test]
    //public async Task Query_ValidShipment_ReturnsResponse()
    //{
    //    // Arrange
    //    var validShipment = CreateValidShipmentQuery();

    //    // Act
    //    var response = await _fraktjaktClient.Query(validShipment);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Success, response.ResponseStatus);
    //    Assert.AreEqual("Mocked query response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    //[Test]
    //public async Task ReQuery_ValidShipmentReQuery_ReturnsResponse()
    //{
    //    // Arrange
    //    var validShipmentReQuery = CreateValidShipmentReQuery();

    //    // Act
    //    var response = await _fraktjaktClient.ReQuery(validShipmentReQuery);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Success, response.ResponseStatus);
    //    Assert.AreEqual("Mocked requery response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    //[Test]
    //public async Task ReQuery_ShipmentId_ReturnsResponse()
    //{
    //    // Arrange
    //    int shipmentId = 123;
    //    bool shipperInfo = true;
    //    float? value = 456.78f;

    //    // Act
    //    var response = await _fraktjaktClient.ReQuery(shipmentId, shipperInfo, value);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Success, response.ResponseStatus);
    //    Assert.AreEqual("Mocked requery response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    //[Test]
    //public async Task Order_ValidOrder_ReturnsResponse()
    //{
    //    // Arrange
    //    var validOrder = CreateValidOrder();

    //    // Act
    //    var response = await _fraktjaktClient.Order(validOrder);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Ok, response.ResponseStatus);
    //    Assert.AreEqual("Mocked order response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    //[Test]
    //public async Task CreateShipment_ValidCreateShipment_ReturnsResponse()
    //{
    //    // Arrange
    //    var validCreateShipment = CreateValidCreateShipment();

    //    // Act
    //    var response = await _fraktjaktClient.CreateShipment(validCreateShipment);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Ok, response.ResponseStatus);
    //    Assert.AreEqual("Mocked create shipment response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    //[Test]
    //public async Task GetServicePoints_ValidUrl_ReturnsResponse()
    //{
    //    // Arrange
    //    string validUrl = "https://example.com/api/servicepoints";

    //    // Act
    //    var response = await _fraktjaktClient.GetServicePoints(validUrl);

    //    // Assert
    //    Assert.IsNotNull(response);
    //    Assert.AreEqual(ResponseStatus.Ok, response.ResponseStatus);
    //    Assert.AreEqual("Mocked service points response", response.ErrorMessage); // Adjust based on expected behavior
    //}

    [TearDown]
    public void Cleanup()
    {
        // Cleanup resources or perform any necessary teardown
        _fraktjaktClient.Dispose();
    }

    // Helper methods to create valid instances for testing
    private ShipmentQuery CreateValidShipmentQuery()
    {
        // Implement logic to create a valid ShipmentQuery for testing
        return new ShipmentQuery(new Sender(123, "key"), new ToAddress("12345"), items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
    }

    private ShipmentReQuery CreateValidShipmentReQuery()
    {
        // Implement logic to create a valid ShipmentReQuery for testing
        return new ShipmentReQuery(new Sender(123, "key"), 789);
    }

    private DirectOrder CreateValidDirectOrder()
    {
        return new DirectOrder(new Sender(123, "key"), new ToAddress("12345"), 123, items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
    }

    private OrderFromQuery CreateValidOrderFromQuery()
    {
        return new OrderFromQuery(new Sender(123, "key"), 123, 456, items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
    }

    private CreateShipment CreateValidCreateShipment()
    {
        // Implement logic to create a valid CreateShipment for testing
        return new CreateShipment(new Sender(123, "key"), new ToAddress("12345"), items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
    }

    
}
