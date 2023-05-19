using todo_api.Services.Responses;
using todo_api.Services.ViewModels;

namespace todo_api.Services;

/// <summary>
/// High level interface providing the business logic of todo list
/// </summary>
public interface ITodoService
{
    Task<IEnumerable<TodoItem>> GetTodos();

    Task<TodoItem?> GetTodoById(long id);

    /// <summary>
    /// Create todo, if success, will return the todoItem
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    Task<TodoItem?> CreateTodo(CreateTodoViewModel todo);

    /// <summary>
    /// Update todo by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="todo"></param>
    /// <returns></returns>
    Task<GenericResponse> UpdateTodoById(int id, UpdateTodoItemViewModel todo);

    /// <summary>
    /// Delete todo by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GenericResponse> DeleteTodoById(int id);
}