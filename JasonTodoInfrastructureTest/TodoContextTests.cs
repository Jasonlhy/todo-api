using JasonTodoCore.Entities;
using JasonTodoCore.Exceptions;
using JasonTodoCore;
using JasonTodoInfrastructure.Data;
using JasonTodoInfrastructure.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Data.Common;

namespace JasonTodoInfrastructure.UnitTests;

public class TodoContextTests : IDisposable
{
    private DbConnection _connection;
    private DbContextOptions<TodoContext> _contextOptions;

    public TodoContextTests()
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

        // Seed records: 4 row
        context.AddRange(
            new Todo { Id = 1, Name = "Task 1", Description = "Task 1 desc", DueDate = new DateTime(2022, 12, 31), Status = TodoStatus.NOT_STARTED },
            new Todo { Id = 2, Name = "Task 2", Description = "Task 2 desc", DueDate = new DateTime(2023, 1, 1), Status = TodoStatus.IN_PROGRESS },
            new Todo { Id = 3, Name = "A Task 3", Description = "Task 3 desc", DueDate = new DateTime(2023, 1, 2), Status = TodoStatus.COMPLETED },
            new Todo { Id = 4, Name = "Task 4", Description = "Task 3 desc", DueDate = new DateTime(2023, 1, 3), Status = TodoStatus.COMPLETED }
        );

        context.SaveChanges();
    }

    TodoContext CreateContext() => new TodoContext(_contextOptions);

    [Test]
    public async Task GetTodoListAsync_WhenNoFilteringAndNoSortBy()
    {
        // Arrange
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);

        // Act 
        var todoList = await todoService.GetTodoListAsync(new Filtering(), "");

        // Assert
        Assert.That(todoList.Count, Is.EqualTo(4));
    }

    [Test]
    public async Task GetTodoListAsync_WhenFilteringByName_ReturnsFilteredTodoList()
    {
        // Arrange
        var filtering = new Filtering { Name = "Task 2" };
        string? sortByField = null;

        // Act
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var result = await todoService.GetTodoListAsync(filtering, sortByField);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Name, Is.EqualTo("Task 2"));
    }

    [Test]
    public async Task GetTodoListAsync_WhenFilteringByStatus_ReturnsFilteredTodoList()
    {
        // Arrange
        var filtering = new Filtering { Status = TodoStatus.COMPLETED };
        string? sortByField = null;
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);

        // Act
        var result = await todoService.GetTodoListAsync(filtering, sortByField);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.All(t => t.Status == TodoStatus.COMPLETED), Is.True);
    }

    [Test]
    public async Task GetTodoListAsync_WhenFilteringByDueDate_ReturnsFilteredTodoList()
    {
        // Arrange
        var filtering = new Filtering { DueDate = new DateTime(2022, 12, 31) };
        string sortByField = null;

        // Act
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);
        var result = await todoService.GetTodoListAsync(filtering, sortByField);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().DueDate, Is.EqualTo(new DateTime(2022, 12, 31)));
    }

    [Test]
    public async Task GetTodoListAsync_WhenSortingByName_ReturnsSortedTodoList()
    {
        // Arrange
        var filtering = new Filtering();
        string sortByField = "name";
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);

        // Act
        var result = await todoService.GetTodoListAsync(filtering, sortByField);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.First().Name, Is.EqualTo("A Task 3"));
        Assert.That(result.Last().Name, Is.EqualTo("Task 4"));
    }

    [Test]
    public async Task GetTodoListAsync_WhenSortingByStatus_ReturnsSortedTodoList()
    {
        // Arrange
        var filtering = new Filtering();
        string sortByField = "stats";
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);

        // Act
        var result = await todoService.GetTodoListAsync(filtering, sortByField);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.First().Status, Is.EqualTo(TodoStatus.NOT_STARTED));
        Assert.That(result.Last().Status, Is.EqualTo(TodoStatus.COMPLETED));
    }

    [Test]
    public async Task GetTodoListAsync_WhenSortingByDueDate_ReturnsSortedTodoList()
    {
        // Arrange
        var filtering = new Filtering();
        string sortByField = "dueDate";
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);

        // Act
        var result = await todoService.GetTodoListAsync(filtering, sortByField);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));
        Assert.That(result.First().DueDate, Is.EqualTo(new DateTime(2022, 12, 31)));
        Assert.That(result.Last().DueDate, Is.EqualTo(new DateTime(2023, 1, 3)));
    }

    [Test]
    public void GetTodoListAsync_WhenSortingByInvalidField_ThrowsInvalidSortingFieldException()
    {
        // Arrange
        var filtering = new Filtering();
        string sortByField = "invalidField";
        using var context = CreateContext();
        var todoService = new TodoService(context, NullLogger<TodoService>.Instance);

        // Act & Assert
        Assert.ThrowsAsync<InvalidSortingFieldException>(() => todoService.GetTodoListAsync(filtering, sortByField));
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
        Assert.That(createTodoResult.Value.Id, Is.EqualTo(5));
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