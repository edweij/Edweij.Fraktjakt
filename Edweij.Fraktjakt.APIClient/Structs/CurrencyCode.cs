namespace Edweij.Fraktjakt.APIClient.Structs;

/// <summary>
/// Represents a ISO 4217 currency code.
/// </summary>
public readonly struct CurrencyCode
{
    /// <summary>
    /// Gets the ISO 4217 currency code.
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyCode"/> struct with the default ISO 4217 currency code ("SEK").
    /// </summary>
    public CurrencyCode()
    {
        Code = "SEK";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyCode"/> struct with the specified ISO 4217 currency code.
    /// </summary>
    /// <param name="code">The ISO 4217 currency code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided ISO 4217 currency code is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided ISO 4217 currency code is not valid.</exception>
    public CurrencyCode(string code)
    {
        if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code), "Currency code not valid");
        if (!validcodes.Contains(code.ToUpper())) throw new ArgumentOutOfRangeException(nameof(code), "Currency code not valid");
        Code = code.ToUpper();
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="CurrencyCode"/>.
    /// <br />
    /// This conversion allows you to create a <see cref="CurrencyCode"/> from a string without explicitly calling the constructor.
    /// </summary>
    /// <param name="code">The currency code as a string.</param>
    /// <returns>A new instance of the <see cref="CurrencyCode"/> struct.</returns>
    public static implicit operator CurrencyCode(string code)
    {
        return new CurrencyCode(code);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        CurrencyCode other = (CurrencyCode)obj;
        return Code == other.Code;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Code}";

    // Valid currency codes
    private readonly string[] validcodes = new string[]
    {
        "SEK", "EUR", "USD", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND",
        "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYN", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUP", "CVE",
        "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "FJD", "FKP", "GBP", "GEL", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HTG",
        "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP",
        "LKR", "LRD", "LSL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZN", "NAD", "NGN",
        "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG",
        "SGD", "SHP", "SLL", "SOS", "SRD", "SSP", "STN", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USN",
        "UYU", "UZS", "VES", "VND", "VUV", "WST", "XAF", "XCD", "XOF", "XPF", "YER", "ZAR", "ZMW", "ZWL"
    };
}
