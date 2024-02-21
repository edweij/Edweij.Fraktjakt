using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient;

public interface IFraktjaktClient
{
    public Task<Response> Trace(int shipmentId, SwedishOrEnglish lang);
    public Task<Response> ShippingDocuments(int shipmentId, SwedishOrEnglish lang);
    public Task<Response> Query(ShipmentQuery shipment);
    public Task<Response> Order(Order order);
    public Task<Response> CreateShipment(CreateShipment createShipment);
    public Task<Response> GetServicePoints(string url);
}
