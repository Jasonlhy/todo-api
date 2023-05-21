# Todo API

Simple Todo API, with unit testing with Sqlite in Memory, integration testing with Sqlite in Memory. It also have basic CI in the github Action.

## Tech Stack and development tool

- .NET 7
- Visual Studio
- Visual Studio Code
- dotnet cli
- git
- SQLite

**SQLite**

- For development and production, the database is using `todo.db` inside `$ENV:LOCALAPPDATA`
- For unit testing, integration testing, the database is using SQLite in memory

## Getting Started for development

The easiest way to try the project is to clone into the namespace

- https://localhost:7124/swagger/index.html
- http://localhost:5162/swagger/index.html

### Windows

Install git

```
winget install --id git
```

[Download Visual Studio 2022](https://visualstudio.microsoft.com/vs/) then open the solution

[Download Visual Studio code and install C# extensions](https://code.visualstudio.com/docs/languages/csharp) and open the folder

**Provision database for development**

Run following in powershell, it will generate the `todo.db` inside `$ENV:LOCALAPPDATA`

```powershell
dotnet tool install --global dotnet-ef
dotnet ef database update -p ./JasonTodoInfrastructure -s ./JasonTodoApi
```

### Linux

```
apt-get install git
```

### Github

It is the easiest way to test the project, clone the project into the codespaces

```bash
dotnet run JasonTodoApi
```

## API Design

## Common design

- Only json will be used request content and response content, not supporting XML
- status is passed as integer instead of string, 0 for non started, 1 for in progress, 2 for completed
- dueDate inside request and response JSON is in ISO 8601 format

For common error validation error, it will return 400 and in following structure

```json
{
  "errorCode": 3,
  "errorMessages": [
    "9999 is not valid status"
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
- sortBy: possible values: name, dueDate, stats

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
  "status": 0
}
```

Example response:

Status Code: 200
Content body: None

### Delete todo

Given a todo id, delete the todo item.

Example request:

DELETE /todos/1

Example response: 

Status Code: 200
Content body: None

Example request 2  (Non existing Id)

DELETE /todos/9999

Example response 2:

Status Code: 404
Content body: None

## Application Design

The application is designed to be 3 layer. 

- JasonTodoCore: Main application logic, this mainly for POCO, high level interface such as TodoService, constant, validation rule, it doesn't have dependency except BCL
- JasonTodoInfrastructure: Data Access Layer, and the implementation of TodoService, it depends on JasonTodoCore
- JasonTodoApi: The REST API end point for accepting the request, after accepting the HTTP request, it uses the library from JasonTodoCore and JasonTodoInfrastructure to handle the logic then response to user

### Design 

- Main logic is handled in the service layer
- I added some length constraint on the data
- API -> Validation -> Service Layer
- Infrastructure layer is for external resource such as database


## Unit Testing

- JasonTodoCore: Test the mapper, validation logic
- JasonTodoInfrastructure: Test the implementation of IGEService, the unit test is running with SQLite in memory, the data is isolated between each test case
- JasonTodoApi: Test the mapper

## Integration Testing

REST API integration testing, it will start up a test server, and then make REST API call to the server and assert the result such as response body and status code

## Entity Framework Core migration

THe initial database schema is created with Entity Framework Core migration

```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
```

## Common Pattern in code
- Nullable reference
- use const instead of enum for status
- ViewModel will be defined as record
- use required field keyword

## Trade off and design consideration
 
- I don't like to use AutoMapper, so I write the mapper myself, and write the test case ...
- The TodoService implementation directly depends on Entity Framework context because I know it is easy to use SQLite and in-memory database with entity framework to test.
I can put the TodoService in the Core but I have to abstract a repository pattern for providing the data. [See](aaa)
- I don't like the provision and migration of database is included in the application logic, which may make it hard to test, so you need to run the command before running the application 
- Because it is a demo project, I don't want to connect to external database due to Cost. I also hard code the path of the database. Ideally it should read from the IConfiguration (Although codespace can create database with docker compose now)

## Further Improvement (?)

- Code coverage report
- Perhaps using JSON for integration testing
- Standalone program for testing the performance such as JMeteter and K6
- CD to some app service