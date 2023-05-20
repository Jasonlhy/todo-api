namespace JasonTodoApiTests
{
    public class CreateUpdateViewModelMapperTests
    {
        [Test]
        public void ToTodoEntity_WithCreateTodoViewModel_ReturnsTodoEntity()
        {
            // Arrange
            var createTodoViewModel = new CreateTodoViewModel
            {
                Name = "Test Todo",
                Description = "This is a test todo",
                DueDate = new DateTime(2022, 1, 1),
                Status = 0
            };

            // Act
            var result = CreateUpdateViewModelMapper.ToTodoEntity(createTodoViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<TodoEntity>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(createTodoViewModel.Name));
                Assert.That(result.Description, Is.EqualTo(createTodoViewModel.Description));
                Assert.That(result.DueDate, Is.EqualTo(createTodoViewModel.DueDate));
                Assert.That(result.Status, Is.EqualTo(createTodoViewModel.Status));
            });
        }

        [Test]
        public void ToTodoEntity_WithUpdateTodoItemViewModel_ReturnsTodoEntity()
        {
            // Arrange
            var id = 1;
            var updateTodoItemViewModel = new UpdateTodoItemViewModel
            {
                Name = "Test Todo",
                Description = "This is a test todo",
                DueDate = new DateTime(2022, 1, 1),
                Status = 0
            };

            // Act
            var result = CreateUpdateViewModelMapper.ToTodoEntity(id, updateTodoItemViewModel);

            // Assert
            Assert.That(result, Is.InstanceOf<TodoEntity>());
            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(id));
                Assert.That(result.Name, Is.EqualTo(updateTodoItemViewModel.Name));
                Assert.That(result.Description, Is.EqualTo(updateTodoItemViewModel.Description));
                Assert.That(result.DueDate, Is.EqualTo(updateTodoItemViewModel.DueDate));
                Assert.That(result.Status, Is.EqualTo(updateTodoItemViewModel.Status));
            });
        }
    }
}
