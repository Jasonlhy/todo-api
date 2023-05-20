using JasonTodoCore.Entities;
using JasonTodoInfrastructure.Data.Models;
using JasonTodoInfrastructure;

namespace JasonTodoInfrastructureTest;

[TestFixture]
public class TodoEntityMapperTests
{
    [Test]
    public void ToTodoTest()
    {
        // Arrange
        var todoEntity = new TodoEntity
        {
            Name = "Test",
            Description = "Test Description",
            DueDate = DateTime.Now,
            Status = 0
        };

        // Act
        var todo = TodoEntityMapper.ToTodo(todoEntity);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(todoEntity.Name, Is.EqualTo(todo.Name));
            Assert.That(todoEntity.Description, Is.EqualTo(todo.Description));
            Assert.That(todoEntity.DueDate, Is.EqualTo(todo.DueDate));
            Assert.That(todoEntity.Status, Is.EqualTo(todo.Status));
        });
    }

    [Test]
    public void UpdateTodoTest()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test",
            Description = "Test Description",
            DueDate = DateTime.Now,
            Status = 0
        };

        var todoEntity = new TodoEntity
        {
            Name = "Updated Test",
            Description = "Updated Test Description",
            DueDate = DateTime.Now.AddDays(1),
            Status = 1
        };

        // Act
        TodoEntityMapper.UpdateTodo(todo, todoEntity);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(todoEntity.Name, Is.EqualTo(todo.Name));
            Assert.That(todoEntity.Description, Is.EqualTo(todo.Description));
            Assert.That(todoEntity.DueDate, Is.EqualTo(todo.DueDate));
            Assert.That(todoEntity.Status, Is.EqualTo(todo.Status));
        });
    }

    [Test]
    public void FromTodoTest()
    {
        // Arrange
        var todo = new Todo
        {
            Id = 1,
            Name = "Test",
            Description = "Test Description",
            DueDate = DateTime.Now,
            Status = 2
        };

        // Act
        var todoEntity = TodoEntityMapper.FromTodo(todo);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(todoEntity.Id, Is.EqualTo(todo.Id));
            Assert.That(todoEntity.Name, Is.EqualTo(todo.Name));
            Assert.That(todoEntity.Description, Is.EqualTo(todo.Description));
            Assert.That(todoEntity.DueDate, Is.EqualTo(todo.DueDate));
            Assert.That(todoEntity.Status, Is.EqualTo(todo.Status));
        });
    }
}
