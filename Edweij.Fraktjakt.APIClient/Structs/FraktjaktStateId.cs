namespace Edweij.Fraktjakt.APIClient.Structs;

public readonly struct FraktjaktStateId
{
    public int Id { get; init; }

    public FraktjaktStateId()
    {
        Id = 0;
    }

    public FraktjaktStateId(int id)
    {
        if (!validids.ContainsKey(id))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
        }
        Id = id;
    }
    public static implicit operator FraktjaktStateId(int id)
    {
        return new FraktjaktStateId(id);
    }

    public override string ToString() => validids[Id];

    private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
        { 0, "Obetald"},
        { 1, "Förberedande"},
        { 2, "not used state"},
        { 3, "Betald"},
        { 4, "Avsänt"},
        { 5, "Levererat"},
        { 6, "Kvitterats"},
        { 7, "Retur"},
        { 12, "Hanteras av transportören"},
        { 17, "Rättas"},
        { 18, "Väntande"},
        { 19, "Söks"}
    };
}
