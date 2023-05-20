using JasonTodoCore.Entities;
using JasonTodoInfrastructure.Data.Models;

namespace JasonTodoInfrastructure;

/// <summary>
/// Mapper to convert between domain entity todo and database todo table
/// 
/// Note: I don't like to use AutoMapper, the methods relies on method overloading so I don't need to type the long name
/// </summary>
public static class TodoEntityMapper
{
    public static Todo ToTodo(TodoEntity todoEntity)
    {
        // Id is automatically created 
        return new Todo
        {
            Name = todoEntity.Name,
            Description = todoEntity.Description,
            DueDate = todoEntity.DueDate,
            Status = todoEntity.Status,
        };
    }

    public static void UpdateTodo(Todo todo, TodoEntity todoEntity)
    {
        todo.Name = todoEntity.Name;
        todo.Description = todoEntity.Description;
        todo.DueDate = todoEntity.DueDate;
        todo.Status = todoEntity.Status;
    }

    public static TodoEntity FromTodo(Todo todo)
    {
        return new TodoEntity
        {
            Id = todo.Id,
            Name = todo.Name,
            Description = todo.Description,
            DueDate = todo.DueDate,
            Status = todo.Status,
            
        };
    }
}
