﻿using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents a response containing a collection of shipping documents.
/// </summary>
/// <param name="Documents">Array of <see cref="ShippingDocument"/></param>
public record ShippingDocumentsResponse(IEnumerable<ShippingDocument> Documents)
{
    /// <summary>
    /// Creates an instance of <see cref="ShippingDocumentsResponse"/> from an HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message to parse.</param>
    /// <returns>An instance of <see cref="ShippingDocumentsResponse"/> representing the parsed response.</returns>
    public static async Task<Response<ShippingDocumentsResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<ShippingDocumentsResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<ShippingDocumentsResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    /// <summary>
    /// Creates an instance of <see cref="ShippingDocumentsResponse"/> from an XML representation.
    /// </summary>
    /// <param name="xml">The XML representation to parse.</param>
    /// <returns>An instance of <see cref="ShippingDocumentsResponse"/> representing the parsed response.</returns>
    public static Response<ShippingDocumentsResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var status = (ResponseStatus)int.Parse(element.Element("code")!.Value);

            if (status == ResponseStatus.Error)
            {
                return Response<ShippingDocumentsResponse>.CreateErrorResponseFromXml(element);
            }


            var documents = new List<ShippingDocument>();
            if (element.Element("shipping_documents") != null && element.Element("shipping_documents")!.HasElements)
            {
                documents.AddRange(element.Element("shipping_documents")!.Elements("shipping_document").Select(ShippingDocument.FromXml));
            }
            var result = new Response<ShippingDocumentsResponse>(element.Element("server_status")!.Value,
                status,
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                new ShippingDocumentsResponse(documents));
            return result;
        }
        catch (Exception ex)
        {
            return Response<ShippingDocumentsResponse>.CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

}
