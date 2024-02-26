using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using Edweij.Fraktjakt.APIClient.Structs;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

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
    public void InvalidConstructorShouldThrow()
    {
        Assert.Multiple(() => {
            Assert.That(() => { var client = new FraktjaktClient(0, null); }, Throws.ArgumentException);
            Assert.That(() => { var client = new FraktjaktClient(0, "key"); }, Throws.ArgumentException);
            Assert.That(() => { var client = new FraktjaktClient(123, null); }, Throws.ArgumentException);
            Assert.That(() => { var client = new FraktjaktClient(123, "    "); }, Throws.ArgumentException);            
        });
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
        SetupMessageHandlerForSuccessResponse(new StringContent(@"<?xml version=""1.0"" encoding=""UTF-8""?><result><server_status>ok</server_status><code>0</code><warning_message></warning_message><error_message></error_message>
<shipping_states><shipping_state><shipment_id>456</shipment_id><name>Levererat</name><id>2</id><fraktjakt_id>5</fraktjakt_id></shipping_state></shipping_states><tracking_code>b6dfc12fc04ec98132da2eb1c1739272cc646ed9</tracking_code>
<tracking_link>https://www.fraktjakt.se/trace/shipment/b6dfc12fc04ec98132da2eb1c1739272cc646ed9&amp;locale=sv</tracking_link><tracking_number>BG9700003016</tracking_number><shipping_company>DB Schenker</shipping_company></result>"));


        // Act
        var response = await _fraktjaktClient.Trace(shipmentId, lang);

        
        // Assert
        Assert.Multiple(() => {
            Assert.That(response as TraceResponse, Is.Not.Null);
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
        SetupMessageHandlerForBadRequestResponse();
        
        // Act
        var response = await _fraktjaktClient.Trace(shipmentId, lang);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successful response"));
        });
    }

    [Test]
    public async Task ShippingDocuments_ValidInput_ReturnsResponse()
    {
        // Arrange
        int shipmentId = 456;
        SwedishOrEnglish lang = "SV";        
        string pdfContent = "%PDF-1.3\n1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R >>\nendobj\n4 0 obj\n<< /Length 26 >>\nstream\nBT /F1 12 Tf 72 720 Td (Hello, World!) Tj ET\nendstream\nendobj\n5 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n";
        byte[] pdfBytes = Encoding.ASCII.GetBytes(pdfContent);
        string pdfBase64 = Convert.ToBase64String(pdfBytes);
        SetupMessageHandlerForSuccessResponse(new StringContent(@$"<?xml version=""1.0"" encoding=""UTF-8""?><result><server_status>ok</server_status><code>0</code><warning_message></warning_message><error_message></error_message>
<shipping_documents><shipping_document><name>PATH/Filename.pdf</name><type_id>1</type_id><type_name>Fraktetikett</type_name><type_description>type-description</type_description><state_name>Redo</state_name><state_description>state-description</state_description>
<format_name>Etiketter 105 x 220 mm</format_name><file>{pdfBase64}</file></shipping_document></shipping_documents></result>"));
        
        // Act
        var response = await _fraktjaktClient.ShippingDocuments(shipmentId, lang);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response as ShippingDocumentsResponse, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            Assert.That(((ShippingDocumentsResponse)response).Documents.ToList(), Has.Count.EqualTo(1));
            Assert.That(((ShippingDocumentsResponse)response).Documents.First().File, Is.EqualTo(pdfBase64));
            Assert.That(((ShippingDocumentsResponse)response).Documents.First().PdfFromBase64, Is.EqualTo(pdfBytes));
        });
    }

    [Test]
    public async Task ShippingDocuments_Returns_ErrorResponse_On_NonSuccessfullStatus()
    {
        // Arrange
        int shipmentId = 456;
        SwedishOrEnglish lang = "SV";
        SetupMessageHandlerForBadRequestResponse();

        // Act
        var response = await _fraktjaktClient.ShippingDocuments(shipmentId, lang);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successful response"));
        });
    }

    [Test]
    public async Task Query_ValidInput_ReturnsResponse()
    {
        // Arrange
        var query = new ShipmentQuery(_fraktjaktClient.Sender, new ToAddress("12345"), items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
        SetupMessageHandlerForSuccessResponse(new StringContent(@$"<?xml version=""1.0"" encoding=""UTF-8""?><shipment><server_status>ok</server_status><code>0</code><warning_message></warning_message><error_message></error_message>
<currency>SEK</currency><id>67887</id><access_code>ABC12345</access_code><access_link>https://www.fraktjakt.se/shipments/show/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</access_link>
<tracking_code>ABC12345</tracking_code><tracking_link>https://www.fraktjakt.se/trace/shipment/b6dfc12fc04</tracking_link><agent_selection_link>https://www.fraktjakt.se/agents/search_closest</agent_selection_link>
<shipping_products><shipping_product><id>15</id><name>Privat</name><description>Bussgods - Privat</description><arrival_time>1-2 dagar</arrival_time><price>159.50</price><tax_class>25.00</tax_class><insurance_fee>75.00</insurance_fee>
<insurance_tax_class>0</insurance_tax_class><to_agent>1</to_agent><agent_info>Cityterminalen Stockholm ca 2 km i Stockholm</agent_info><agent_link>https://www.fraktjakt.se/agents/search_closest</agent_link>
<agent_in_info>Jönköping Bussgods ca 1 km i Jönköping</agent_in_info><agent_in_link>https://www.fraktjakt.se/agents/search_closest/377482?type=8&amp;shipper=4</agent_in_link>
<service_point_locator_api>https://www.fraktjakt.se/agents/service_point_locator</service_point_locator_api><shipper><id>4</id><name>Bussgods</name><logo_url>https://www.fraktjakt.se/images/shippers/4.png</logo_url></shipper></shipping_product>
</shipping_products></shipment>"));        

        // Act
        var response = await _fraktjaktClient.Query(query);

        // Assert
        Assert.Multiple(() => {
            Assert.That(response as QueryResponse, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            Assert.That(((QueryResponse)response).Products.ToList(), Has.Count.EqualTo(1));
        });
    }

    [Test]
    public async Task Query_Returns_ErrorResponse_On_NonSuccessfullStatus()
    {
        // Arrange
        var query = new ShipmentQuery(_fraktjaktClient.Sender, new ToAddress("12345"), items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
        SetupMessageHandlerForBadRequestResponse();

        // Act
        var response = await _fraktjaktClient.Query(query);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successful response"));
        });
    }

    [Test]
    public async Task ReQuery_ValidInput_ReturnsResponse()
    {
        // Arrange
        var shipment = new ShipmentReQuery(_fraktjaktClient.Sender, 123);
        SetupMessageHandlerForSuccessResponse(new StringContent(@$"<?xml version=""1.0"" encoding=""UTF-8""?><shipment><server_status>ok</server_status><code>0</code><warning_message></warning_message><error_message></error_message>
<currency>SEK</currency><id>123</id><access_code>ABC12345</access_code><access_link>https://www.fraktjakt.se/shipments/show/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</access_link>
<tracking_code>ABC12345</tracking_code><tracking_link>https://www.fraktjakt.se/trace/shipment/b6dfc12fc04</tracking_link><agent_selection_link>https://www.fraktjakt.se/agents/search_closest</agent_selection_link>
<shipping_products><shipping_product><id>15</id><name>Privat</name><description>Bussgods - Privat</description><arrival_time>1-2 dagar</arrival_time><price>159.50</price><tax_class>25.00</tax_class><insurance_fee>75.00</insurance_fee>
<insurance_tax_class>0</insurance_tax_class><to_agent>1</to_agent><agent_info>Cityterminalen Stockholm ca 2 km i Stockholm</agent_info><agent_link>https://www.fraktjakt.se/agents/search_closest</agent_link>
<agent_in_info>Jönköping Bussgods ca 1 km i Jönköping</agent_in_info><agent_in_link>https://www.fraktjakt.se/agents/search_closest/377482?type=8&amp;shipper=4</agent_in_link>
<service_point_locator_api>https://www.fraktjakt.se/agents/service_point_locator</service_point_locator_api><shipper><id>4</id><name>Bussgods</name><logo_url>https://www.fraktjakt.se/images/shippers/4.png</logo_url></shipper></shipping_product>
</shipping_products></shipment>"));
        
        // Act
        var response = await _fraktjaktClient.ReQuery(shipment);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response as QueryResponse, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            Assert.That(((QueryResponse)response).Products.ToList(), Has.Count.EqualTo(1));
        });
    }

    [Test]
    public async Task ReQuery_Returns_ErrorResponse_On_NonSuccessfullStatus()
    {
        // Arrange
        var shipment = new ShipmentReQuery(_fraktjaktClient.Sender, 123);
        SetupMessageHandlerForBadRequestResponse();

        // Act
        var response = await _fraktjaktClient.ReQuery(shipment);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successful response"));
        });
    }

    [Test]
    public async Task Order_ValidInput_ReturnsResponse()
    {
        // Arrange
        var order = new DirectOrder(_fraktjaktClient.Sender, new ToAddress("12345"), 123, items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
        SetupMessageHandlerForSuccessResponse(new StringContent(@$"<?xml version=""1.0"" encoding=""UTF-8""?><result><server_status>ok</server_status><status>ok</status><code>0</code><warning_message></warning_message><error_message></error_message>
<shipment_id>45654</shipment_id><access_code>ABC12345</access_code><access_link>https://www.fraktjakt.se/shipments/show/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</access_link>
<return_link>https://www.fraktjakt.se/shipments/xml_create_return/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</return_link><cancel_link>https://www.fraktjakt.se/shipments/xml_cancel/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</cancel_link>
<tracking_code>ABC12345</tracking_code><tracking_link>https://www.fraktjakt.se/trace/shipment/b6dfc12fc04</tracking_link><amount>1066.33</amount><currency>SEK</currency>
<payment_link>https:// www.fraktjakt.se/orders/finish/1631?code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</payment_link><sender_email_link>https://www.fraktjakt.se/orders/confirmation/1671?key=cabec52762637412652e5023deb5e5c2</sender_email_link>
<agent_info>Cityterminalen Stockholm ca 2 km i Stockholm</agent_info><agent_link>https://www.fraktjakt.se/agents/search_closest</agent_link><service_point_locator_api>https://www.fraktjakt.se/agents/service_point_locator</service_point_locator_api></result>"));

        // Act
        var response = await _fraktjaktClient.Order(order);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response as OrderResponse, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            //Binding from xml should be tested in the OrderResponseTests
        });
    }

    [Test]
    public async Task Order_Returns_ErrorResponse_On_NonSuccessfullStatus()
    {
        // Arrange
        var order = new DirectOrder(_fraktjaktClient.Sender, new ToAddress("12345"), 123, items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
        SetupMessageHandlerForBadRequestResponse();

        // Act
        var response = await _fraktjaktClient.Order(order);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successful response"));
        });
    }

    [Test]
    public async Task CreateShipment_ValidInput_ReturnsResponse()
    {
        // Arrange
        var createShipment = new CreateShipment(_fraktjaktClient.Sender, new ToAddress("12345"), new Recipient() { CompanyName = "company"}, items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
        SetupMessageHandlerForSuccessResponse(new StringContent(@$"<?xml version=""1.0"" encoding=""UTF-8""?><result><server_status>ok</server_status><status>ok</status><code>0</code><warning_message></warning_message><error_message></error_message>
<shipment_id>45654</shipment_id><access_code>ABC12345</access_code><access_link>https://www.fraktjakt.se/shipments/show/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</access_link>
<return_link>https://www.fraktjakt.se/shipments/xml_create_return/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</return_link>
<cancel_link>https://www.fraktjakt.se/shipments/xml_cancel/163221?access_code=b6dfc12fc04ec98132da2eb1c1739272cc646ed9</cancel_link>
<tracking_code>ABC12345</tracking_code><tracking_link>https://www.fraktjakt.se/trace/shipment/b6dfc12fc04</tracking_link></result>"));

        // Act
        var response = await _fraktjaktClient.CreateShipment(createShipment);

        // Assert
        Assert.Multiple(() => {
            Assert.That(response as CreateShipmentResponse, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Ok));
            //Binding from xml should be tested in the OrderResponseTests
        });
    }

    [Test]
    public async Task CreateShipment_Returns_ErrorResponse_On_NonSuccessfullStatus()
    {
        // Arrange
        var createShipment = new CreateShipment(_fraktjaktClient.Sender, new ToAddress("12345"), new Recipient() { CompanyName = "company" }, items: new[] { new ShipmentItem("Item", 1, 10.0f, 1.0f) });
        SetupMessageHandlerForBadRequestResponse();

        // Act
        var response = await _fraktjaktClient.CreateShipment(createShipment);


        // Assert
        Assert.Multiple(() => {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Error));
            Assert.That(response.ErrorMessage, Does.StartWith("Not successful response"));
        });
    }



    [TearDown]
    public void Cleanup()
    {
        _fraktjaktClient.Dispose();
    }    


    private void SetupMessageHandlerForSuccessResponse(StringContent contentToReturn)
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = contentToReturn
            });

    }

    private void SetupMessageHandlerForBadRequestResponse()
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Error")
            });

    }


}
