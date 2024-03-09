namespace Edweij.Fraktjakt.APIClient.Enums;

/// <summary>
/// Represents the status of a response.
/// </summary>
public enum ResponseStatus
{
    /// <summary>
    /// The operation or request was successful.
    /// </summary>
    Ok,

    /// <summary>
    /// The operation or request completed with a warning.
    /// </summary>
    Warning,

    /// <summary>
    /// The operation or request encountered an error.
    /// </summary>
    Error
}
