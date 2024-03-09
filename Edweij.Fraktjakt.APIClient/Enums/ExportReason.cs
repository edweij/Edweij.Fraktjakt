namespace Edweij.Fraktjakt.APIClient.Enums;

/// <summary>
/// Represents the reason for exporting goods.
/// </summary>
public enum ExportReason
{
    /// <summary>
    /// Goods are exported for sale.
    /// </summary>
    SALE,

    /// <summary>
    /// Goods are exported as a gift.
    /// </summary>
    GIFT,

    /// <summary>
    /// Goods are exported as samples.
    /// </summary>
    SAMPLE,

    /// <summary>
    /// Goods are exported as returns.
    /// </summary>
    RETURN,

    /// <summary>
    /// Goods are exported for repair.
    /// </summary>
    REPAIR,

    /// <summary>
    /// Goods are exported as personal effects.
    /// </summary>
    PERSONAL_EFFECTS
}
