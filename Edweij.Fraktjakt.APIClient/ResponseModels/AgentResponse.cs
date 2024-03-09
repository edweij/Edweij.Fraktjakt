using System.Text.Json;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents a response containing information about an agent.
/// </summary>
public record AgentResponse(float Distance, string HtmlInfo, int ShipperId, float Latitude,
    float Longitude, string Name, string Shipper, int Id, AddressResponse Address)
{
    /// <summary>
    /// Gets or sets the operation hours of the agent.
    /// </summary>
    string? AgentOperationHours { get; set; } = null;

    /// <summary>
    /// Creates a collection of <see cref="AgentResponse"/> instances from a JSON element.
    /// </summary>
    /// <param name="json">The JSON element to parse.</param>
    /// <returns>A collection of <see cref="AgentResponse"/> instances.</returns>
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
