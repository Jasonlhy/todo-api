namespace JasonTodoCore.Entities;

/// <summary>
/// Domain Entity without any database, UI logic involved
/// </summary>
public class TodoEntity
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTime DueDate { get; set; }

    /// <summary>
    /// Not Started, In Progress, Completed
    /// </summary>
    public required int Status { get; set; }
}
