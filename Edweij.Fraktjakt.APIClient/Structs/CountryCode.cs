namespace Edweij.Fraktjakt.APIClient.Structs;

/// <summary>
/// Represents a ISO 3166 country code.
/// </summary>
public readonly struct CountryCode
{
    /// <summary>
    /// Gets the ISO 3166 country code.
    /// </summary>
    public string Code { get; init; }
    public string Name { get; init; }
    public bool HasPostalCode { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCode"/> struct with the default ISO 3166 country code ("SE").
    /// </summary>
    public CountryCode()
    {
        var countryInfo = validCodes.FirstOrDefault(c => c.Code == "SE");
        Code = countryInfo.Code;
        Name = countryInfo.Name;
        HasPostalCode = countryInfo.HasPostalCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CountryCode"/> struct with the specified ISO 3166 country code.
    /// </summary>
    /// <param name="code">The ISO 3166 country code.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided ISO 3166 country code is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided ISO 3166 country code is not valid.</exception>
    public CountryCode(string code)
    {
        if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code), "Country code not valid");
        var countryInfo = validCodes.FirstOrDefault(c => c.Code == "SE");
        if (countryInfo == default)
        {
            throw new ArgumentOutOfRangeException(nameof(code), "Country code not valid");
        }        
        Code = countryInfo.Code;
        Name = countryInfo.Name;
        HasPostalCode = countryInfo.HasPostalCode;
    }

    /// <summary>
    /// Implicitly converts a string to a <see cref="CountryCode"/>.
    /// <br />
    /// This conversion allows you to create a <see cref="CountryCode"/> from a string without explicitly calling the constructor.
    /// </summary>
    /// <param name="code">The country code as a string.</param>
    /// <returns>A new instance of the <see cref="CountryCode"/> struct.</returns>
    public static implicit operator CountryCode(string cc)
    {
        return new CountryCode(cc);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        CountryCode other = (CountryCode)obj;
        return Code == other.Code;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Code}";

    // Valid country codes
    private readonly (string Code, string Name, bool HasPostalCode)[] validCodes = new[] { ("AX", "Aaland Islands", true), ("AF", "Afghanistan", true),
        ("AL", "Albania", true), ("DZ", "Algeria", true), ("AS", "American Samoa", true), ("AD", "Andorra", true), ("AO", "Angola", true), ("AI", "Anguilla", true),
        ("AQ", "Antarctica", false), ("AG", "Antigua and Barbuda", false), ("AR", "Argentina", true), ("AM", "Armenia", true), ("AW", "Aruba", false), ("AC", "Ascension Island", true),
        ("AU", "Australia", true), ("AT", "Austria", true), ("AZ", "Azerbaijan", true), ("PT", "Azores (Portuguese autonomous region)", true), ("BS", "Bahamas", false), ("BH", "Bahrain", true),
        ("BD", "Bangladesh", true), ("BB", "Barbados", true), ("BY", "Belarus", true), ("BE", "Belgium", true), ("BZ", "Belize", false), ("BJ", "Benin", false), ("BM", "Bermuda", true), ("BT", "Bhutan", true),
        ("BO", "Bolivia", false), ("BA", "Bosnia and Herzegovina", true), ("BW", "Botswana", false), ("BV", "Bouvet Island", false), ("BR", "Brazil", true), ("IO", "British Indian Ocean Territory", true),
        ("BN", "Brunei Darussalam", true), ("BG", "Bulgaria", true), ("BF", "Burkina Faso", false), ("BI", "Burundi", false), ("KH", "Cambodia", true), ("CM", "Cameroon", false), ("CA", "Canada", true),
        ("IC", "Canary Islands", false), ("CV", "Cape Verde", true), ("KY", "Cayman Islands", true), ("CF", "Central African Republic", false), ("EA", "Ceuta and Melilla", false), ("TD", "Chad", true),
        ("CL", "Chile", true), ("CN", "China", true), ("CX", "Christmas Island", true), ("CP", "Clipperton Island", false), ("CC", "Cocos (Keeling) Islands", true), ("CO", "Colombia", true), ("KM", "Comoros", false),
        ("CG", "Congo", false), ("CD", "Congo, Democratic Republic of the", true), ("CK", "Cook Islands", false), ("CR", "Costa Rica", true), ("HR", "Croatia", true), ("CU", "Cuba", true), ("CW", "Curacao", false),
        ("CY", "Cyprus", true), ("CZ", "Czech Republic", true), ("DK", "Denmark", true), ("DG", "Diego Garcia", false), ("DJ", "Djibouti", false), ("DM", "Dominica", false), ("DO", "Dominican Republic", true),
        ("EC", "Ecuador", true), ("EG", "Egypt", true), ("SV", "El Salvador", true), ("GQ", "Equatorial Guinea", false), ("ER", "Eritrea", false), ("EE", "Estonia", true), ("ET", "Ethiopia", true), ("FK", "Falkland Islands (Malvinas)", true),
        ("FO", "Faroe Islands", true), ("FJ", "Fiji", false), ("FI", "Finland", true), ("FR", "France", true), ("FX", "France, Metropolitan", false), ("GF", "French Guiana", true), ("PF", "French Polynesia", true),
        ("TF", "French Southern Territories", false), ("GA", "Gabon", true), ("GM", "Gambia", false), ("GE", "Georgia", true), ("DE", "Germany", true), ("GH", "Ghana", false), ("GI", "Gibraltar", true), ("GR", "Greece", true),
        ("GL", "Greenland", true), ("GD", "Grenada", false), ("GP", "Guadeloupe", true), ("GU", "Guam", true), ("GT", "Guatemala", true), ("GG", "Guernsey", true), ("GN", "Guinea", true), ("GW", "Guinea-Bissau", true),
        ("GY", "Guyana", false), ("HT", "Haiti", true), ("HM", "Heard Island and McDonald Islands", true), ("HN", "Honduras", true), ("HK", "Hong Kong", false), ("HU", "Hungary", true), ("IS", "Iceland", true), ("IN", "India", true), 
        ("ID", "Indonesia", true), ("IR", "Iran, Islamic Republic of", true), ("IQ", "Iraq", true), ("IE", "Ireland", true), ("IM", "Isle of Man", true), ("IL", "Israel", true), ("IT", "Italy", true), ("CI", "Ivory Coast", false), 
        ("JM", "Jamaica", true), ("JP", "Japan", true), ("JE", "Jersey", true), ("JO", "Jordan", true), ("KZ", "Kazakhstan", true), ("KE", "Kenya", true), ("KI", "Kiribati", false), ("KP", "Korea, Democratic People's Republic of", false), 
        ("KR", "Korea, Republic of", true), ("XK", "Kosovo (use Serbia)", true), ("KW", "Kuwait", true), ("KG", "Kyrgyzstan", true), ("LA", "Lao People's Democratic Republic", true), ("LV", "Latvia", true), ("LB", "Lebanon", true), 
        ("LS", "Lesotho", true), ("LR", "Liberia", true), ("LY", "Libyan Arab Jamahiriya", true), ("LI", "Liechtenstein", true), ("LT", "Lithuania", true), ("LU", "Luxembourg", true), ("MO", "Macao", false), ("MK", "Macedonia", true), 
        ("MG", "Madagascar", true), ("MW", "Malawi", false), ("MY", "Malaysia", true), ("MV", "Maldives", true), ("ML", "Mali", false), ("MT", "Malta", true), ("MH", "Marshall Islands", true), ("MQ", "Martinique", true), ("MR", "Mauritania", false), 
        ("MU", "Mauritius", false), ("MX", "Mexico", true), ("FM", "Micronesia, Federated States of", true), ("MD", "Moldova", true), ("MC", "Monaco", true), ("MN", "Mongolia", true), ("ME", "Montenegro", true), ("MS", "Montserrat", true), 
        ("MA", "Morocco", true), ("MZ", "Mozambique", true), ("MM", "Myanmar", true), ("NA", "Namibia", true), ("NR", "Nauru", false), ("NP", "Nepal", true), ("NL", "Netherlands", true), ("AN", "Netherlands Antilles", false), 
        ("NC", "New Caledonia", true), ("NZ", "New Zealand", true), ("NI", "Nicaragua", true), ("NE", "Niger", true), ("NG", "Nigeria", true), ("NU", "Niue", false), ("NF", "Norfolk Island", true), ("MP", "Northern Mariana Islands", true), 
        ("NO", "Norway", true), ("OM", "Oman", true), ("PK", "Pakistan", true), ("PW", "Palau", true), ("PS", "Palestinian National Authority", false), ("PA", "Panama", true), ("PG", "Papua New Guinea", true), ("PY", "Paraguay", true), 
        ("PE", "Peru", true), ("PH", "Philippines", true), ("PN", "Pitcairn", true), ("PL", "Poland", true), ("PT", "Portugal", false), ("PR", "Puerto Rico", true), ("QA", "Qatar", false), ("RE", "Réunion", true), ("RO", "Romania", true), 
        ("RU", "Russia", true), ("RW", "Rwanda", false), ("BL", "Saint Barthélemy", true), ("SH", "Saint Helena", true), ("KN", "Saint Kitts and Nevis", false), ("LC", "Saint Lucia", true), ("MF", "Saint Martin (French part)", true), 
        ("PM", "Saint Pierre and Miquelon", true), ("VC", "Saint Vincent and the Grenadines", true), ("WS", "Samoa", false), ("SM", "San Marino", true), ("ST", "Sao Tome and Principe", false), ("SA", "Saudi Arabia", true), ("SN", "Senegal", true), 
        ("RS", "Serbia", true), ("CS", "Serbia and Montenegro (ended 2006)", false), ("SC", "Seychelles", false), ("SL", "Sierra Leone", false), ("SG", "Singapore", true), ("SK", "Slovakia", true), ("SI", "Slovenia", true), 
        ("SB", "Solomon Islands", false), ("SO", "Somalia", false), ("ZA", "South Africa", true), ("GS", "South Georgia and the South Sandwich Islands", true), ("ES", "Spain", true), ("LK", "Sri Lanka", true), ("SD", "Sudan", true), 
        ("SR", "Suriname", false), ("SJ", "Svalbard and Jan Mayen", true), ("SZ", "Swaziland", true), ("SE", "Sweden", true), ("CH", "Switzerland", true), ("SY", "Syrian Arab Republic", false), ("TW", "Taiwan, Province of China", false), 
        ("TJ", "Tajikistan", true), ("TZ", "Tanzania, United Republic of", false), ("TH", "Thailand", true), ("TL", "Timor-Leste", false), ("TG", "Togo", false), ("TK", "Tokelau", false), ("TO", "Tonga", false), ("TT", "Trinidad and Tobago", true), 
        ("TA", "Tristan da Cunha", false), ("TN", "Tunisia", true), ("TR", "Turkey", true), ("TM", "Turkmenistan", true), ("TC", "Turks and Caicos Islands", true), ("TV", "Tuvalu", false), ("UG", "Uganda", false), ("UA", "Ukraine", true), 
        ("AE", "United Arab Emirates", false), ("GB", "United Kingdom", true), ("US", "United States", true), ("UM", "United States Minor Outlying Islands", false), ("UY", "Uruguay", true), ("UZ", "Uzbekistan", true), ("VU", "Vanuatu", false), 
        ("VA", "Vatican City State", true), ("VE", "Venezuela", true), ("VN", "Viet Nam", true), ("VG", "Virgin Islands, British", true), ("VI", "Virgin Islands, U.S.", true), ("WF", "Wallis and Futuna", true), ("EH", "Western Sahara", false), 
        ("YE", "Yemen", false), ("ZM", "Zambia", true), ("ZW", "Zimbabwe", false) };
}
