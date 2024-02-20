namespace Edweij.Fraktjakt.APIClient
{
    public abstract class ValidationObject
    {
        public bool IsValid => !GetRuleViolations().Any();

        public abstract IEnumerable<RuleViolation> GetRuleViolations();
    }
}
