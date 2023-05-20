using JasonTodoCore.Entities;
using JasonTodoCore.Validators;
using System.Text;

namespace JasonTodoCoreTest;

public class TodoEntityValidatorTests
{
    private static string CreateLongString(string stringTemplate, int repeat)
    {
        StringBuilder stringBuilder = new StringBuilder(stringTemplate);
        for (int i = 0; i < repeat; i++)
        {
            stringBuilder.Append(stringTemplate);
        }

        return stringBuilder.ToString();
    }

    [Test]
    public void IsValid_ReturTrue()
    {
        var todoEntity = new TodoEntity()
        {
            Description = "Test Desc",
            Name = "Test Name",
            DueDate = DateTime.Now,
            Status = 1,
        };

        var validator = new TodoEntityValidator();
        var isValid = validator.IsValid(todoEntity);
        Assert.True(isValid);
    }

    [Test]
    public void IsValid_ReturnFalse_InvalidName()
    {
        var name = CreateLongString("name", 100);
        var todoEntity = new TodoEntity()
        {
            Description = "Test Desc",
            Name = name,
            DueDate = DateTime.Now,
            Status = 1,
        };

        var validator = new TodoEntityValidator();
        var isValid = validator.IsValid(todoEntity);
        Assert.False(isValid);

        var error = validator.GetErrors().ElementAt(0);
        Assert.That(error.FieldName, Is.EqualTo("Name"));
        Assert.That(error.Message, Is.EqualTo("Name is longer than the limit: 100"));
    }

    [Test]
    public void IsValid_ReturnFalse_MissingName()
    {
        var todoEntity = new TodoEntity()
        {
            Description = "Test Desc",
            Name = "",
            DueDate = DateTime.Now,
            Status = 1,
        };

        var validator = new TodoEntityValidator();
        var isValid = validator.IsValid(todoEntity);
        Assert.False(isValid);

        var error = validator.GetErrors().ElementAt(0);
        Assert.That(error.FieldName, Is.EqualTo("Name"));
        Assert.That(error.Message, Is.EqualTo("Name is missing"));
    }

    [Test]
    public void IsValid_ReturnFalse_InvalidDescription()
    {
        var desc = CreateLongString("desc", 100);

        var todoEntity = new TodoEntity()
        {
            Description = desc,
            Name = "Test Desc",
            DueDate = DateTime.Now,
            Status = 1,
        };

        var validator = new TodoEntityValidator();
        var isValid = validator.IsValid(todoEntity);
        Assert.False(isValid);

        var error = validator.GetErrors().ElementAt(0);
        Assert.That(error.FieldName, Is.EqualTo("Description"));
        Assert.That(error.Message, Is.EqualTo("Description is longer than the limit: 255"));
    }

    [Test]
    public void IsValid_ReturnFalse_MissingDescription()
    {
        var todoEntity = new TodoEntity()
        {
            Description = "",
            Name = "name",
            DueDate = DateTime.Now,
            Status = 1,
        };

        var validator = new TodoEntityValidator();
        var isValid = validator.IsValid(todoEntity);
        Assert.False(isValid);

        var error = validator.GetErrors().ElementAt(0);
        Assert.That(error.FieldName, Is.EqualTo("Description"));
        Assert.That(error.Message, Is.EqualTo("Description is missing"));
    }

    [Test]
    public void IsValid_ReturnFalse_InvalidStatus()
    {
        var todoEntity = new TodoEntity()
        {
            Description = "Test",
            Name = "Name",
            DueDate = DateTime.Now,
            Status = -1,
        };

        var validator = new TodoEntityValidator();
        var isValid = validator.IsValid(todoEntity);
        Assert.False(isValid);

        var error = validator.GetErrors().ElementAt(0);
        Assert.That(error.FieldName, Is.EqualTo("Status"));
        Assert.That(error.Message, Is.EqualTo("-1 is not valid status"));
    }
}