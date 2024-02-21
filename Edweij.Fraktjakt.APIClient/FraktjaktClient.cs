using System.Web;

namespace Edweij.Fraktjakt.APIClient
{
    public class FraktjaktClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _useMD5Checksum = true;

        public Sender Sender { get; init; }


        public FraktjaktClient(int id, string key, bool useMD5Checksum = true) {
            if (id <= 0) throw new ArgumentException(nameof(id));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.fraktjakt.se");
            Sender = new Sender(id, key);
            _useMD5Checksum = useMD5Checksum;
        }

        public FraktjaktClient(Sender sender, bool useMD5Checksum = true)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (!sender.IsValid) throw new ArgumentException("Sender is not valid");

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.fraktjakt.se");
            Sender = sender;
            _useMD5Checksum = useMD5Checksum;
        }

        public async Task<HttpResponseMessage> Trace(int shipmentId, SwedishOrEnglish lang)
        {
            var url = $"/trace/xml_trace?consignor_id={Sender.Id}&consignor_key={Sender.Key}&shipment_id={shipmentId}&locale={lang}";
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> ShippingDocuments(int shipmentId, SwedishOrEnglish lang)
        {
            var url = $"/shipping_documents/xml_get?consignor_id={Sender.Id}&consignor_key={Sender.Key}&shipment_id={shipmentId}&locale={lang}";
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> Query(ShipmentQuery shipment)
        {
            if (shipment == null) throw new ArgumentNullException(nameof(shipment));            
            if (!shipment.IsValid) throw new ArgumentException("Shipment is not valid");
            if (shipment.Sender != Sender) throw new ArgumentException("Sender in shipment is different from the clients sender");

            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + shipment.ToXml();
            var url = $"/fraktjakt/query_xml?xml={UrlEncode(xml)}";
            if (_useMD5Checksum)
            {
                url += $"&md5_checksum={MD5(xml)}";
            }
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> Order(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (!order.IsValid) throw new ArgumentException("Order is not valid");
            if (order.Sender != Sender) throw new ArgumentException("Sender in order is different from the clients sender");

            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + order.ToXml();
            var url = $"/orders/order_xml?xml={UrlEncode(xml)}";
            if (_useMD5Checksum)
            {
                url += $"&md5_checksum={MD5(xml)}";
            }
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> CreateShipment(CreateShipment createShipment)
        {
            if (createShipment == null) throw new ArgumentNullException(nameof(createShipment));
            if (!createShipment.IsValid) throw new ArgumentException("createShipment is not valid");
            if (createShipment.Sender != Sender) throw new ArgumentException("Sender in createShipment is different from the clients sender");

            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + createShipment.ToXml();
            var url = $"/shipments/shipment_xml?xml={UrlEncode(xml)}";
            if (_useMD5Checksum)
            {
                url += $"&md5_checksum={MD5(xml)}";
            }
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> GetServicePoints(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));
            return await _httpClient.GetAsync(url);            
        }

        public string UrlEncode(string input)
        {
            return HttpUtility.UrlEncode(input);
        }

        public string MD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}