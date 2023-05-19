namespace JasonTodoCore.Results;

/// <summary>
/// Generic results with error message and error code
/// </summary>
public class GenericResult
{
    public bool Success { get; set; }

    public string? ErrorMessage { get; set; }

    public int? ErrorCode { get; set; }

    public static GenericResult Good() => new GenericResult() { Success = true };
}
