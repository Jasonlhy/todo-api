using JasonTodoApi.Mappers;
using JasonTodoApi.ViewModels;
using JasonTodoCore;
using JasonTodoCore.Constants;
using JasonTodoCore.Entities;
using JasonTodoCore.Validators;
using JasonTodoInfrastructure;
using JasonTodoInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Our servie
builder.Services.AddDbContext<TodoContext>(optionsBuilder =>
{
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);
    var dbPath = System.IO.Path.Join(path, "blogging.db");
    optionsBuilder.UseSqlite($"Data Source={dbPath}");
});
builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

MapTodoRoute(app);

app.Run();



static void MapTodoRoute(WebApplication app)
{
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-7.0
    // GET: /todos
    app.MapGet("/todos", async ([FromServices] ITodoService todoService,
        [FromQuery] DateTime? dueDate,
        [FromQuery] string? name,
        [FromQuery] int? status,
        [FromQuery] string? sortBy,
        [FromQuery] bool? sortAscending) =>
    {
        // This fields is kind of .... 
        if (status.HasValue && !TodoStatusHelper.IsValidTodoStatus(status.Value))
        {
            return Results.BadRequest(new ErrorViewModel()
            {
                ErrorCode = GeneralErrorCode.InvalidStatus,
                ErrorMessages = new string[] { $"{status} is not valid" }
            });
        }

        var todoLists = await todoService.GetTodoListAsync(new Filtering
        {
            Name = name,
            DueDate = dueDate,
            Status = status,
        }, sortBy);
        var todoItemList = TodoItemMapper.FromTodoEntityList(todoLists);

        return Results.Ok(todoItemList);
    })
    .Produces<IEnumerable<TodoItem>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithOpenApi(generatedOperation =>
    {
        generatedOperation.Summary = "Get todo list";
        generatedOperation.Description = "Get todo list";
        var parameter = generatedOperation.Parameters[0];
        parameter.Description = "filter by";
        parameter.Description = "sort by";
        return generatedOperation;
    });

    // GET: /todos/{id}
    app.MapGet("/todos/{id}", async ([FromServices] ITodoService todoService, long id) =>
    {
        var todoEntity = await todoService.GetTodoByIdAsync(id);
        if (todoEntity is null)
        {
            return Results.StatusCode(StatusCodes.Status404NotFound);
        }

        var todoItem = TodoItemMapper.FromTodoEntity(todoEntity);
        return Results.Ok(todoItem);
    })
    .Produces<TodoItem>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithOpenApi(generatedOperation =>
    {
        generatedOperation.Summary = "Get todos with id";
        generatedOperation.Description = "Get todos with id";
        var parameter = generatedOperation.Parameters[0];
        parameter.Description = "The ID associated with the created Todo";
        return generatedOperation;
    });

    // POST: /todos
    app.MapPost("/todos", async ([FromServices] ITodoService todoService, CreateTodoViewModel createTodoViewModel) =>
    {
        TodoEntity todoEntity = CreateUpdateViewModelMapper.ToTodoEntity(createTodoViewModel);
        var validator = new TodoEntityValidator();
        if (validator.IsValid(todoEntity) == false)
        {
            return Results.BadRequest(ErrorViewModelMapper.FromValidationErrorDetail(validator.GetErrors()));
        }

        var todoCreateResult = await todoService.CreateTodoAsync(todoEntity);
        if (todoCreateResult.Success == false)
        {
            var errorResponse = ErrorViewModelMapper.FromGenericResult(todoCreateResult);
            return Results.BadRequest(errorResponse);
        }
        else
        {
            TodoItem todoItem = TodoItemMapper.FromTodoEntity(todoCreateResult.Value);
            return Results.Created($"/todos/{todoEntity.Id}", todoItem);
        }
    })
    .Produces<TodoItem>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError)
    // .Accepts<CreateTodoViewModel>("application/json")
    .WithOpenApi(generatedOperation =>
    {
        generatedOperation.Summary = "Create todos";
        generatedOperation.Description = "Create todos";
        return generatedOperation;
    });

    // PUT: /todos/{id}
    app.MapPut("/todos/{id}", async ([FromServices] ITodoService todoService, int id, UpdateTodoItemViewModel updateTodoItemViewModel) =>
    {
        var todoEntity = CreateUpdateViewModelMapper.ToTodoEntity(id, updateTodoItemViewModel);
        var validator = new TodoEntityValidator();
        if (validator.IsValid(todoEntity) == false)
        {
            return Results.BadRequest(ErrorViewModelMapper.FromValidationErrorDetail(validator.GetErrors()));
        }

        var genericResult = await todoService.UpdateTodoByIdAsync(id, todoEntity);
        if (!genericResult.Success)
        {
            return Results.BadRequest(ErrorViewModelMapper.FromGenericResult(genericResult));
        }

        return Results.StatusCode(StatusCodes.Status200OK);
    })
    .Produces<TodoItem>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError)
    .WithOpenApi(generatedOperation =>
    {
        generatedOperation.Summary = "Update todos with id";
        generatedOperation.Description = "Update todos with id";
        var parameter = generatedOperation.Parameters[0];
        parameter.Description = "JSON for updating the todo";
        return generatedOperation;
    });

    // DELETE: /todos/{id}
    app.MapDelete("/todos/{id}", async ([FromServices] ITodoService todoService, int id) =>
    {
        var genericResponse = await todoService.DeleteTodoByIdAsync(id);
        if (!genericResponse.Success)
        {
            if (genericResponse.ErrorCode == GeneralErrorCode.NotFound)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.BadRequest(ErrorViewModelMapper.FromGenericResult(genericResponse));
            }
        }

        return Results.StatusCode(StatusCodes.Status200OK);
    })
    .WithOpenApi(generatedOperation =>
    {
        generatedOperation.Summary = "Delete todos with id";
        generatedOperation.Description = "Delete todos with id";
        var parameter = generatedOperation.Parameters[0];
        parameter.Description = "id for the todo to be deleted";
        return generatedOperation;
    });
}

// for integration test
public partial class Program { }
