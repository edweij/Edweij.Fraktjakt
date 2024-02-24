﻿using Edweij.Fraktjakt.APIClient.Enums;
using System.Text;

namespace Edweij.Fraktjakt.APIClient.RequestModels;

public class ToAddress : Address
{
    public ToAddress(string postalCode) : base(postalCode) { }
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
