using Edweij.Fraktjakt.APIClient.Structs;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.ResponseModels;

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
