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
    /// Get todo list
    /// </summary>
    /// <param name="sortBy">Sort by multiiple column</param>
    /// <param name="filterBy"></param>
    /// <returns></returns>
    /// <exception cref="InvaliSortingFieldException">Invalid field</exception>
    Task<IEnumerable<TodoEntity>> GetTodoListAsync(Filtering filtering, string? sortByField);

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