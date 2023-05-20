using JasonTodoCore.Entities;

namespace JasonTodoCoreTest;

public class TodoStatusHelperTests
{
    [Test]
    public void IsValidTodoStatus_ReturnTrue([Values(0, 1, 2)] int status)
    {
        Assert.True(TodoStatusHelper.IsValidTodoStatus(status));
    }

    [Test]
    public void IsValidTodoStatus_ReturnFalse([Values(-1, 4)] int status)
    {
        Assert.False(TodoStatusHelper.IsValidTodoStatus(status));
    }
}