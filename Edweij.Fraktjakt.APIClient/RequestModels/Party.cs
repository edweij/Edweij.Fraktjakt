namespace Edweij.Fraktjakt.APIClient.RequestModels;

public abstract class Party : XmlRequestObject
{        
    /// <summary>
    /// Max 32 characters
    /// </summary>
    public string? CompanyName { get; set; } = null;

    /// <summary>
    /// max 32 characters
    /// </summary>
    public string? PersonName { get; set; } = null;

    /// <summary>
    /// Max 32 characters
    /// </summary>
    public string? Phone { get; set; } = null;

    /// <summary>
    /// A valid email address, max 64 characters
    /// </summary>
    public string? Email { get; set; } = null;

    /// <summary>
    /// EORI-nummer for export out off EU, max 32 characters
    /// </summary>
    public string? Eori { get; set; } = null;

    /// <summary>
    /// Tax identification number (TIN), used in shipments from or to EU.
    /// Max 18 characters
    /// </summary>
    public string? Tin { get; set; } = null;

    public override IEnumerable<RuleViolation> GetRuleViolations()
    {
        if (!string.IsNullOrEmpty(CompanyName) && CompanyName.Length > 32) yield return new RuleViolation("CompanyName", "Max length 32");
        if (!string.IsNullOrEmpty(PersonName) && PersonName.Length > 32) yield return new RuleViolation("PersonName", "Max length 32");
        if (!string.IsNullOrEmpty(Phone) && Phone.Length > 32) yield return new RuleViolation("Phone", "Max length 32");
        if (!string.IsNullOrEmpty(Email))
        {
            if (Email.Length > 64) yield return new RuleViolation("Email", "Max length 64");
            if (!Email.IsValidEmailAddress()) yield return new RuleViolation("Email", "Not valid");
        }
        if (!string.IsNullOrEmpty(Eori) && Eori.Length > 32) yield return new RuleViolation("Eori", "Max length 32");
        if (!string.IsNullOrEmpty(Tin) && Tin.Length > 18) yield return new RuleViolation("Tin", "Max length 18");
        yield break;
    }

    
}




