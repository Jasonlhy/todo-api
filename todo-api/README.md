# Persisnt layer

# Application design
- System (otherwise you need to add additoinal logic )

# Layout

## Archit

API -> Service Layer -> TodoContext


## Database

```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate

# Applied remaining database transaction
dotnet ef database update
```

# Exception handling

## Some tradeoff
- I didn't implement
- I

## 

## Logging

## Language Feature
- Nullable reference
- Dto -> ViewModel 
- use const instead of enum for status

// for json API
 - public required int Status { get; set; }

# FilterBy, sortedBy

# Trade off
-  Task<IEnumerable<TodoEntity>> GetTodos(string[] sortByField, int[] filterByStatus);
/// Note: I don't like to use AutoMapper, the methods relies on method overloading so I don't need to type the long name