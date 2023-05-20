using JasonTodoApi.ViewModels;
using JasonTodoCore.Constants;
using JasonTodoCore.Results;
using JasonTodoCore.Validators;

namespace JasonTodoApi.Mappers;

public static class ErrorViewModelMapper
{
    public static ErrorViewModel FromGenericResult(GenericResult genericResult)
    {
        return new ErrorViewModel()
        {
            ErrorCode = genericResult.ErrorCode,
            ErrorMessages = new string[] { genericResult.ErrorMessage! },
        };
    }


    public static ErrorViewModel FromValidationErrorDetail(IEnumerable<ValidatorErrorDetail> validatorErrorDetails)
    {
        return new ErrorViewModel()
        {
            ErrorCode = GeneralErrorCode.EntityValidationFailed,
            ErrorMessages = validatorErrorDetails.Select(d => d.Message).ToList(),
        };
    }
}