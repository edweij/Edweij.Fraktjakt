using System.Text;
using System.Xml;

namespace Edweij.Fraktjakt.APIClient
{
    public class FromAddress : Address
    {
        public override string ToXml()
        {
            if (IsValid)
            {
                var sb = new StringBuilder();
                using (var w = XmlWriter.Create(sb, XmlWriterSettings))
                {
                    w.WriteStartElement("address_from");
                    w.WriteRaw(base.ToXml());
                }
                return sb.ToString();

            }
            throw new ArgumentException("Address element is not valid");
        }
    }
}
