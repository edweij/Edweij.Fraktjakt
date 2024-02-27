﻿using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

public record TraceResponse(string TrackingCode, string TrackingLink, IEnumerable<ShippingState> ShippingStates)
{

    public string? TrackingNumber { get; init; }
    public string? ShippingCompany { get; init; }
    public IEnumerable<string>? ShippingDocuments { get; init; }

    public static async Task<Response<TraceResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<TraceResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<TraceResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    public static Response<TraceResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);            

            var result = new Response<TraceResponse>(element.Element("server_status")!.Value,
                (ResponseStatus)int.Parse(element.Element("code")!.Value),
                element.Element("warning_message")!.Value,
                element.Element("error_message")!.Value,
                TraceResponseFromXml(element));

            return result;
        }
        catch (Exception ex)
        {
            return Response<TraceResponse>.CreateErrorResponse($"Invalid xml: {ex.Message}");
        }
    }

    private static TraceResponse TraceResponseFromXml(XElement element)
    {
        var traceResponse = new TraceResponse(
            element.Element("tracking_code")!.Value,
            element.Element("tracking_link")!.Value,
            ShippingStatesFromXml(element))
        {
            TrackingNumber = element.Element("tracking_number") != null ? element.Element("tracking_number")!.Value : null,
            ShippingCompany = element.Element("shipping_company") != null ? element.Element("shipping_company")!.Value : null,
            ShippingDocuments = element.Element("shipping_documents") != null ? element.Element("shipping_documents")!.Elements("shipping_document").Select(e => e.Value) : null,
        };
        return traceResponse;
    }

    private static IEnumerable<ShippingState> ShippingStatesFromXml(XElement element)
    {
        var states = new List<ShippingState>();
        if (element.Element("shipping_states") != null && element.Element("shipping_states")!.HasElements)
        {
            states.AddRange(element.Element("shipping_states")!.Elements("shipping_state").Select(ShippingState.FromXml));
        }

        return states;
    }

}
