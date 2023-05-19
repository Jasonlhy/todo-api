using JasonTodoCore;
using JasonTodoCore.Constants;
using JasonTodoCore.Entities;
using JasonTodoCore.Results;
using JasonTodoInfrastructure.Data;
using JasonTodoInfrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JasonTodoInfrastructure;

public class TodoService : ITodoService
{
    private readonly TodoContext context;
    private readonly ILogger<TodoService> logger;

    public TodoService(TodoContext context, ILogger<TodoService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<TodoEntity>> GetTodos(string[] sortByField, int[] filterByStatus)
    {
        var todoList = await context.Todos.ToListAsync();
        return todoList.Select(TodoEntityMapper.FromTodo);
    }

    public async Task<TodoEntity?> GetTodoById(long id)
    {
        var todo = await context.Todos.FindAsync(id);
        if (todo is null)
        {
            return null;
        }

        return TodoEntityMapper.FromTodo(todo);
    }

    public async Task<CreateResult<TodoEntity>> CreateTodo(TodoEntity createTodoViewwModel)
    {
        Todo todo = TodoEntityMapper.ToTodo(createTodoViewwModel);
        await context.Todos.AddAsync(todo);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.CREATE_TODO, ex, "Failed to create with todo {createTodoViewwModel}", createTodoViewwModel.ToString());

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

    public async Task<GenericResult> UpdateTodoById(int id, TodoEntity updateTodoItemViewModel)
    {
        var existingTodo = await context.Todos.FirstAsync(t => t.Id == id);
        if (existingTodo is null)
        {
            return new GenericResult()
            {
                Success = false,
                ErrorCode = GenericResultCode.NotFound,
                ErrorMessage = $"Cannot find todo list with id {id} to update",
            };
        }

        try
        {
            TodoEntityMapper.UpdateTodo(existingTodo, updateTodoItemViewModel);
            await context.SaveChangesAsync();
            return GenericResult.Good();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.UPDATE_TODO, ex, "Failed to update todo with {id}", id);

            return new GenericResult()
            {
                Success = false,
                ErrorCode = GenericResultCode.NotFound,
                ErrorMessage = $"Failed to update the database id {id}",
            };
        }
    }

    public async Task<GenericResult> DeleteTodoById(int id)
    {
        var existingTodo = await context.Todos.FirstAsync(t => t.Id == id);
        if (existingTodo is null)
        {
            return new GenericResult()
            {
                Success = false,
                ErrorCode = GenericResultCode.NotFound,
                ErrorMessage = $"Cannot find todo list with id {id} to delete",
            };
        }

        try
        {
            context.Todos.Remove(existingTodo);
            await context.SaveChangesAsync();
            return GenericResult.Good();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.DELETE_TODO, ex, "Failed to delete todo with {id}", id);

            return new GenericResult()
            {
                Success = false,
                ErrorCode = GenericResultCode.NotFound,
                ErrorMessage = $"Failed to delete the todolist item {id}",
            };
        }
    }


}