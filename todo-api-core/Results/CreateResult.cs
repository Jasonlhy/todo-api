namespace JasonTodoCore.Results;

public class CreateResult<T> : GenericResult
{
    // public bool Success { get; set; }

    public required T Value { get; set; }
}
