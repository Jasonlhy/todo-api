using JasonTodoApi.Mappers;
using JasonTodoCore.Constants;
using JasonTodoCore.Results;
using JasonTodoCore.Validators;

namespace JasonTodoAp.UnitiTests;

[TestFixture]
public class ErrorViewModelMapperTests
{
    [Test]
    public void FromGenericResult_ConvertsGenericResultToErrorViewModel()
    {
        // Arrange
        var genericResult = new GenericResult
        {
            ErrorCode = GeneralErrorCode.InvalidStatus,
            ErrorMessage = "-1 is not a valid status"
        };

        // Act
        var errorViewModel = ErrorViewModelMapper.FromGenericResult(genericResult);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(errorViewModel.ErrorCode, Is.EqualTo(genericResult.ErrorCode));
            Assert.That(errorViewModel.ErrorMessages.Count(), Is.EqualTo(1));
            Assert.That(errorViewModel.ErrorMessages.ElementAt(0), Is.EqualTo(genericResult.ErrorMessage));
        });
    }

    [Test]
    public void FromValidationErrorDetail_ConvertsValidatorErrorDetailToErrorViewModel()
    {
        // Arrange
        var validatorErrorDetails = new List<ValidatorErrorDetail>
        {
            new ValidatorErrorDetail
            {
                FieldName = "Name",
                Message = "The name field is required.",
            },
            new ValidatorErrorDetail
            {
                FieldName = "Description",
                Message = "The description field is too long."
            },
        };

        // Act
        var errorViewModel = ErrorViewModelMapper.FromValidationErrorDetail(validatorErrorDetails);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(errorViewModel.ErrorCode, Is.EqualTo(GeneralErrorCode.EntityValidationFailed));
            Assert.That(errorViewModel.ErrorMessages.Count(), Is.EqualTo(2));
            Assert.That(errorViewModel.ErrorMessages.ElementAt(0), Is.EqualTo(validatorErrorDetails[0].Message));
            Assert.That(errorViewModel.ErrorMessages.ElementAt(1), Is.EqualTo(validatorErrorDetails[1].Message));
        });
    }
}