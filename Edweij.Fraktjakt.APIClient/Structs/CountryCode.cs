namespace Edweij.Fraktjakt.APIClient.Structs;

public readonly struct CountryCode
{
    public string CC { get; init; }

    public CountryCode()
    {
        CC = "SE";
    }

    public CountryCode(string cc)
    {
        if (string.IsNullOrEmpty(cc)) throw new ArgumentNullException(nameof(cc), "Country code not valid");
        if (!validcodes.Contains(cc.ToUpper())) throw new ArgumentOutOfRangeException(nameof(cc), "Country code not valid");
        CC = cc.ToUpper();
    }
    public static implicit operator CountryCode(string cc)
    {
        return new CountryCode(cc);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        CountryCode other = (CountryCode)obj;
        return CC == other.CC;
    }

    public override int GetHashCode()
    {
        return CC.GetHashCode();
    }

    public override string ToString() => $"{CC}";

    private readonly string[] validcodes = new string[]
    {
        "AD","AE","AF","AG","AI","AL","AM","AO","AQ","AR","AS","AT","AU","AW","AX","AZ","BA","BB","BD",
        "BE","BF","BG","BH","BI","BJ","BL","BM","BN","BO","BQ","BR","BS","BT","BV","BW","BY","BZ","CA",
        "CC","CD","CF","CG","CH","CI","CK","CL","CM","CN","CO","CR","CU","CV","CW","CX","CY","CZ","DE",
        "DJ","DK","DM","DO","DZ","EC","EE","EG","EH","ER","ES","ET","FI","FJ","FK","FM","FO","FR","GA",
        "GB","GD","GE","GF","GG","GH","GI","GL","GM","GN","GP","GQ","GR","GS","GT","GU","GW","GY","HK",
        "HM","HN","HR","HT","HU","ID","IE","IL","IM","IN","IO","IQ","IR","IS","IT","JE","JM","JO","JP",
        "KE","KG","KH","KI","KM","KN","KP","KR","KW","KY","KZ","LA","LB","LC","LI","LK","LR","LS","LT",
        "LU","LV","LY","MA","MC","MD","ME","MF","MG","MH","MK","ML","MM","MN","MO","MP","MQ","MR","MS",
        "MT","MU","MV","MW","MX","MY","MZ","NA","NC","NE","NF","NG","NI","NL","NO","NP","NR","NU","NZ",
        "OM","PA","PE","PF","PG","PH","PK","PL","PM","PN","PR","PS","PT","PW","PY","QA","RE","RO","RS",
        "RU","RW","SA","SB","SC","SD","SE","SG","SH","SI","SJ","SK","SL","SM","SN","SO","SR","SS","ST",
        "SV","SX","SY","SZ","TC","TD","TF","TG","TH","TJ","TK","TL","TM","TN","TO","TR","TT","TV","TW",
        "TZ","UA","UG","UM","US","UY","UZ","VA","VC","VE","VG","VI","VN","VU","WF","WS","YE","YT","ZA",
        "ZM","ZW"
    };
}
