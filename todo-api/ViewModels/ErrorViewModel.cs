using System.Text.Json.Serialization;

namespace JasonTodoAPI.ViewModels;

public record ErrorViewModel
{
    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public IEnumerable<string> ErrorMessages { get; set; } = Enumerable.Empty<string>();
}
