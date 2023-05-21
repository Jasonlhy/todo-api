using System.Text.Json.Serialization;

namespace JasonTodoApi.ViewModels;

public record ErrorViewModel
{
    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("errorMessages")]
    public IEnumerable<string> ErrorMessages { get; set; } = Enumerable.Empty<string>();
}
