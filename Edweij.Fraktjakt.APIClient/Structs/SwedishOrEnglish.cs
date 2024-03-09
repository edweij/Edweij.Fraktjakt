namespace Edweij.Fraktjakt.APIClient.Structs;

/// <summary>
/// Represents a ISO 639-1 language code for Swedish ("sv") or English ("en").
/// </summary>
public readonly struct SwedishOrEnglish
{
    /// <summary>
    /// Gets the ISO 639-1 language code.
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// Valid ISO 639-1 language codes for Swedish and English.
    /// </summary>
    private static readonly HashSet<string> ValidCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "sv", "en"
        };

    /// <summary>
    /// Initializes a new instance of the <see cref="SwedishOrEnglish"/> struct with the default language code ("sv").
    /// </summary>
    public SwedishOrEnglish() { Code = "sv"; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwedishOrEnglish"/> struct with the specified ISO 639-1 language code.
    /// </summary>
    /// <param name="code">The ISO 639-1 language code.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided ISO 639-1 language code is not valid.</exception>
    public SwedishOrEnglish(string code)
    {
        if (!ValidCodes.Contains(code.ToUpper()))
        {
            throw new ArgumentOutOfRangeException(nameof(code), "Language code not valid");
        }
        Code = code.ToLower();
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="SwedishOrEnglish"/>.
    /// <br />
    /// This conversion allows you to create a <see cref="SwedishOrEnglish"/> from a string without explicitly calling the constructor.
    /// </summary>
    /// <param name="code">The ISO 639-1 language code as a string.</param>
    /// <returns>A new instance of the <see cref="SwedishOrEnglish"/> struct.</returns>
    public static implicit operator SwedishOrEnglish(string code)
    {
        return new SwedishOrEnglish(code);
    }

    /// <summary>
    /// Returns the string representation of the ISO 639-1 language code.
    /// </summary>
    /// <returns>The language code.</returns>
    public override string ToString() => Code;

    /// <summary>
    /// Creates a new instance of <see cref="SwedishOrEnglish"/> with the ISO 639-1 language code set to "sv" (Swedish).
    /// </summary>
    /// <returns>A new instance of <see cref="SwedishOrEnglish"/> with the ISO 639-1 language code "sv".</returns>
    public static SwedishOrEnglish Swedish()
    {
        return new SwedishOrEnglish("sv");
    }

    /// <summary>
    /// Creates a new instance of <see cref="SwedishOrEnglish"/> with the ISO 639-1 language code set to "en" (English).
    /// </summary>
    /// <returns>A new instance of <see cref="SwedishOrEnglish"/> with the ISO 639-1 language code "en".</returns>
    public static SwedishOrEnglish English()
    {
        return new SwedishOrEnglish("en");
    }
}
