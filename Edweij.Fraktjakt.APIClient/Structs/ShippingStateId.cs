namespace Edweij.Fraktjakt.APIClient
{
    public readonly struct ShippingStateId
    {
        public int Id { get; init; }

        public ShippingStateId()
        {
            Id = 0;
        }

        public ShippingStateId(int id)
        {
            if (!validids.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
            }
            this.Id = id;
        }
        public static implicit operator ShippingStateId(int id)
        {
            return new ShippingStateId(id);
        }

        public override string ToString() => validids[Id];

        private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
            { 0, "Hanteras av transportören"},
            { 1, "Avsänt"},
            { 2, "Levererat"},
            { 3, "Kvitterats"},
            { 4, "Retur"}
        };
    }
}
