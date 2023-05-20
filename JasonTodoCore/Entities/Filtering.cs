namespace JasonTodoCore.Entities;

/// <summary>
/// Filtering entity for Todo list, todo list can be filter by name and by due date and by status 
/// It is designed as object for further extensions
/// </summary>
public class Filtering
{
    /// <summary>
    /// Filter by equals to this name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Filter by equals to this DueDate
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Filter by equals to this Status
    /// </summary>
    public int? Status { get; set; }
}
