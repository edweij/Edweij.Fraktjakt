using Edweij.Fraktjakt.APIClient.Enums;
using System.Text.Json;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents a response containing a list of agents.
/// </summary>
public record AgentListResponse(IEnumerable<AgentResponse> Agents)
{
    /// <summary>
    /// Creates an instance of <see cref="Response{AgentListResponse}"/> from an HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
    /// <returns>An instance of <see cref="Response{AgentListResponse}"/> representing the parsed response.</returns>
    public static async Task<Response<AgentListResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<AgentListResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<AgentListResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string json = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromJson(json);
    }

    /// <summary>
    /// Creates an instance of <see cref="Response{AgentListResponse}"/> from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string to parse.</param>
    /// <returns>An instance of <see cref="Response{AgentListResponse}"/> representing the parsed response.</returns>
    public static Response<AgentListResponse> FromJson(string json)
    {        
        try
        {
            JsonDocument document = JsonDocument.Parse(json);
            var root = document.RootElement;

            
            string serverStatus = root.GetProperty("status").GetString() ?? string.Empty;
            var  responseStatus = (ResponseStatus)root.GetProperty("code").GetInt32();
            JsonElement agentsElement = root.GetProperty("agents");
            return new Response<AgentListResponse>(serverStatus, responseStatus, string.Empty, string.Empty, new AgentListResponse(AgentResponse.FromJson(agentsElement)));
        }
        catch (Exception ex)
        {
            return Response<AgentListResponse>.CreateErrorResponse($"Error parsing JSON: {ex.Message}");
            
        }
    }
}
