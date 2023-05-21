namespace JasonTodoCore.Entities;

public static class SortingHelper
{
    public static bool IsSupportedSortingField(string sortingField)
    {
        return sortingField == "name" || sortingField == "stats" || sortingField == "dueDate";
    }
}
