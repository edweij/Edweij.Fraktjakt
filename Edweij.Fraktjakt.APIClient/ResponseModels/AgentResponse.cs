using Edweij.Fraktjakt.APIClient.responseModels;
using System.Text.Json;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record AgentResponse(float Distance, string HtmlInfo, int ShipperId, float Latitude,
    float Longitude, string Name, string Shipper, int Id, AddressResponse Address)
{
    string? AgentOperationHours { get; set; } = null;

    public static IEnumerable<AgentResponse> FromJson(JsonElement json)
    {
        var result = new List<AgentResponse>();

        foreach (JsonElement agentElement in json.EnumerateArray())
        {
            float distance = agentElement.GetProperty("distance").GetSingle();
            string htmlInfo = agentElement.GetProperty("html_info").GetString() ?? string.Empty;
            int shipperId = agentElement.GetProperty("shipper_id").GetInt32();
            float latitude = agentElement.GetProperty("latitude").GetSingle();
            float longitude = agentElement.GetProperty("longitude").GetSingle();
            string name = agentElement.GetProperty("name").GetString() ?? string.Empty;
            string shipper = agentElement.GetProperty("shipper").GetString() ?? string.Empty;
            string agentOperationHours = agentElement.GetProperty("agent_operation_hours").GetString() ?? string.Empty;
            int id = agentElement.GetProperty("id").GetInt32();
            
            JsonElement addressElement = agentElement.GetProperty("address");
            string postalCode = addressElement.GetProperty("postal_code").GetString() ?? string.Empty;
            string street = addressElement.GetProperty("street").GetString() ?? string.Empty;
            string city = addressElement.GetProperty("city").GetString() ?? string.Empty;
            string country = addressElement.GetProperty("country").GetString() ?? string.Empty;

            result.Add(new AgentResponse(distance, htmlInfo, shipperId, latitude, longitude, name, shipper, id, new AddressResponse(postalCode, street, city, country)) { AgentOperationHours = agentOperationHours });
        }

        return result;
    }

}
