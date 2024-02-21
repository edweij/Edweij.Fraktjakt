using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record ShippingDocumentsResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, IEnumerable<ShippingDocument> Documents)
    : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
{

    public static async Task<Response> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return CreateErrorResponse($"Not successfull response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    public static Response FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var documents = new List<ShippingDocument>();
            if (element.Element("shipping_documents") != null && element.Element("shipping_documents")!.HasElements)
            {
                documents.AddRange(element.Element("shipping_documents")!.Elements("shipping_document").Select(ShippingDocument.FromXml));
            }
            var result = new ShippingDocumentsResponse(element.Element("server_status")!.Value,
                (ResponseStatus)int.Parse(element.Element("code")!.Value),
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                documents);
            return result;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

}
