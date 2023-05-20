using JasonTodoCore.Constants;
using JasonTodoCore.Entities;

namespace JasonTodoCore.Validators;

public class TodoEntityValidator : IValidator<TodoEntity>
{
    public List<ValidatorErrorDetail> errorDetailList;

    public TodoEntityValidator()
    {
        errorDetailList = new List<ValidatorErrorDetail>();
    }

    public IEnumerable<ValidatorErrorDetail> GetErrors()
    {
        return errorDetailList;
    }

    public bool IsValid(TodoEntity entity)
    {
        bool valid = true;

        if (string.IsNullOrEmpty(entity.Name))
        {
            errorDetailList.Add(new ValidatorErrorDetail
            {
                FieldName = "Name",
                Message = "Name is missing",
            });
            valid = false;
        }
        else if (entity.Name.Length > TodoConstant.NAME_LENGTH)
        {
            errorDetailList.Add(new ValidatorErrorDetail
            {
                FieldName = "Name",
                Message = $"Name is longer than the limit: {TodoConstant.NAME_LENGTH}",
            });
            valid = false;
        }

        if (string.IsNullOrEmpty(entity.Description))
        {
            errorDetailList.Add(new ValidatorErrorDetail
            {
                FieldName = "Description",
                Message = "Description is missing",
            });
            valid = false;
        }
        else if (entity.Description.Length > TodoConstant.DESCRIPTION_LENGTH)
        {
            errorDetailList.Add(new ValidatorErrorDetail
            {
                FieldName = "Description",
                Message = $"Description is longer than the limit: {TodoConstant.DESCRIPTION_LENGTH}",
            });
            valid = false;
        }

        if (!TodoStatusHelper.IsValidTodoStatus(entity.Status))
        {
            errorDetailList.Add(new ValidatorErrorDetail
            {
                FieldName = "Status",
                Message = $"{entity.Status} is not valid status",
            });
            valid = false;
        }

        return valid;
    }
}
