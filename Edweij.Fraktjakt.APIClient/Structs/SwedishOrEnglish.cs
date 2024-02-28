namespace Edweij.Fraktjakt.APIClient.Structs;

public readonly struct SwedishOrEnglish
{
    public string Code { get; init; }

    private static readonly HashSet<string> ValidCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "sv", "en"
        };

    public SwedishOrEnglish() { Code = "sv"; }

    public SwedishOrEnglish(string code)
    {
        if (!ValidCodes.Contains(code.ToUpper()))
        {
            throw new ArgumentOutOfRangeException(nameof(code), "Language code not valid");
        }
        Code = code.ToLower();
    }

    public static implicit operator SwedishOrEnglish(string code)
    {
        return new SwedishOrEnglish(code);
    }

    public override string ToString() => Code;

    public static SwedishOrEnglish Swedish()
    {
        return new SwedishOrEnglish("sv");
    }

    public static SwedishOrEnglish English()
    {
        return new SwedishOrEnglish("en");
    }
}
