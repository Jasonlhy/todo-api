namespace JasonTodoCore.Validators;

/// <summary>
/// Given a entity, check the logicical valid of each field
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IValidator<T>
{
    bool IsValid(T value);

    IEnumerable<ValidatorErrorDetail> GetErrors();
}
