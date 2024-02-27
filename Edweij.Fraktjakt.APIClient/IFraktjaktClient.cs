using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using Edweij.Fraktjakt.APIClient.Structs;

namespace Edweij.Fraktjakt.APIClient;

public interface IFraktjaktClient
{
    Task<Response<TraceResponse>> Trace(int shipmentId, SwedishOrEnglish lang);
    Task<Response<ShippingDocumentsResponse>> ShippingDocuments(int shipmentId, SwedishOrEnglish lang);
    Task<Response<QueryResponse>> Query(Query shipment);
    Task<Response<QueryResponse>> ReQuery(ReQuery shipment);
    Task<Response<QueryResponse>> ReQuery(int shipmentId, bool shipperInfo, float? value);
    Task<Response<OrderResponse>> Order(Order order);
    Task<Response<CreateShipmentResponse>> CreateShipment(CreateShipment createShipment);
    Task<Response<AgentListResponse>> GetServicePoints(string url);
    Sender Sender { get; }
}
