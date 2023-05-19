using Microsoft.EntityFrameworkCore;
using todo_api.Constants;
using todo_api.Db;
using todo_api.Db.Models;
using todo_api.Services.Mappers;
using todo_api.Services.Responses;
using todo_api.Services.ViewModels;

namespace todo_api.Services;

public class TodoService : ITodoService
{
    private readonly TodoContext context;
    private readonly ILogger<TodoService> logger;

    public TodoService(TodoContext context, ILogger<TodoService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<TodoItem>> GetTodos()
    {
        var todoList = await context.Todos.ToListAsync();
        return todoList.Select(TodoMapper.FromTodo);
    }

    public async Task<TodoItem?> GetTodoById(long id)
    {
        var todo = await context.Todos.FindAsync(id);
        if (todo is null)
        {
            return null;
        }

        return TodoMapper.FromTodo(todo);
    }

    public async Task<TodoItem?> CreateTodo(CreateTodoViewModel createTodoViewwModel)
    {
        Todo todo = TodoMapper.ToTodo(createTodoViewwModel);
        await context.Todos.AddAsync(todo);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.CREATE_TODO, ex, "Failed to create with todo {createTodoViewwModel}", createTodoViewwModel.ToString());
        }

        return TodoMapper.FromTodo(todo);
    }

    public async Task<GenericResponse> UpdateTodoById(int id, UpdateTodoItemViewModel updateTodoItemViewModel)
    {
        var existingTodo = await context.Todos.FirstAsync(t => t.Id == id);
        if (existingTodo is null)
        {
            return new GenericResponse()
            {
                Success = false,
                ErrorCode = GenericErrorCode.NotFound,
                ErrorMessage = $"Cannot find todo list with id {id} to update",
            };
        }

        try
        {
            TodoMapper.UpdateTodo(existingTodo, updateTodoItemViewModel);
            await context.SaveChangesAsync();
            return GenericResponse.Good();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.UPDATE_TODO, ex, "Failed to update todo with {id}", id);

            return new GenericResponse()
            {
                Success = false,
                ErrorCode = GenericErrorCode.DbUpdateFailed,
                ErrorMessage = $"Failed to update the database id {id}",
            };
        }
    }

    public async Task<GenericResponse> DeleteTodoById(int id)
    {
        var existingTodo = await context.Todos.FirstAsync(t => t.Id == id);
        if (existingTodo is null)
        {
            return new GenericResponse()
            {
                Success = false,
                ErrorCode = GenericErrorCode.NotFound,
                ErrorMessage = $"Cannot find todo list with id {id} to delete",
            };
        }

        try
        {
            context.Todos.Remove(existingTodo);
            await context.SaveChangesAsync();
            return GenericResponse.Good();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(LoggingEvent.DELETE_TODO, ex, "Failed to delete todo with {id}", id);

            return new GenericResponse()
            {
                Success = false,
                ErrorCode = GenericErrorCode.DbUpdateFailed,
                ErrorMessage = $"Failed to delete the todolist item {id}",
            };
        }
    }
}