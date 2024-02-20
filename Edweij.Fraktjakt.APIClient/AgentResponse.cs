﻿using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient
{
    public record AgentListResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, IEnumerable<AgentResponse> Agents) : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
    {
        public static async Task<AgentListResponse> FromHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (!httpResponseMessage.IsSuccessStatusCode) throw new ArgumentException($"Not successfull response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
            string json = await httpResponseMessage.Content.ReadAsStringAsync();
            return FromJson(json);
        }

        public static AgentListResponse FromJson(string json)
        {
            //json = @"{""status"":""ok"",""agents"":[{""distance"":2.05,""html_info"":""DHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.622806,""longitude"":18.321997,""name"":""STORA COOP VISBY"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395522,""address"":{""postal_code"":""62153"",""street"":""STENHUGGARV\u00c4GEN 5"",""city"":""VISBY"",""country"":""SE""}},{""distance"":2.37,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.638641,""longitude"":18.300478,""name"":""OKQ8 VISBY"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395523,""address"":{""postal_code"":""62145"",""street"":""KUNG MAGNUS V\u00c4G 12"",""city"":""VISBY"",""country"":""SE""}},{""distance"":4.49,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.61736,""longitude"":18.2843,""name"":""ICA N\u00c4RA WISBORG"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395524,""address"":{""postal_code"":""62149"",""street"":""STENKUMLAV\u00c4G 36"",""city"":""VISBY"",""country"":""SE""}},{""distance"":17.5,""html_info"":""Postf\u00f6rskott / COD"",""shipper_id"":5,""latitude"":57.505343,""longitude"":18.4549,""name"":""PRESSBYR\u00c5N ROMA KLOSTER"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395521,""address"":{""postal_code"":""62254"",""street"":""VISBYV\u00c4GEN 33"",""city"":""ROMAKLOSTER"",""country"":""SE""}},{""distance"":23.07,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.79748,""longitude"":18.50295,""name"":""TEMPO STENKYRKA"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395526,""address"":{""postal_code"":""62442"",""street"":""STENKYRKA H\u00c4LGE"",""city"":""TINGST\u00c4DE"",""country"":""SE""}},{""distance"":33.63,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.704118,""longitude"":18.798027,""name"":""COOP KONSUM SLITE"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395525,""address"":{""postal_code"":""62448"",""street"":""TULLHAGSPLAN 5"",""city"":""SLITE"",""country"":""SE""}},{""distance"":34.2,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.38747,""longitude"":18.19977,""name"":""KLINTE SN\u00c4USBODI"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395528,""address"":{""postal_code"":""62377"",""street"":""NORRA KUSTV\u00c4GEN 10"",""city"":""KLINTEHAMN"",""country"":""SE""}},{""distance"":35.66,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.78488,""longitude"":18.7851,""name"":""COOP KONSUM L\u00c4RBRO"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395527,""address"":{""postal_code"":""62452"",""street"":""LINDV\u00c4GEN 1"",""city"":""L\u00c4RBRO"",""country"":""SE""}},{""distance"":44.01,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.336435,""longitude"":18.697781,""name"":""LJUGARNS HANDEL"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395529,""address"":{""postal_code"":""62365"",""street"":""LANTMANNAV\u00c4GEN 1"",""city"":""LJUGARN"",""country"":""SE""}},{""distance"":50.06,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.23736,""longitude"":18.37371,""name"":""F\u00c4RGHUSET HEMSE"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395530,""address"":{""postal_code"":""62350"",""street"":""STORGATAN 46C"",""city"":""HEMSE"",""country"":""SE""}},{""distance"":54.46,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.86309,""longitude"":19.055236,""name"":""ICA SUPERMARKET BUNGEHALLEN"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395531,""address"":{""postal_code"":""62462"",""street"":""F\u00c5R\u00d6V\u00c4GEN 10"",""city"":""F\u00c5R\u00d6SUND"",""country"":""SE""}},{""distance"":76.19,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.032285,""longitude"":18.277343,""name"":""GULF BURGSVIK"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395532,""address"":{""postal_code"":""62335"",""street"":""HOBURGSV\u00c4GEN 35"",""city"":""BURGSVIK"",""country"":""SE""}},{""distance"":138.14,""html_info"":""Postf\u00f6rskott / COD"",""shipper_id"":5,""latitude"":57.37495,""longitude"":16.49044,""name"":""COOP F\u00c5RBO"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395533,""address"":{""postal_code"":""57276"",""street"":""FICKSJ\u00d6V\u00c4GEN 40"",""city"":""F\u00c5RBO"",""country"":""SE""}},{""distance"":183.87,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.72119,""longitude"":16.53124,""name"":""HANDLAR'N GUNNEBO"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395534,""address"":{""postal_code"":""59375"",""street"":""CENTRUMV\u00c4GEN 23"",""city"":""GUNNEBO"",""country"":""SE""}},{""distance"":191.16,""html_info"":""Postf\u00f6rskott / COD\u003Cbr /\u003EDHL Express utl\u00e4mning / pickup\u003Cbr /\u003EDHL Express inl\u00e4mning / dropoff"",""shipper_id"":5,""latitude"":57.76338,""longitude"":16.60102,""name"":""XL BYGG MATERIALM\u00c4NNEN"",""shipper"":""DHL Freight"",""agent_operation_hours"":null,""id"":395535,""address"":{""postal_code"":""59362"",""street"":""FOLKPARKSV\u00c4GEN 60"",""city"":""V\u00c4STERVIK"",""country"":""SE""}}],""code"":0}";
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);
            return new AgentListResponse(jsonObject.status.ToString(), (ResponseStatus)jsonObject.code, "", "", AgentResponse.FromJson(json));
        }
    }

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
