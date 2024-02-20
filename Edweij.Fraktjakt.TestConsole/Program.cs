using Edweij.Fraktjakt.APIClient;

namespace Edweij.Fraktjakt.TestConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int id = 37195;
            string key = "fd23c28a493774889c406ea1f3122fa476eb76f8";
            var sender = new Sender(id, key)
            {
                Currency = "SEK",
                Language = Language6391.sv,
                SystemName = "Edweij.Fraktjakt"
            };
            var client = new Client(sender);
            var toaddress = new ToAddress
            {
                CityName = "Visby",
                CountryCode = "SE",
                IsResidental = true,
                PostalCode = "62141",
                StreetAddress1 = "Skarphällsgatan 13"
            };
            var shipment = new ShipmentQuery(sender, toaddress);
            shipment.ShipperInfo = true;
            
            shipment.AddShipmentItem(new ShipmentItem
            {
                ArticleNumber = "9789188763518",
                CountryOfManufacture = "SE",
                Currency = "SEK",
                Name = "Litteratur, konst och politik i välfärdsstatens Sverige",
                Quantity = 4,
                TotalWeight = 2.112f,
                UnitLength = 22.5f,
                UnitWidth = 15.5f,
                UnitHeight = 2.0f,
                UnitPrice = 100f,
                ShelfPosition = "Ej angett"
            });
            Console.WriteLine($"Before query: {DateTime.Now}");
            var response = await ShipmentResponse.FromHttpResponse(await client.Query(shipment));
            Console.WriteLine($"After query: {DateTime.Now}");
            if (response.ResponseStatus != ResponseStatus.Error &&  response is ShipmentResponse)
            {                
                var product = ((ShipmentResponse)response).Products.FirstOrDefault(p => !string.IsNullOrEmpty(p.ServicePointLocatorApi));
                if (product != null)
                {
                    Console.WriteLine($"Before GetServicePoints: {DateTime.Now}");
                    var agents = await AgentListResponse.FromHttpResponse(await client.GetServicePoints(product.ServicePointLocatorApi!));
                    Console.WriteLine($"After GetServicePoints: {DateTime.Now}");
                    if (agents.ResponseStatus == ResponseStatus.Ok)
                    {
                        Console.WriteLine($"Got service points: {agents.Agents.Count()}");
                        int agentId = agents.Agents.First().Id;
                    }
                }
                
            }
            var k = Console.ReadKey();
        }
    }
}