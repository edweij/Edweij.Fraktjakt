using Edweij.Fraktjakt.APIClient.Enums;
using System.Text.Json;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record AgentListResponse(IEnumerable<AgentResponse> Agents)
{
    public static async Task<Response<AgentListResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<AgentListResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<AgentListResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string json = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromJson(json);
    }

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
