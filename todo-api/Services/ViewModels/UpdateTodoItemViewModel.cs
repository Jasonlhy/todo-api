namespace todo_api.Services.ViewModels;

/// <summary>
/// The view model for updating todo item request
/// </summary>
public record UpdateTodoItemViewModel
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime DueDate { get; set; }
    public required string Status { get; set; }
}
