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

## Provision Database and maintaining database

 dotnet ef database update --connection "Data Source=C:\\Users\\J-LHY\\AppData\\Local\\blogging2.db"

## Language Feature
https://localhost:7124/swagger/index.html

- Nullable reference
- Dto -> ViewModel 
- use const instead of enum for status

// for json API
 - public required int Status { get; set; }
- For missing field

# Unit Test
- 

## Unit Test

## Integration Test
- It 

# FilterBy, sortedBy

# Trade off and design consideration
- The service

## Improvement
-       var xml = """
                    {
                        "name" : "API Name",
                        "description" : "API Description",
                        "status" : 0,
                        "dueDate" : 
                    }
                    """;
## Logging

-  Task<IEnumerable<TodoEntity>> GetTodos(string[] sortByField, int[] filterByStatus);
/// Note: I don't like to use AutoMapper, the methods relies on method overloading so I don't need to type the long name