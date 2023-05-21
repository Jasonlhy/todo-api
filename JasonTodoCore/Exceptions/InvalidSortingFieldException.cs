namespace JasonTodoCore.Exceptions;

public class InvalidSortingFieldException : Exception
{
    public InvalidSortingFieldException(string fieldName) : base($"{fieldName} cannot be sorted") 
    {
        
    }
}
