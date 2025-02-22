using System;
using TaskManager.Services;
using Moq;
using TaskManager.Repository;
using TaskManager.Models;
using Microsoft.AspNetCore.Components;

namespace TaskManagerTest
{
    public class TaskItemServiceTest
    {
        private readonly Mock<ITaskItemRepository> _taskItemRepository = new Mock<ITaskItemRepository>();
        private readonly ITaskItemService _taskItemService;
        public TaskItemServiceTest()
        {
            _taskItemRepository = new Mock<ITaskItemRepository>();
            _taskItemService = new TaskItemService(_taskItemRepository.Object);
        }

        [Fact]
        public async Task GetTaskItemById_Valid_ReturnsTaskItemAsync()
        {
            
            var taskItem = new TaskItem
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now
            };

            _taskItemRepository.Setup(x => x.GetTaskItemByIdAsync(1)).ReturnsAsync(taskItem);

            var result = await _taskItemService.GetTaskItemByIdAsync(1);
            
            Assert.NotNull(result);
            Assert.Equal(taskItem.Id, result.Id);
            Assert.Equal(taskItem.Title, result.Title);
            Assert.Equal(taskItem.Description, result.Description);
            Assert.Equal(taskItem.DueDate, result.DueDate);
        }

        [Fact]
        public async Task GetAllTaskItems_Valid_ReturnsTaskItemsAsync()
        {
            var taskItems = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Title = "Task 1",
                    Description = "Description 1",
                    DueDate = DateTime.Now
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Task 2",
                    Description = "Description 2",
                    DueDate = DateTime.Now
                }
            };

            _taskItemRepository.Setup(x => x.GetAllTaskItemsAsync()).ReturnsAsync(taskItems);

            var result = await _taskItemService.GetAllTaskItemsAsync();

            Assert.NotNull(result);
            Assert.Equal(taskItems.Count, result.Count());

            Assert.Collection(result, item =>
            {
                Assert.Equal(taskItems[0].Id, item.Id);
                Assert.Equal(taskItems[0].Title, item.Title);
                Assert.Equal(taskItems[0].Description, item.Description);
                Assert.Equal(taskItems[0].DueDate, item.DueDate);
            },
            item =>
            {
                Assert.Equal(taskItems[1].Id, item.Id);
                Assert.Equal(taskItems[1].Title, item.Title);
                Assert.Equal(taskItems[1].Description, item.Description);
                Assert.Equal(taskItems[1].DueDate, item.DueDate);
            });
        }

        [Fact]
        public async Task AddTaskItem_Valid_ReturnsTaskItemAsync()
        {
            var taskItem = new TaskItem
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now
            };

            _taskItemRepository.Setup(x => x.AddTaskItemAsync(taskItem)).ReturnsAsync(taskItem);

            var result = await _taskItemService.AddTaskItemAsync(taskItem);

            Assert.NotNull(result);
            Assert.Equal(taskItem.Id, result.Id);
            Assert.Equal(taskItem.Title, result.Title);
            Assert.Equal(taskItem.Description, result.Description);
            Assert.Equal(taskItem.DueDate, result.DueDate);
        }

        [Fact]
        public async Task UpdateTaskItem_Valid_ReturnsTaskItemAsync()
        {
            var taskItem = new TaskItem
            {
                Id = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now
            };

            _taskItemRepository.Setup(x => x.UpdateTaskItemAsync(taskItem)).ReturnsAsync(taskItem);

            var result = await _taskItemService.UpdateTaskItemAsync(taskItem);

            Assert.NotNull(result);
            Assert.Equal(taskItem.Id, result.Id);
            Assert.Equal(taskItem.Title, result.Title);
            Assert.Equal(taskItem.Description, result.Description);
            Assert.Equal(taskItem.DueDate, result.DueDate);
        }

        [Fact]
        public async Task DeleteTaskItem_Valid_ReturnsTrueAsync()
        {
            _taskItemRepository.Setup(x => x.DeleteTaskItemAsync(1)).Returns(Task.CompletedTask);

            var result = await _taskItemService.DeleteTaskItemAsync(1);

            Assert.True(result);

            _taskItemRepository.Verify(x => x.DeleteTaskItemAsync(1), Times.Once);
        }
    }
}
