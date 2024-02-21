using Newtonsoft.Json;

namespace Edweij.Fraktjakt.APIClient
{


    public record AgentResponse(float Distance, string HtmlInfo, int ShipperId, float Latitude, 
        float Longitude, string Name, string Shipper, int Id, AddressResponse Address)
    {
        string? AgentOperationHours { get; set; } = null;
        
        public static IEnumerable<AgentResponse> FromJson(string json)
        {
            var agents = new List<AgentResponse>();
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
            if (jsonObject != null && jsonObject!.agents != null && jsonObject!.agents.Count > 0)
            {
                foreach (var a in jsonObject!.agents)
                {
                    CountryCode cc = a.address.country.ToString();
                    var address = new AddressResponse(a.address.postal_code.ToString(), a.address.street.ToString(), a.address.city.ToString(), cc);
                    var agent = new AgentResponse((float)a.distance, a.html_info.ToString(), (int)a.shipper_id, (float)a.latitude, (float)a.longitude, a.name.ToString(), a.shipper.ToString(), (int)a.id, address);
                    agent.AgentOperationHours = a.agent_operation_hours;
                    agents.Add(agent);
                }
            }
            
            return agents;
        }

    }

    
}
