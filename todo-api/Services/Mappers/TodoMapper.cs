using todo_api.Db.Models;
using todo_api.Services.ViewModels;

namespace todo_api.Services.Mappers;

/// <summary>
/// Mapper to convert between database entity todo and different view model
/// 
/// Note: I don't like to use AutoMapper, the methods relies on method overloading so I don't need to type the long name
/// </summary>
public static class TodoMapper
{
    public static Todo ToTodo(CreateTodoViewModel createTodoViewModel)
    {
        // Id is automatically created 
        return new Todo
        {
            Name = createTodoViewModel.Name,
            Description = createTodoViewModel.Description,
            DueDate = createTodoViewModel.DueDate,
            Status = createTodoViewModel.Status,
        };
    }

    public static void UpdateTodo(Todo todo, UpdateTodoItemViewModel updateTodoItemViewModel)
    {
        todo.Name = updateTodoItemViewModel.Name;
        todo.Description = updateTodoItemViewModel.Description;
        todo.DueDate = updateTodoItemViewModel.DueDate;
        todo.Status = updateTodoItemViewModel.Status;
    }

    public static TodoItem FromTodo(Todo todo)
    {
        return new TodoItem
        {
            Id = todo.Id,
            Name = todo.Name,
            Description = todo.Description,
            DueDate = todo.DueDate,
            Status = todo.Status,
        };
    }
}
