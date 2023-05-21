using JasonTodoApi.ViewModels;
using JasonTodoCore.Constants;
using JasonTodoCore.Entities;
namespace JasonTodoApi.Mappers;

/// <summary>
/// Mapper to convert between other JSON view model and domain objects
/// </summary>
public static class CreateUpdateViewModelMapper
{
    public static TodoEntity ToTodoEntity(CreateTodoViewModel createTodoViewModel)
    {
        // Id is automatically created
        // It is set to not started by default
        return new TodoEntity
        {
            Name = createTodoViewModel.Name,
            Description = createTodoViewModel.Description,
            DueDate = createTodoViewModel.DueDate,
            Status = TodoStatus.NOT_STARTED,
        };
    }

    public static TodoEntity ToTodoEntity(int id, UpdateTodoItemViewModel updateToddoViewModel)
    {
        return new TodoEntity
        {
            Id = id,
            Name = updateToddoViewModel.Name,
            Description = updateToddoViewModel.Description,
            DueDate = updateToddoViewModel.DueDate,
            Status = updateToddoViewModel.Status,
        };
    }
}
