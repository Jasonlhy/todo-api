namespace JasonTodoApiTests;

public class TodoItemMapperTests
{
    [Test]
    public void FromTodoEntity_ReturnsCorrectTodoItem()
    {
        // Arrange
        var todoEntity = new TodoEntity
        {
            Id = 1,
            Name = "Test Todo",
            Description = "This is a test todo item",
            DueDate = new DateTime(2022, 1, 1),
            Status = 1
        };

        // Act
        var todoItem = TodoItemMapper.FromTodoEntity(todoEntity);

        // Assert
        Assert.That(todoItem.Id, Is.EqualTo(todoEntity.Id));
        Assert.That(todoItem.Name, Is.EqualTo(todoEntity.Name));
        Assert.That(todoItem.Description, Is.EqualTo(todoEntity.Description));
        Assert.That(todoItem.DueDate, Is.EqualTo(todoEntity.DueDate));
        Assert.That(todoItem.Status, Is.EqualTo(todoEntity.Status));
        Assert.That(todoItem.StatusString, Is.EqualTo("In Progress"));
    }

    [Test]
    public void FromTodoEntityList_ReturnsCorrectTodoItemList()
    {
        // Arrange
        var todoEntityList = new List<TodoEntity>
        {
            new TodoEntity
            {
                Id = 1,
                Name = "Test Todo 1",
                Description = "This is a test todo item 1",
                DueDate = new DateTime(2022, 1, 1),
                Status = 1
            },
            new TodoEntity
            {
                Id = 2,
                Name = "Test Todo 2",
                Description = "This is a test todo item 2",
                DueDate = new DateTime(2022, 2, 1),
                Status = 2
            }
        };

        // Act
        var todoItemList = TodoItemMapper.FromTodoEntityList(todoEntityList);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(todoItemList.Count(), Is.EqualTo(2));

            var item1 = todoItemList.ElementAt(0);
            Assert.That(item1.Id, Is.EqualTo(1));
            Assert.That(item1.Name, Is.EqualTo("Test Todo 1"));
            Assert.That(item1.Description, Is.EqualTo("This is a test todo item 1"));
            Assert.That(item1.DueDate, Is.EqualTo(new DateTime(2022, 1, 1)));
            Assert.That(item1.Status, Is.EqualTo(1));
            Assert.That(item1.StatusString, Is.EqualTo("In Progress"));

            var item2 = todoItemList.ElementAt(1);
            Assert.That(item2.Id, Is.EqualTo(2));
            Assert.That(item2.Name, Is.EqualTo("Test Todo 2"));
            Assert.That(item2.Description, Is.EqualTo("This is a test todo item 2"));
            Assert.That(item2.DueDate, Is.EqualTo(new DateTime(2022, 2, 1)));
            Assert.That(item2.Status, Is.EqualTo(2));
            Assert.That(item2.StatusString, Is.EqualTo("Completed"));
        });
    }
}
