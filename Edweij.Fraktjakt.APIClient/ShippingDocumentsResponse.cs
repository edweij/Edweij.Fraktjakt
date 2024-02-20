﻿using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient
{
    public record ShippingDocumentsResponse(string ServerStatus, ResponseStatus ResponseStatus, string WarningMessage, string ErrorMessage, IEnumerable<ShippingDocument> Documents)
        : Response(ServerStatus, ResponseStatus, WarningMessage, ErrorMessage)
    {

        public static async Task<Response> FromHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null) return UnbindableResponse("HttpResponseMessage was null");
            if (!httpResponseMessage.IsSuccessStatusCode) return UnbindableResponse($"Not successfull response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
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
                return UnbindableResponse($"Invalid xml: {ex.Message}");
            }
        }

    }

    public record ShippingDocument(string Name, ShippingDocumentTypeId TypeId, string TypeName, string TypeDescription, string StateName, string StateDescription, string FormatName, string File)
    {
        public static ShippingDocument FromXml(XElement element)
        {
            return new ShippingDocument(element.Element("name")!.Value,
                int.Parse(element.Element("type_id")!.Value), 
                element.Element("type_name")!.Value, 
                element.Element("type_description")!.Value, 
                element.Element("state_name")!.Value, 
                element.Element("state_description")!.Value, 
                element.Element("format_name")!.Value,
                element.Element("file")!.Value);
        }

        public byte[] PdfFromBase64()
        {
            byte[] byteArray = Convert.FromBase64String(File);
                return byteArray;
        }
    }

    public readonly struct ShippingDocumentTypeId
    {
        public int Id { get; init; }

        public ShippingDocumentTypeId()
        {
            Id = 0;
        }

        public ShippingDocumentTypeId(int id)
        {
            if (!validids.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
            }
            this.Id = id;
        }
        public static implicit operator ShippingDocumentTypeId(int id)
        {
            return new ShippingDocumentTypeId(id);
        }

        public override string ToString() => validids[Id];

        private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
            { 1, "Pro Forma-faktura"},
            { 2, "Handelsfaktura"},
            { 3, "Fraktetikett"},
            { 4, "Fraktsedel"},
            { 5, "Sändningslista"},
            { 10, "Följesedel"},
            { 11, "CN22"},
            { 12, "CN23"},
            { 13, "Säkerhetsdeklaration"}
        };
    }

}