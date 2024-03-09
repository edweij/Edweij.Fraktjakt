namespace Edweij.Fraktjakt.APIClient.Structs;

/// <summary>
/// Represents a Fraktjakt state identifier.
/// </summary>
public readonly struct FraktjaktStateId
{
    /// <summary>
    /// Gets the state identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the The string representation of the state id
    /// </summary>
    public string Name => validids[Id];

    /// <summary>
    /// Initializes a new instance of the <see cref="FraktjaktStateId"/> struct with the default state identifier (0).
    /// </summary>
    public FraktjaktStateId()
    {
        Id = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FraktjaktStateId"/> struct with the specified state identifier.
    /// </summary>
    /// <param name="id">The state identifier.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided state identifier is not valid.</exception>
    public FraktjaktStateId(int id)
    {
        if (!validids.ContainsKey(id))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
        }
        Id = id;
    }

    /// <summary>
    /// Implicitly converts an integer to a <see cref="FraktjaktStateId"/>.
    /// <br />
    /// This conversion allows you to create a <see cref="FraktjaktStateId"/> from an int without explicitly calling the constructor.
    /// </summary>
    /// <param name="id">The state identifier as an integer.</param>
    /// <returns>A new instance of the <see cref="FraktjaktStateId"/> struct.</returns>
    public static implicit operator FraktjaktStateId(int id)
    {
        return new FraktjaktStateId(id);
    }

    /// <summary>
    /// Returns a string representation of the state based on the identifier.
    /// </summary>
    /// <returns>The string representation of the state.</returns>
    public override string ToString() => validids[Id];

    // Valid state identifiers and their corresponding strings
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
