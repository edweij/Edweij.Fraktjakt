using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient;

public interface IFraktjaktClient
{
    Task<Response> Trace(int shipmentId, SwedishOrEnglish lang);
    Task<Response> ShippingDocuments(int shipmentId, SwedishOrEnglish lang);
    Task<Response> Query(ShipmentQuery shipment);
    Task<Response> ReQuery(ShipmentReQuery shipment);
    Task<Response> ReQuery(int shipmentId, bool shipperInfo, float? value);
    Task<Response> Order(Order order);
    Task<Response> CreateShipment(CreateShipment createShipment);
    Task<Response> GetServicePoints(string url);
}
