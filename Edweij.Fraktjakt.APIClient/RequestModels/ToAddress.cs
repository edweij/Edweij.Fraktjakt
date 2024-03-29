﻿using Edweij.Fraktjakt.APIClient.Enums;
using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class ToAddress : Address
{
    public ToAddress(string postalCode) : base(postalCode) { }

    /// <summary>
    /// Language code in ISO 639-1
    /// Specifies which language Fraktjakt should use in email and communication with the receiver.
    /// </summary>
    public Language6391 Language { get; set; } = Language6391.sv;

    public override string ToXml()
    {
        if (IsValid)
        {
            var sb = new StringBuilder();
            using (var w = CreateXmlWriter(sb))
            {
                w.WriteStartElement("address_to");
                w.WriteRaw(base.ToXml());
                w.WriteElementString("language", Language.ToString());
            }
            return sb.ToString();
        }
        throw new ArgumentException("Address element is not valid");
    }
}
