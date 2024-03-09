using Edweij.Fraktjakt.APIClient.Enums;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents the tracking information and shipping states for a shipment trace.
/// </summary>
public record TraceResponse(string TrackingCode, string TrackingLink, IEnumerable<ShippingState> ShippingStates)
{
    /// <summary>
    /// Gets or sets the optional tracking number associated with the trace response.
    /// </summary>
    public string? TrackingNumber { get; init; }
    /// <summary>
    /// Gets or sets the optional shipping company name associated with the trace response.
    /// </summary>
    public string? ShippingCompany { get; init; }
    /// <summary>
    /// Gets or sets the optional list of shipping documents associated with the trace response.
    /// </summary>
    public IEnumerable<string>? ShippingDocuments { get; init; }

    /// <summary>
    /// Creates an instance of <see cref="TraceResponse"/> from an HTTP response message.
    /// </summary>
    /// <param name="httpResponseMessage">The HTTP response message containing trace information.</param>
    /// <returns>An instance of <see cref="TraceResponse"/> representing the parsed trace information.</returns>
    public static async Task<Response<TraceResponse>> FromHttpResponse(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage == null) return Response<TraceResponse>.CreateErrorResponse("HttpResponseMessage was null");
        if (!httpResponseMessage.IsSuccessStatusCode) return Response<TraceResponse>.CreateErrorResponse($"Not successful response ({httpResponseMessage.StatusCode}). Response Content: '{await httpResponseMessage.Content.ReadAsStringAsync()}'.");
        string xml = await httpResponseMessage.Content.ReadAsStringAsync();
        return FromXml(xml);
    }

    /// <summary>
    /// Creates a response instance of <see cref="TraceResponse"/> from an XML representation.
    /// </summary>
    /// <param name="xml">The XML string containing the trace information.</param>
    /// <returns>A response instance of <see cref="TraceResponse"/> representing the parsed trace information.</returns>
    public static Response<TraceResponse> FromXml(string xml)
    {
        try
        {
            XElement element = XElement.Parse(xml);
            var status = (ResponseStatus)int.Parse(element.Element("code")!.Value);

            if (status == ResponseStatus.Error)
            {
                return Response<TraceResponse>.CreateErrorResponseFromXml(element);
            }

            var result = new Response<TraceResponse>(element.Element("server_status")!.Value,
                status,
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

    /// <summary>
    /// Parses and creates an instance of <see cref="TraceResponse"/> from an XML element.
    /// </summary>
    /// <param name="element">The XML element containing the trace information.</param>
    /// <returns>An instance of <see cref="TraceResponse"/> representing the parsed trace information.</returns>
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

    /// <summary>
    /// Parses and creates a list of <see cref="ShippingState"/> instances from an XML element.
    /// </summary>
    /// <param name="element">The XML element containing shipping states information.</param>
    /// <returns>A list of <see cref="ShippingState"/> instances representing the parsed shipping states.</returns>
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
