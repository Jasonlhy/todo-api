namespace todo_api.Services.ViewModels;

/// <summary>
/// The view model for creating todo item
/// </summary>
public record CreateTodoViewModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public required string Status { get; set; }
}
