namespace Edweij.Fraktjakt.APIClient.Structs;

/// <summary>
/// Represents a shipping document type identifier.
/// </summary>
public readonly struct ShippingDocumentTypeId
{
    /// <summary>
    /// Gets the shipping document type identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the The string representation of the shipping document type
    /// </summary>
    public string Name => validids[Id];

    /// <summary>
    /// Initializes a new instance of the <see cref="ShippingDocumentTypeId"/> struct with the shipping document type identifier.
    /// </summary>
    /// <param name="id">The shipping document type identifier.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided shipping document type identifier is not valid.</exception>
    public ShippingDocumentTypeId(int id)
    {
        if (!validids.ContainsKey(id))
        {
            throw new ArgumentOutOfRangeException(nameof(id), "id not valid");
        }
        Id = id;
    }

    /// <summary>
    /// Implicitly converts an integer to a <see cref="ShippingDocumentTypeId"/>.
    /// <br />
    /// This conversion allows you to create a <see cref="ShippingDocumentTypeId"/> from an int without explicitly calling the constructor.
    /// </summary>
    /// <param name="id">The shipping document type identifier as an integer.</param>
    /// <returns>A new instance of the <see cref="ShippingDocumentTypeId"/> struct.</returns>
    public static implicit operator ShippingDocumentTypeId(int id)
    {
        return new ShippingDocumentTypeId(id);
    }

    /// <summary>
    /// Returns a string representation of the shipping document type based on the identifier.
    /// </summary>
    /// <returns>The string representation of the shipping document type.</returns>
    public override string ToString() => validids[Id];

    // Valid shipping document type identifiers and their corresponding strings
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
