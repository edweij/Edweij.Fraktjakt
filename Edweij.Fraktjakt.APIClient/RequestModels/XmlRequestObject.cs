using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public abstract class XmlRequestObject : ValidationObject
{
    private readonly XmlWriterSettings xmlWriterSettings;

    public XmlRequestObject()
    {
        xmlWriterSettings = new XmlWriterSettings();
        xmlWriterSettings.Indent = false;
        xmlWriterSettings.Encoding = Encoding.UTF8;
        xmlWriterSettings.NewLineOnAttributes = false;
        xmlWriterSettings.CheckCharacters = true;
        xmlWriterSettings.OmitXmlDeclaration = true;
        xmlWriterSettings.WriteEndDocumentOnClose = true;
    }

    public abstract string ToXml();

    public XmlWriter CreateXmlWriter(StringBuilder sb, ConformanceLevel conformanceLevel = ConformanceLevel.Document)
    {
        xmlWriterSettings.ConformanceLevel = conformanceLevel;
        return XmlWriter.Create(sb, xmlWriterSettings);
    } 

    public bool IsValidXml(string xml)
    {
        try
        {
            var testElement = XElement.Parse(xml);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

public static class XmlRequestObjectExtensions
{

    public static string ToStringPeriodDecimalSeparator(this float value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";
        return value.ToString(nfi);
    }

    public static bool IsValidEmailAddress(this string emailAddress)
    {
        try
        {
            var email = new MailAddress(emailAddress);
            return email.Address == emailAddress.Trim();
        }
        catch
        {
            return false;
        }
    }
}
