using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

/// <summary>
/// Represents a shipping document.
/// </summary>
/// <param name="Name">Link to a shipping document, on Fraktjakt.</param>
/// <param name="TypeId">Id for the type of document this is. Can be used for local filtering of the document.</param>
/// <param name="TypeName">The name of the type in plain text.</param>
/// <param name="TypeDescription">Explanation and instructions for the type.</param>
/// <param name="StateName">Status of the document.</param>
/// <param name="StateDescription">A description of the status</param>
/// <param name="FormatName">An explanation of the status. </param>
/// <param name="File">Entire Base64 encoded file.</param>
public record ShippingDocument(string Name, ShippingDocumentTypeId TypeId, string TypeName, string TypeDescription, string StateName, string StateDescription, string FormatName, string File)
{
    /// <summary>
    /// Creates an instance of <see cref="ShippingDocument"/> from an XML element.
    /// </summary>
    /// <param name="element">The XML element to parse.</param>
    /// <returns>An instance of <see cref="ShippingDocument"/> representing the parsed document.</returns>
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

    /// <summary>
    /// Converts the base64-encoded PDF content of the shipping document to a byte array.
    /// </summary>
    /// <returns>A byte array representing the PDF content of the shipping document.</returns>
    public byte[] PdfFromBase64()
    {
        byte[] byteArray = Convert.FromBase64String(File);
        return byteArray;
    }
}
