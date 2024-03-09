namespace Edweij.Fraktjakt.APIClient.Structs;

/// <summary>
/// Represents a shipping state identifier.
/// </summary>
public readonly struct ShippingStateId
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
    /// Initializes a new instance of the <see cref="ShippingStateId"/> struct with the default state identifier (0).
    /// </summary>
    public ShippingStateId()
    {
        Id = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShippingStateId"/> struct with the specified state identifier.
    /// </summary>
    /// <param name="id">The state identifier.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided state identifier is not valid.</exception>
    public ShippingStateId(int id)
    {
        if (!validids.ContainsKey(id))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
        }
        Id = id;
    }

    /// <summary>
    /// Implicitly converts an integer to a <see cref="ShippingStateId"/>.
    /// <br />
    /// This conversion allows you to create a <see cref="ShippingStateId"/> from an int without explicitly calling the constructor.
    /// </summary>
    /// <param name="id">The state identifier as an integer.</param>
    /// <returns>A new instance of the <see cref="ShippingStateId"/> struct.</returns>
    public static implicit operator ShippingStateId(int id) => new(id);


    public static implicit operator int(ShippingStateId id) => id.Id;

    /// <summary>
    /// Returns a string representation of the state based on the identifier.
    /// </summary>
    /// <returns>The string representation of the state.</returns>
    public override string ToString() => validids[Id];

    // Valid state identifiers and their corresponding strings
    private readonly Dictionary<int, string> validids = new Dictionary<int, string> {
        { 0, "Hanteras av transportören"},
        { 1, "Avsänt"},
        { 2, "Levererat"},
        { 3, "Kvitterats"},
        { 4, "Retur"}
    };
}
