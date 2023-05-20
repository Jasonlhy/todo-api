using JasonTodoCore;
using JasonTodoCore.Constants;
using JasonTodoCore.Entities;
using JasonTodoCore.Exceptions;
using JasonTodoCore.Results;
using JasonTodoInfrastructure.Data;
using JasonTodoInfrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JasonTodoInfrastructure;

/// <summary>
/// Implementation of the Todo list with EntityFramework core conntaing to database
/// </summary>
public class TodoService : ITodoService
{
    private readonly TodoContext context;
    private readonly ILogger<TodoService> logger;

    public TodoService(TodoContext context, ILogger<TodoService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<TodoEntity>> GetTodoListAsync(Filtering filtering, string? sortByField, bool sortAscending = true)
    {
        // Filter by
        IQueryable<Todo> todos = context.Todos;
        if (filtering.Name is not null)
        {
            todos = todos.Where(t => t.Name == filtering.Name);
        }
        else if (filtering.Status is not null)
        {
            todos = todos.Where(t => t.Status == filtering.Status);
        }
        else if (filtering.DueDate is not null)
        {
            todos = todos.Where(t => t.DueDate == filtering.DueDate);
        }

        // Order by 
        if (!string.IsNullOrEmpty(sortByField))
        {
            // todo
            if (sortByField == "name")
            {
                todos = (sortAscending) ? todos.OrderBy(t => t.Name) : todos.OrderByDescending(t => t.Name);
            }
            else if (sortByField == "stats")
            {
                todos = (sortAscending) ? todos.OrderBy(t => t.Status) : todos.OrderByDescending(t => t.Status);
            }
            else if (sortByField == "dueDate")
            {
                todos = (sortAscending) ? todos.OrderBy(t => t.DueDate) : todos.OrderByDescending(t => t.DueDate);
            }
            else
            {
                throw new InvalidSortingFieldException(sortByField);
            }
        }

        var todoList = await todos.ToListAsync();
        return todoList.Select(TodoEntityMapper.FromTodo);
    }

    public async Task<TodoEntity?> GetTodoByIdAsync(long id)
    {
        var todo = await context.Todos.FindAsync(id);
        if (todo is null)
        {
            return null;
        }

        return TodoEntityMapper.FromTodo(todo);
    }

    public async Task<CreateResult<TodoEntity>> CreateTodoAsync(TodoEntity todoEntity)
    {
        Todo todo = TodoEntityMapper.ToTodo(todoEntity);
        await context.Todos.AddAsync(todo);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.CREATE_TODO, ex, "Failed to create with todo {createTodoViewwModel}", todoEntity.ToString());

            return new CreateResult<TodoEntity>()
            {
                Success = false,
                Value = TodoEntityMapper.FromTodo(todo),
            };
        }

        return new CreateResult<TodoEntity>()
        {
            Success = true,
            Value = TodoEntityMapper.FromTodo(todo),
        };
    }

    public async Task<GenericResult> UpdateTodoByIdAsync(long id, TodoEntity todoEntity)
    {
        var existingTodo = await context.Todos.FindAsync(id);
        if (existingTodo is null)
        {
            return new GenericResult()
            {
                Success = false,
                ErrorCode = GeneralErrorCode.NotFound,
                ErrorMessage = $"Cannot find todo list with id {id} to update",
            };
        }

        try
        {
            TodoEntityMapper.UpdateTodo(existingTodo, todoEntity);
            await context.SaveChangesAsync();
            return GenericResult.True();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.UPDATE_TODO, ex, "Failed to update todo with {id}", id);

            return new GenericResult()
            {
                Success = false,
                ErrorCode = GeneralErrorCode.DbUpdateFailed,
                ErrorMessage = $"Failed to update the database id {id}",
            };
        }
    }

    public async Task<GenericResult> DeleteTodoByIdAsync(long id)
    {
        var existingTodo = await context.Todos.FindAsync(id);
        if (existingTodo is null)
        {
            return new GenericResult()
            {
                Success = false,
                ErrorCode = GeneralErrorCode.NotFound,
                ErrorMessage = $"Cannot find todo list with id {id} to delete",
            };
        }

        try
        {
            context.Todos.Remove(existingTodo);
            await context.SaveChangesAsync();
            return GenericResult.True();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.DELETE_TODO, ex, "Failed to delete todo with {id}", id);

            return new GenericResult()
            {
                Success = false,
                ErrorCode = GeneralErrorCode.DbUpdateFailed,
                ErrorMessage = $"Failed to delete the todolist item {id}",
            };
        }
    }


}