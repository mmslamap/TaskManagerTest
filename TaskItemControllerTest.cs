using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Moq;
using TaskManager.Controllers;
using TaskManager.Services;
using TaskManager.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerTest
{
    public class TaskItemControllerTest
    {
        private readonly Mock<ITaskItemService> _taskItemService;
        private readonly TaskItemController _taskItemController;

        public TaskItemControllerTest()
        {
            _taskItemService = new Mock<ITaskItemService>();
            _taskItemController = new TaskItemController(_taskItemService.Object);
        }

        [Fact]
        public async Task GetAllTaskItemsAsync_ReturnsOk_WithTaskItems()
        { 
            var taskItems = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now },
                new TaskItem { Id = 2, Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now }
            };

            _taskItemService.Setup(x => x.GetAllTaskItemsAsync()).ReturnsAsync(taskItems);

            var result = await _taskItemController.GetAllTaskItemsAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTaskItems = Assert.IsType<List<TaskItem>>(okResult.Value);

            Assert.Collection(returnedTaskItems,
                item => Assert.Equal(taskItems[0].Id, item.Id),
                item => Assert.Equal(taskItems[1].Id, item.Id));
        }

        [Fact]
        public async Task GetTaskItemByIdAsync_ReturnsOk_WithTaskItem()
        {
            var taskItem = new TaskItem 
            { 
                Id = 1, 
                Title = "Task 1", 
                Description = "Description 1", 
                DueDate = DateTime.Now 
            };

            _taskItemService.Setup(x => x.GetTaskItemByIdAsync(1)).ReturnsAsync(taskItem);

            var result = await _taskItemController.GetTaskItemByIdAsync(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTaskItem = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal(taskItem.Id, returnedTaskItem.Id);
        }

        [Fact]
        public async Task GetTaskItemByIdAsync_ReturnsNotFound_WhenTaskItemNotFound()
        {
            _taskItemService.Setup(x => x.GetTaskItemByIdAsync(1)).ThrowsAsync(new InvalidOperationException("Task not found"));

            var result = await _taskItemController.GetTaskItemByIdAsync(1);

            var notFoundResult =  Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Task not found", notFoundResult.Value);
        }

        [Fact]
        public async Task AddTaskItemAsync_ReturnsOk_WithTaskItem()
        {
            var taskItem = new TaskItem
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now
            };

            _taskItemService.Setup(x => x.AddTaskItemAsync(taskItem)).ReturnsAsync(taskItem);

            var result = await _taskItemController.AddTaskItemAsync(taskItem);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTaskItem = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal(taskItem.Id, returnedTaskItem.Id);
        }

        [Fact]
        public async Task UpdateTaskItemAsync_ReturnsOk_WithTaskItem()
        {
            var taskItem = new TaskItem
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now
            };

            _taskItemService.Setup(x => x.UpdateTaskItemAsync(taskItem)).ReturnsAsync(taskItem);

            var result = await _taskItemController.UpdateTaskItemAsync(taskItem);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTaskItem = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal(taskItem.Id, returnedTaskItem.Id);
            Assert.Equal(taskItem.Title, returnedTaskItem.Title);
            Assert.Equal(taskItem.Description, returnedTaskItem.Description);
            Assert.Equal(taskItem.DueDate, returnedTaskItem.DueDate);
        }

        [Fact]
        public async Task UpdateTaskItemAsync_ReturnsNotFound_WhenTaskItemNotFound()
        {
            var taskItem = new TaskItem
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now
            };

            _taskItemService.Setup(x => x.UpdateTaskItemAsync(taskItem)).ThrowsAsync(new InvalidOperationException("Task not found"));

            var result = await _taskItemController.UpdateTaskItemAsync(taskItem);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Task not found", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteTaskItemAsync_ReturnsOk_WhenTaskItemDeleted()
        {
            _taskItemService.Setup(x => x.DeleteTaskItemAsync(1)).ReturnsAsync(true);

            var result = await _taskItemController.DeleteTaskItemAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTaskItemAsync_ReturnsNotFound_WhenTaskItemNotFound()
        {
            _taskItemService.Setup(x => x.DeleteTaskItemAsync(1)).ReturnsAsync(false);

            var result = await _taskItemController.DeleteTaskItemAsync(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
