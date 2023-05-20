# Todo API

## Tech Stack and development tool

- .NET 7
- Visual Studio
- Visual Studio Code
- dotnet cli

## API Design

## Common design

- Only json will be used request content and response content, not supporting XML
- status is passed as integer instead of string, 0 for non started, 1 for in progress, 2 for completed
- dueDate inside request and response JSON is in ISO 8601 format

For common error validation

```
{
  "errorCode": 3,
  "errorMessage": [
    "9999 is not valid"
  ]
}
```

### Get todo list with filtering and sorting

GET /todos?[dueDate=2023-05-21T13%3A00%3A00.0000000][&name=22][&status=1][&sortBy=name]

Return a list of todo item, can be filtered by name,status,dueDate, filtering is done by exact match. The list can be sorted by a column, either ascending or descending

Optional parameter in query string:
- dueDate in ISO DateString format
- name: either
- status: 0 for non started, 1 for in progress, 2 for completed
- sortBy: 

Example request:

```json
[
  {
    "id": 2,
    "name": "string",
    "description ": "string",
    "dueDate": "2023-05-19T23:26:24.058",
    "status": 0,
    "statusString": "Not Started"
  },
  {
    "id": 4,
    "name": "string",
    "description ": "string",
    "dueDate": "2023-05-19T23:31:35.436",
    "status": 0,
    "statusString": "Not Started"
  },
]
```

### Get todo details

GET /todos/2

Example response:

```json
[
  {
    "id": 2,
    "name": "string",
    "description ": "string",
    "dueDate": "2023-05-19T23:26:24.058",
    "status": 0,
    "statusString": "Not Started"
  }
]
```

### Create todo

POST /todos

POST JSON with the information of the todo item to be created, it will return the new created todo item in JSON

Example request:

```json
{
  "name": "string",
  "description": "string",
  "dueDate": "2023-05-20T22:00:29.787Z",
  "status": 0
}
```

Example response:

Status Code: 201

```json
{
  "id": 22,
  "name": "string",
  "description ": "string",
  "dueDate": "2023-05-21T13:00:00Z",
  "status": 0,
  "statusString": "Not Started"
}
```

### Update todo

PUT /todos/1

```response
{
  "id": 22,
  "name": "string",
  "description ": "string",
  "dueDate": "2023-05-21T13:00:00Z",
  "status": 0,
  "statusString": "Not Started"
}
```

Example response:

Status Code: 200
Content body: None

### Delete todo

DELETE /todos/1

Example response: 

Status Code: 200
Content body: None

## Application Design

The application is designed to be 3 layer. 

- JasonTodoCore: Main application logic, this mainly for POCO, high level interface of business logic such as TodoService, it doesn't have dependency except BCL
- JasonTodoInfrastructure: Data Access Layer, and the implemenation of TodoService, it depends on JasonTodoCore
- JasonTodoApi: The REST API end point for accepting the request, after accepting the HTTP request, it uses the service exported by JasonTodoCore and JasonTodoInfrastructure

## Unit Testing

- JasonTodoCore: Main application logic, this mainly for POCO, high level interface of business logic such as TodoService, it doesn't have dependency except BCL
- JasonTodoInfrastructure: Data Access Layer, and the implemenation of TodoService, it depends on JasonTodoCore
- JasonTodoApi: The REST API end point for accepting the request, after accepting the HTTP request, it uses the service exported by JasonTodoCore and JasonTodoInfrastructure

## Integrating Testing

Integration Testing in this context means REST API integration

## Provision Database

```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate

# Applied remaining database transaction
dotnet ef database update
```

# Exception handling

## Provision Database and maintaining database

 dotnet ef database update --connection "Data Source=C:\\Users\\J-LHY\\AppData\\Local\\blogging2.db"

## Language Feature
https://localhost:7124/swagger/index.html

## Common Pattern in code
- Nullable reference
- use const instead of enum for status
- ViewModel will be defined as record
- use required field keyword

## Trade off and design consideration
 
- I don't like to use AutoMapper, so I write the mapper myself, and write the testcase ...
- The TodoService implementation directly depends on Entity Framework context because I know it is easy to use SQLite and in-memory database with entity framework to test, 
otherwise I have to abstract a respositary pattern for testing its business logic

## Further Improvement (?)

Use JSON for integration testing

```json
var json = """
        {
            "name" : "API Name",
            "description" : "API Description",
            "status" : 0,
            "dueDate" : 
        }
         """;
```