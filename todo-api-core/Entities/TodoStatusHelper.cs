namespace JasonTodoCore.Entities;

public static class TodoStatusHelper
{
    public static bool IsValidTodoStatus(int number)
    {
        return (number == 1 || number == 2 || number == 3);
    }
}
