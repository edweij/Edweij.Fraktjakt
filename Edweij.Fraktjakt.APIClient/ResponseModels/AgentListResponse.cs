using Edweij.Fraktjakt.APIClient.Enums;
using System.Text.Json;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record AgentListResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, IEnumerable<AgentResponse> Agents) : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
{
    public static async Task<Response> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string json = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromJson(json);
    }

    public static Response FromJson(string json)
    {        
        try
        {
            JsonDocument document = JsonDocument.Parse(json);
            var root = document.RootElement;

            
            string serverStatus = root.GetProperty("status").GetString() ?? string.Empty;
            var  responseStatus = (ResponseStatus)root.GetProperty("code").GetInt32();
            JsonElement agentsElement = root.GetProperty("agents");
            return new AgentListResponse(serverStatus, responseStatus, string.Empty, string.Empty, AgentResponse.FromJson(agentsElement));            
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error parsing JSON: {ex.Message}");
            
        }
    }
}
