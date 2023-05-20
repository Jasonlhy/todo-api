using JasonTodoInfrastructure;
using JasonTodoInfrastructure.Data;
using JasonTodoInfrastructure.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data.Common;

namespace JasonTodoInfrastructureTest;

public class Tests : IDisposable
{
    private DbConnection _connection;
    private DbContextOptions<TodoContext> _contextOptions;

    public Tests()
    {

    }

    public void Dispose() => _connection.Dispose();

    [SetUp]
    public void Setup()
    {
        // Using https://learn.microsoft.com/en-us/ef/core/testing/testing-without-the-database#sqlite-in-memory
        // It provides easy isolation between tests, and does not require dealing with actual SQLite files, lazy ....

        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<TodoContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new TodoContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            // viewCommand.ExecuteNonQuery();
        }

        // Seed records: 2 row
        context.AddRange(
            new Todo { Id = 1, Name = "Task 1", Description = "Task 1 desc", DueDate = DateTime.Now, Status = 0 },
            new Todo { Id = 2, Name = "Task 2", Description = "Task 2 desc", DueDate = DateTime.Now, Status = 0 }
        );

        context.SaveChanges();
    }

    TodoContext CreateContext() => new TodoContext(_contextOptions);

    [Test]
    public async Task GetTodo_Return2Elements()
    {
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var todoList = await todoService.GetTodoListAsync(new JasonTodoCore.Entities.Filtering
        {
        }, "");


        Assert.That(todoList.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetTodo1_ShouldReturnTodo()
    {
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var todo = await todoService.GetTodoByIdAsync(1);
        Assert.That(todo, Is.Not.Null);
        Assert.That(todo.Description, Is.EqualTo("Task 1 desc"));
    }

    [Test]
    public async Task GetTodo999_ShouldReturnNull()
    {
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var todo = await todoService.GetTodoByIdAsync(999);
        Assert.That(todo, Is.Null);
    }

    [Test]
    public async Task InsertTodo_ShouldBeSuccess()
    {
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var createTodoResult = await todoService.CreateTodoAsync(new JasonTodoCore.Entities.TodoEntity
        {
            // id is not needed for creating todo
            Name = "jason entity",
            Description = "jason description",
            Status = 0,
            DueDate = DateTime.Now,
        });

        Assert.That(createTodoResult.Success, Is.EqualTo(true));
        Assert.That(createTodoResult.Value.Id, Is.EqualTo(3));
    }

    [Test]
    public async Task DeleteTodo_ShouldBeSuccess()
    {
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var deleteResult = await todoService.DeleteTodoByIdAsync(1);

        Assert.That(deleteResult.Success, Is.EqualTo(true));
    }

    [Test]
    public async Task UpdateTodo_ShouldBeSuccess()
    {
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var deleteResult = await todoService.UpdateTodoByIdAsync(1,
            new JasonTodoCore.Entities.TodoEntity
            {
                Name = "Task 1",
                Description = "New Task 1 desc",
                DueDate = DateTime.Now,
                Status = 0
            }
        );

        Assert.That(deleteResult.Success, Is.EqualTo(true));

        var todo = await todoService.GetTodoByIdAsync(1);
        Assert.That(todo, Is.Not.Null);
        Assert.That(todo.Description, Is.EqualTo("New Task 1 desc"));
    }
}