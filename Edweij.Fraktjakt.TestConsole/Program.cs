using Edweij.Fraktjakt.APIClient;
using Edweij.Fraktjakt.APIClient.Enums;
using Edweij.Fraktjakt.APIClient.RequestModels;
using Edweij.Fraktjakt.APIClient.ResponseModels;
using Microsoft.Extensions.Configuration;

namespace Edweij.Fraktjakt.TestConsole
{
    internal class Program
    {
        static async Task Main()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("secrets.json");
            var configuration = builder.Build();
            int id = int.Parse(configuration["Id"] ?? "0");
            string key = configuration["Key"] ?? string.Empty;

            //var sender = new Sender(id, key)
            //{
            //    Currency = "SEK",
            //    Language = "sv",
            //    SystemName = "Edweij.Fraktjakt"
            //};
            //var client = new FraktjaktClient(sender);
            //var toaddress = new ToAddress("62141")
            //{
            //    CityName = "Visby",
            //    CountryCode = "SE",
            //    IsResidental = true,
            //    StreetAddress1 = "Skarphällsgatan 13"
            //};
            //var shipment = new Query(sender, toaddress)
            //{
            //    ShipperInfo = true
            //};

            //shipment.AddShipmentItem(new ShipmentItem("Litteratur, konst och politik i välfärdsstatens Sverige", 4, 100f, 2.112f)
            //{
            //    ArticleNumber = "9789188763518",
            //    CountryOfManufacture = "SE",
            //    Currency = "SEK",
            //    UnitLength = 22.5f,
            //    UnitWidth = 15.5f,
            //    UnitHeight = 2.0f,
            //    ShelfPosition = "Ej angett"
            //});
            //Console.WriteLine($"Before query: {DateTime.Now}");
            //var response = await client.Query(shipment);
            //Console.WriteLine($"After query: {DateTime.Now}");
            //if (response.ResponseStatus == ResponseStatus.Ok &&  response is QueryResponse shipmentResponse)
            //{                
            //    var product = shipmentResponse.Products.FirstOrDefault(p => !string.IsNullOrEmpty(p.ServicePointLocatorApi));
            //    if (product != null)
            //    {
            //        Console.WriteLine($"Before GetServicePoints: {DateTime.Now}");
            //        var agentsResponse = await client.GetServicePoints(product.ServicePointLocatorApi!);
            //        Console.WriteLine($"After GetServicePoints: {DateTime.Now}");
            //        if (agentsResponse.ResponseStatus == ResponseStatus.Ok && agentsResponse is AgentListResponse)
            //        {
            //            var agents = agentsResponse as AgentListResponse;
            //            Console.WriteLine($"Got service points: {agents!.Agents.Count()}");
            //            int agentId = agents.Agents.First().Id;
            //        }
            //    }
                
            //}
            var k = Console.ReadKey();
        }
    }
}