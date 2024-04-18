using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public abstract class XmlRequestObject : ValidationObject
{
    private readonly XmlWriterSettings xmlWriterSettings;

    /// <summary>
    /// Base class for all request obejts which produce XML for API calls
    /// </summary>
    public XmlRequestObject()
    {
        xmlWriterSettings = new XmlWriterSettings
        {
            Indent = false,
            Encoding = Encoding.UTF8,
            NewLineOnAttributes = false,
            CheckCharacters = true,
            OmitXmlDeclaration = true,
            WriteEndDocumentOnClose = true
        };
    }

    /// <summary>
    /// When overridden creates XML for API calls
    /// </summary>
    /// <returns>XML as string</returns>
    public abstract string ToXml();

    /// <summary>
    /// Creates a XML writer 
    /// </summary>
    /// <param name="sb">StringBuilder to use inside the XML writer</param>
    /// <param name="conformanceLevel">Defaults to document</param>
    /// <returns>XmlWriter</returns>
    public virtual XmlWriter CreateXmlWriter(StringBuilder sb, ConformanceLevel conformanceLevel = ConformanceLevel.Document)
    {
        xmlWriterSettings.ConformanceLevel = conformanceLevel;
        return XmlWriter.Create(sb, xmlWriterSettings);
    }

    /// <summary>
    /// Is used to check if generated XML string is valid XML. 
    /// </summary>
    /// <param name="xml">the XML string to validate</param>
    /// <returns>true or false</returns>
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
    /// <summary>
    /// Creating a string from float with a period as decimal separator
    /// </summary>
    /// <param name="value">float value</param>
    /// <returns>String with period as decimal separator</returns>
    public static string ToStringPeriodDecimalSeparator(this float value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";
        return value.ToString(nfi);
    }

    /// <summary>
    /// Validates an email address
    /// </summary>
    /// <param name="emailAddress">The email address to validate</param>
    /// <returns>true och false</returns>
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
