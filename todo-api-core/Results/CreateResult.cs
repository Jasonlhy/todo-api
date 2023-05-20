namespace JasonTodoCore.Results;

public class CreateResult<T> : GenericResult
{
    public required T Value { get; set; }
}
