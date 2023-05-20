using JasonTodoAPI.ViewModels;
using JasonTodoCore.Entities;

namespace JasonTodoAPI.Mappers;

/// <summary>
/// Mapper to convert between JSON view model and domain entity
/// </summary>
public static class TodoItemMapper
{
    public static TodoItem FromTodoEntity(TodoEntity todoEntity)
    {
        return new TodoItem
        {
            Id = todoEntity.Id,
            Name = todoEntity.Name,
            Description = todoEntity.Description,
            DueDate = todoEntity.DueDate,
            Status = todoEntity.Status,
            StatusString = TodoStatusHelper.ToString(todoEntity.Status),
        };
    }

    public static IEnumerable<TodoItem> FromTodoEntityList(IEnumerable<TodoEntity> todoEntityList)
    {
        foreach (var ent in todoEntityList)
        {
            yield return FromTodoEntity(ent);
        }
    }
}
