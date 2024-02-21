namespace Edweij.Fraktjakt.APIClient.Structs;

public readonly struct SwedishOrEnglish
{
    public string Code { get; init; }

    private static readonly HashSet<string> ValidCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    "SV", "EN"
};

    public SwedishOrEnglish(string code)
    {
        if (!ValidCodes.Contains(code.ToUpper()))
        {
            throw new ArgumentOutOfRangeException(nameof(code), "Language code not valid");
        }
        Code = code.ToUpper();
    }

    public static implicit operator SwedishOrEnglish(string code)
    {
        return new SwedishOrEnglish(code);
    }

    public override string ToString() => Code;
}
