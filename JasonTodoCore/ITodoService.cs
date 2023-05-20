using JasonTodoCore.Entities;
using JasonTodoCore.Exceptions;
using JasonTodoCore.Results;

namespace JasonTodoCore;

/// <summary>
/// High level interface providing the business logic of todo list
/// </summary>
public interface ITodoService
{
    /// <summary>
    /// Get todolist 
    /// </summary>
    /// <param name="filtering">Filtering object</param>
    /// <param name="sortByField">Sort by field value</param>
    /// <param name="sortAscending">Sort by ascending? which is default</param>
    /// <returns></returns>
    Task<IEnumerable<TodoEntity>> GetTodoListAsync(Filtering filtering, string? sortByField, bool sortAscending = true);

    /// <summary>
    /// Get the todo with id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TodoEntity?> GetTodoByIdAsync(long id);

    /// <summary>
    /// Create todo
    /// </summary>
    /// <param name="todo"></param>
    /// <returns></returns>
    Task<CreateResult<TodoEntity>> CreateTodoAsync(TodoEntity todoEntity);

    /// <summary>
    /// Update todo by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="todo"></param>
    /// <returns></returns>
    Task<GenericResult> UpdateTodoByIdAsync(long id, TodoEntity todoEntity);

    /// <summary>
    /// Delete todo by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GenericResult> DeleteTodoByIdAsync(long id);
}