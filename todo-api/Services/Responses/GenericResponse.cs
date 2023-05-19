using System.Text.Json.Serialization;

namespace todo_api.Services.Responses;

/// <summary>
/// Generic response for either true or false
/// </summary>
public record GenericResponse
{
    /// <summary>
    /// true if success
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// If status is false, this should contains error message
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// If status is false, this can contain error code
    /// <see cref="GenericErrorCode"/>
    /// </summary>
    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    public static GenericResponse Good() => new GenericResponse() { Success = true };
}
