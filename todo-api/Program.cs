using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_api.Db;
using todo_api.Services;
using todo_api.Services.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddDbContext<TodoContext>(
    options =>
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "blogging.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
);
builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-7.0
// GET: /todos
app.MapGet("/todos", async ([FromServices] ITodoService todoService, [FromQuery] string? filter, [FromQuery] string? sortBy) =>
{
    var todoLists = await todoService.GetTodos();
    return Results.Ok(todoLists);
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
    var todo = await todoService.GetTodoById(id);
    if (todo is null)
    {
        return Results.StatusCode(StatusCodes.Status404NotFound);
    }

    return Results.Ok(todo);
})
.Produces<TodoItem>(StatusCodes.Status200OK)
.Produces<TodoItem>(StatusCodes.Status404NotFound)
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
    TodoItem? todoCreate = await todoService.CreateTodo(createTodoViewModel);
    if (todoCreate is null)
    {
        return Results.StatusCode(StatusCodes.Status400BadRequest);
    }
    else
    {
        return Results.StatusCode(StatusCodes.Status201Created);
    }
})
.Produces<TodoItem>(StatusCodes.Status400BadRequest)
.Produces<TodoItem>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status500InternalServerError)
.Accepts<CreateTodoViewModel>("application/json")
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Summary = "Create todos";
    generatedOperation.Description = "Create todos";
    return generatedOperation;
});

// PUT: /todos/{id}
app.MapPut("/todos/{id}", async ([FromServices] ITodoService todoService, int id, UpdateTodoItemViewModel updateTodoItemViewModel) =>
{
    var existingTodo = await todoService.UpdateTodoById(id, updateTodoItemViewModel);
    if (existingTodo == null)
    {
        return Results.StatusCode(StatusCodes.Status404NotFound);
    }

    return Results.StatusCode(StatusCodes.Status200OK);
})
.Produces<TodoItem>(StatusCodes.Status404NotFound)
.Produces<TodoItem>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Summary = "Update todos with id";
    generatedOperation.Description = "Update todos with id";
    var parameter = generatedOperation.Parameters[0];
    parameter.Description = "JSON for updating the view model";
    return generatedOperation;
});

// DELETE: /todos/{id}
app.MapDelete("/todos/{id}", async ([FromServices] ITodoService todoService, int id) =>
{
    var genericResponse = await todoService.DeleteTodoById(id);
    if (genericResponse.Success)
    {
        return Results.StatusCode(StatusCodes.Status200OK);
    }

    return Results.StatusCode(StatusCodes.Status400BadRequest);
});

app.Run();

