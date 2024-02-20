using System.Globalization;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient
{
    public record TraceResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, string TrackingCode, string TrackingLink, IEnumerable<ShippingState> ShippingStates)
        : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
    {

        public string? TrackingNumber { get; init; }
        public string? ShippingCompany { get; init; }
        public IEnumerable<string>? ShippingDocuments { get; init; }

        public static async Task<Response> FromHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null) return UnbindableResponse("HttpResponseMessage was null");
            if (!httpResponseMessage.IsSuccessStatusCode) return UnbindableResponse($"Not successfull response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
            string xml = await httpResponseMessage.Content.ReadAsStringAsync();
            return FromXml(xml);
        }

        public static Response FromXml(string xml)
        {
            try
            {
                XElement element = XElement.Parse(xml);
                var states = new List<ShippingState>();
                if (element.Element("shipping_states") != null && element.Element("shipping_states")!.HasElements)
                {
                    states.AddRange(element.Element("shipping_states")!.Elements("shipping_state").Select(ShippingState.FromXml));                    
                }
                
                var result = new TraceResponse(element.Element("server_status")!.Value,
                    (ResponseStatus)int.Parse(element.Element("code")!.Value),
                    element.Element("warning_message")!.Value,
                    element.Element("error_message")!.Value,
                    element.Element("tracking_code")!.Value,
                    element.Element("tracking_link")!.Value,
                    states)
                { 
                    TrackingNumber = element.Element("tracking_number") != null ? element.Element("tracking_number")!.Value : null,
                    ShippingCompany = element.Element("shipping_company") != null ? element.Element("shipping_company")!.Value : null,
                    ShippingDocuments = element.Element("shipping_documents") != null ? element.Element("shipping_documents")!.Elements("shipping_document").Select(e => e.Value) : null,
                };               
                
                return result;
            }
            catch (Exception ex)
            {
                return UnbindableResponse($"Invalid xml: {ex.Message}");
            }
        }

    }

    public record ShippingState(int ShipmentId, string Name, ShippingStateId StateId, FraktjaktStateId FraktjaktStateId)
    {
        public static ShippingState FromXml(XElement element)
        {
            return new ShippingState(int.Parse(element.Element("shipment_id")!.Value),
                element.Element("name")!.Value,
                int.Parse(element.Element("id")!.Value),
                int.Parse(element.Element("fraktjakt_id")!.Value));
        }
    }

    public readonly struct ShippingStateId
    {
        public int Id { get; init; }

        public ShippingStateId()
        {
            Id = 0;
        }

        public ShippingStateId(int id)
        {
            if (!validids.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
            }
            this.Id = id;
        }
        public static implicit operator ShippingStateId(int id)
        {
            return new ShippingStateId(id);
        }

        public override string ToString() => validids[Id];

        private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
            { 0, "Hanteras av transportören"},
            { 1, "Avsänt"},
            { 2, "Levererat"},
            { 3, "Kvitterats"},
            { 4, "Retur"}
        };
    }

    public readonly struct FraktjaktStateId
    {
        public int Id { get; init; }

        public FraktjaktStateId()
        {
            Id = 0;
        }

        public FraktjaktStateId(int id)
        {
            if (!validids.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
            }
            this.Id = id;
        }
        public static implicit operator FraktjaktStateId(int id)
        {
            return new FraktjaktStateId(id);
        }

        public override string ToString() => validids[Id];

        private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
            { 0, "Obetald"},
            { 1, "Förberedande"},
            { 2, "not used state"},
            { 3, "Betald"},
            { 4, "Avsänt"},
            { 5, "Levererat"},
            { 6, "Kvitterats"},
            { 7, "Retur"},
            { 12, "Hanteras av transportören"},
            { 17, "Rättas"},
            { 18, "Väntande"},
            { 19, "Söks"}
        };
    }


}
