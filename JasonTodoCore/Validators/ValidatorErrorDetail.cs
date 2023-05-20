namespace JasonTodoCore.Validators;

public class ValidatorErrorDetail
{
    public required string FieldName { get; set; }

    // I am beging lazy here
    // Ideally this should have a error code indicate which rule it is violated for i18n
    public required string Message { get; set; }
}
