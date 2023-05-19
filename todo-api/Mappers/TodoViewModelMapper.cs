using JasonTodoAPI.ViewModels;
using JasonTodoCore.Entities;

namespace JasonTodoAPI.Mappers;

/// <summary>
/// Mapper to convert between JSON view model and domain entity
/// </summary>
public static class TodoViewModelMapper
{
    public static TodoEntity ToTodoEntity(CreateTodoViewModel createTodoViewModel)
    {
        // Id is automatically created 
        return new TodoEntity
        {
            Name = createTodoViewModel.Name,
            Description = createTodoViewModel.Description,
            DueDate = createTodoViewModel.DueDate,
            Status =  createTodoViewModel.Status,
        };
    }

    public static TodoEntity ToTodoEntity(int id, UpdateTodoItemViewModel updateToddoViewModel)
    {
        return new TodoEntity
        {
            Id = id,
            Name = updateToddoViewModel.Name,
            Description = updateToddoViewModel.Description,
            DueDate = updateToddoViewModel.DueDate,
            Status = updateToddoViewModel.Status,
        };
    }

    public static TodoItem FromTodoEntity(TodoEntity todoEntity)
    {
        return new TodoItem
        {
            Id = todoEntity.Id,
            Name = todoEntity.Name,
            Description = todoEntity.Description,
            DueDate = todoEntity.DueDate,
            Status = todoEntity.Status,
        };
    }
}
