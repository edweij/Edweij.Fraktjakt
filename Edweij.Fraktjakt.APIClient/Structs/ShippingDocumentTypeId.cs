namespace Edweij.Fraktjakt.APIClient
{
    public readonly struct ShippingDocumentTypeId
    {
        public int Id { get; init; }

        public ShippingDocumentTypeId()
        {
            Id = 0;
        }

        public ShippingDocumentTypeId(int id)
        {
            if (!validids.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
            }
            this.Id = id;
        }
        public static implicit operator ShippingDocumentTypeId(int id)
        {
            return new ShippingDocumentTypeId(id);
        }

        public override string ToString() => validids[Id];

        private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
            { 1, "Pro Forma-faktura"},
            { 2, "Handelsfaktura"},
            { 3, "Fraktetikett"},
            { 4, "Fraktsedel"},
            { 5, "Sändningslista"},
            { 10, "Följesedel"},
            { 11, "CN22"},
            { 12, "CN23"},
            { 13, "Säkerhetsdeklaration"}
        };
    }
}
