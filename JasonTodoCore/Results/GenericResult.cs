namespace JasonTodoCore.Results;

/// <summary>
/// Generic results with error message and error code
/// </summary>
public class GenericResult
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Error code indicate the type of the error
    /// <see cref="GeneralErrorCode"/>
    /// </summary>
    public int? ErrorCode { get; set; }

    public static GenericResult True() => new GenericResult() { Success = true };
}
