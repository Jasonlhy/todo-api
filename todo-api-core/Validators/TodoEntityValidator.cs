using JasonTodoCore.Constants;
using JasonTodoCore.Entities;

namespace JasonTodoCore.Validators;

public class TodoEntityValidator : IValidator<TodoEntity>
{
    public List<ValidatorErrorDetail> Erros;

    public TodoEntityValidator()
    {
        Erros = new List<ValidatorErrorDetail>();
    }

    public IEnumerable<ValidatorErrorDetail> GetErrors()
    {
        return Erros;
    }

    public bool IsValid(TodoEntity entity)
    {
        bool valid = true;

        if (string.IsNullOrEmpty(entity.Name))
        {
            Erros.Add(new ValidatorErrorDetail
            {
                FieldName = "Name",
                Message = "Name is missing",
            });
            valid = false;
        }
        else if (entity.Name.Length > TodoConstant.NAME_LENGTH)
        {
            Erros.Add(new ValidatorErrorDetail
            {
                FieldName = "Name",
                Message = $"Name is longer than the limit: {TodoConstant.NAME_LENGTH}",
            });
            valid = false;
        }

        if (string.IsNullOrEmpty(entity.Description))
        {
            Erros.Add(new ValidatorErrorDetail
            {
                FieldName = "Description",
                Message = "Description is missing",
            });
            valid = false;
        }
        else if (entity.Description.Length > TodoConstant.DESCRIPTION_LENGTH)
        {
            Erros.Add(new ValidatorErrorDetail
            {
                FieldName = "Description",
                Message = $"Description is longer than the limit: {TodoConstant.DESCRIPTION_LENGTH}",
            });
            valid = false;
        }

        if (!TodoStatusHelper.IsValidTodoStatus(entity.Status))
        {
            Erros.Add(new ValidatorErrorDetail
            {
                FieldName = "Status",
                Message = $"{entity.Status} is not valid status",
            });
            valid = false;
        }

        return valid;
    }
}
