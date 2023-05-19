using System.Text.Json.Serialization;

namespace JasonTodoAPI.ViewModels;

/// <summary>
/// View model for updating todo item request
/// </summary>
public record UpdateTodoItemViewModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("status")]
    public required int Status { get; set; }
}
