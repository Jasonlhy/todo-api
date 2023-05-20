namespace JasonTodoCore.Entities;

public static class TodoStatusHelper
{
    public static bool IsValidTodoStatus(int number)
    {
        return (number is 0 or 1 or 2);
    }

    public static string ToString(int number)
    {
        return number switch
        {
            0 => "Not Started",
            1 => "In Progress",
            2 => "Completed",
            _ => throw new InvalidOperationException("number is not a valid todo status: " + number),
        };
    }
}
