using JasonTodoApi.ViewModels;
using JasonTodoCore.Constants;
using JasonTodoCore.Entities;
using JasonTodoInfrastructure.Data;
using JasonTodoInfrastructure.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace JasonTodoApi.IntegrationTests
{
    public class TodoRestApiTests : IDisposable
    {
        private JasonTodoApplication jasonTodoApplication;
        private HttpClient httpClient; // httpClient is thread-safe

        public void Dispose()
        {
            jasonTodoApplication.Dispose();
            httpClient.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            // Create seed data for the todo application
            jasonTodoApplication = new JasonTodoApplication();

            using (var scope = jasonTodoApplication.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                using var todoContext = provider.GetRequiredService<TodoContext>();
                todoContext.Database.EnsureCreated();

                // Seed records with 4 data
                todoContext.AddRange(
                    new Todo { Id = 1, Name = "Task 1", Description = "Task 1 desc", DueDate = new DateTime(2022, 12, 31), Status = TodoStatus.NOT_STARTED },
                    new Todo { Id = 2, Name = "Task 2", Description = "Task 2 desc", DueDate = new DateTime(2023, 1, 1), Status = TodoStatus.IN_PROGRESS },
                    new Todo { Id = 3, Name = "A Task 3", Description = "Task 3 desc", DueDate = new DateTime(2023, 1, 2), Status = TodoStatus.COMPLETED },
                    new Todo { Id = 4, Name = "Task 4", Description = "Task 4 desc", DueDate = new DateTime(2023, 1, 3), Status = TodoStatus.COMPLETED }
                );

                todoContext.SaveChanges();
            }

            httpClient = jasonTodoApplication.CreateClient();
        }

        [Test]
        public async Task GetTodoList()
        {
            var todoList = await httpClient.GetFromJsonAsync<IEnumerable<TodoItem>>("/todos");
            Assert.That(todoList, Is.Not.Null);
            Assert.That(todoList.Count(), Is.EqualTo(4));

            var response = await httpClient.GetAsync("/todos?status=999");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            var errorViewModel = System.Text.Json.JsonSerializer.Deserialize<ErrorViewModel>(response.Content.ReadAsStream());
            Assert.That(errorViewModel, Is.Not.Null);
            Assert.That(errorViewModel.ErrorCode, Is.EqualTo(GeneralErrorCode.InvalidStatus));
        }

        [Test]
        public async Task GetTodoList_WithFilter()
        {
            var todoList = await httpClient.GetFromJsonAsync<IEnumerable<TodoItem>>("/todos?name=Task 4");
            Assert.That(todoList, Is.Not.Null);
            Assert.That(todoList.Count(), Is.EqualTo(1));

            // TodoItem is a record, so it can also be compared with values...
            Assert.That(todoList.ElementAt(0), Is.EqualTo(new TodoItem
            {
                Id = 4,
                Name = "Task 4",
                Description = "Task 4 desc",
                DueDate = new DateTime(2023, 1, 3),
                Status = 2,
                StatusString = "Completed",
            }));
        }

        [Test]
        public async Task GetTodoList_WithSortBy()
        {
            var todoList = await httpClient.GetFromJsonAsync<IEnumerable<TodoItem>>("/todos?sortBy=name");
            Assert.That(todoList, Is.Not.Null);
            Assert.That(todoList.Count(), Is.EqualTo(4));

            // TodoItem is a record, so it can also be compared with values...
            Assert.That(todoList.ElementAt(0), Is.EqualTo(new TodoItem
            {
                Id = 3,
                Name = "A Task 3",
                Description = "Task 3 desc",
                DueDate = new DateTime(2023, 1, 2),
                Status = 2,
                StatusString = "Completed",
            }));
        }

        [Test]
        public async Task GetTodoList_WithSortBy_NotSupportField()
        {
            var response = await httpClient.GetAsync("/todos?sortBy=notsupportfield");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task GetTodo3_Ok()
        {
            var todoItem = await httpClient.GetFromJsonAsync<TodoItem>("/todos/3");
            Assert.That(todoItem, Is.Not.Null);
            Assert.That(todoItem.Id, Is.EqualTo(3));
            Assert.That(todoItem.Name, Is.EqualTo("A Task 3"));
            Assert.That(todoItem.Description, Is.EqualTo("Task 3 desc"));
            Assert.That(todoItem.Status, Is.EqualTo(TodoStatus.COMPLETED));
        }

        [Test]
        public async Task GetTodo_NonExistId()
        {

            // Not exists id
            var response = await httpClient.GetAsync("/todos/999");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreateTodoList_Created()
        {
            var response = await httpClient.PostAsJsonAsync("/todos", new CreateTodoViewModel
            {
                Name = "API Name",
                Description = "API Description",
                DueDate = new DateTime(2023, 5, 21),
            });
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Created));

            // Happy path will return the created todo item
            var createdTodoItem = System.Text.Json.JsonSerializer.Deserialize<TodoItem>(response.Content.ReadAsStream());
            Assert.That(createdTodoItem, Is.Not.Null);

            Assert.That(createdTodoItem.Id, Is.EqualTo(5));
            Assert.That(createdTodoItem.Name, Is.EqualTo("API Name"));
            Assert.That(createdTodoItem.Status, Is.EqualTo(0));
            Assert.That(createdTodoItem.DueDate, Is.EqualTo(new DateTime(2023, 5, 21)));

            // Find the todo item agian
            var todoItem = await httpClient.GetFromJsonAsync<TodoItem>("/todos/5");
            Assert.NotNull(todoItem);
            Assert.That(todoItem.Name, Is.EqualTo("API Name"));
            Assert.That(todoItem.Description, Is.EqualTo("API Description"));
            Assert.That(todoItem.Status, Is.EqualTo(0));
            Assert.That(createdTodoItem.DueDate, Is.EqualTo(new DateTime(2023, 5, 21)));
        }

        [Test]
        public async Task UpdateTodoList_Ok()
        {
            var response = await httpClient.PutAsJsonAsync("/todos/1", new UpdateTodoItemViewModel
            {
                Name = "New API Name",
                Description = "New API Description",
                Status = 1,
                DueDate = new DateTime(2023, 5, 1),
            });
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            // Find the todo item agian
            var todoItem = await httpClient.GetFromJsonAsync<TodoItem>("/todos/1");
            Assert.NotNull(todoItem);
            Assert.That(todoItem.Name, Is.EqualTo("New API Name"));
            Assert.That(todoItem.Description, Is.EqualTo("New API Description"));
            Assert.That(todoItem.Status, Is.EqualTo(1));
            Assert.That(todoItem.DueDate, Is.EqualTo(new DateTime(2023, 5, 1)));
        }

        [Test]
        public async Task UpdateTodoList_NonExistId()
        {
            var response = await httpClient.PutAsJsonAsync("/todos/99999", new UpdateTodoItemViewModel
            {
                Name = "New API Name",
                Description = "New API Description",
                Status = 1,
                DueDate = new DateTime(2023, 5, 1),
            }); 
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task DeleteTodoList_Ok()
        {
            var response = await httpClient.DeleteAsync("/todos/1");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

            // Find the todo item agian
            var todoList = await httpClient.GetFromJsonAsync<IEnumerable<TodoItem>>("/todos");
            Assert.That(todoList!.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task DeleteTodoList_NonExistId()
        {
            var response = await httpClient.DeleteAsync("/todos/99");
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }
    }
}