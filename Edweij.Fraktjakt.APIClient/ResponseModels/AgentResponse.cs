using System.Text.Json;
using System.Text.Json.Serialization;

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
    BusinessHours? BusinessHours { get; init; } = null;

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
            int id = agentElement.GetProperty("id").GetInt32();

            JsonElement addressElement = agentElement.GetProperty("address");
            string postalCode = addressElement.GetProperty("postal_code").GetString() ?? string.Empty;
            string street = addressElement.GetProperty("street").GetString() ?? string.Empty;
            string city = addressElement.GetProperty("city").GetString() ?? string.Empty;
            string country = addressElement.GetProperty("country").GetString() ?? string.Empty;

            result.Add(new AgentResponse(distance, htmlInfo, shipperId, latitude, longitude, name, shipper, id,
                new AddressResponse(postalCode, street, city, country))
            {
                BusinessHours = JsonSerializer.Deserialize<BusinessHours>(agentElement.GetProperty("agent_operation_hours"))
            });
        }

        return result;
    }   

}

/// <summary>
/// Time of day the agent is opening and closing
/// </summary>
public class OpeningHours
{
    /// <summary>
    /// The time the agent opens as string e.g. 07:00
    /// </summary>
    [JsonPropertyName("open")]
    public string Open { get; set; } = string.Empty;

    /// <summary>
    /// The time the agent closes as string e.g. 19:00
    /// </summary>
    [JsonPropertyName("close")]
    public string Close { get; set; } = string.Empty;
}

/// <summary>
/// Array with openinghours for a certain weekday
/// </summary>
public class Overrides
{
    /// <summary>
    /// Weekdays as string e.g. fri with a list of opening hours for that weekday
    /// </summary>
    [JsonPropertyName("weekday")]
    public Dictionary<string, List<OpeningHours>> Weekday { get; set; } = new Dictionary<string, List<OpeningHours>>();
}

/// <summary>
/// The Business hours for the agent
/// </summary>
public class BusinessHours
{
    /// <summary>
    /// Array with default opening hours
    /// </summary>
    [JsonPropertyName("_default")]
    public List<object> Default { get; set; } = new List<object>();

    /// <summary>
    /// Weekdays as string e.g. fri with a list of opening hours for that weekday
    /// </summary>
    [JsonPropertyName("overrides")]
    public Overrides? Overrides { get; set; } = null;
}