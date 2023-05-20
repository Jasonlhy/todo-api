﻿using JasonTodoCore;
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

    public async Task<IEnumerable<TodoEntity>> GetTodos(Filtering filtering, string? sortByField)
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
                todos = todos.OrderBy(t => t.Name);
            }
            else if (sortByField == "stats")
            {
                todos = todos.OrderBy(t => t.Status);
            }
            else if (sortByField == "dueDate")
            {
                todos = todos.OrderBy(t => t.DueDate);
            }
            else
            {
                throw new InvalidOperationException("Unsupported field to be sorted by");
            }
        }

        var todoList = await todos.ToListAsync();
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

    public async Task<GenericResult> UpdateTodoById(long id, TodoEntity updateTodoItemViewModel)
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

    public async Task<GenericResult> DeleteTodoById(long id)
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