using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManagerTest
{
    public class TaskItemRepositoryTest :IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskItemRepository _repository;
        public TaskItemRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new TaskItemRepository(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        [Fact]
        public async Task AddTaskAsync_Valid_ReturnsAddedTaskAndPersistsInDb()
        {

            var taskItem = new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now
            };
            
            var addedTask = await _repository.AddTaskItemAsync(taskItem);

            Assert.NotNull(addedTask);
            Assert.Equal("Test Task", addedTask.Title);
            Assert.Equal("Test Description", addedTask.Description);
            Assert.True(addedTask.Id > 0);

            var taskFromDb = await _context.TaskItems.FindAsync(addedTask.Id);

            Assert.NotNull(taskFromDb);
            Assert.Equal(addedTask.Id, taskFromDb.Id);
            Assert.Equal(addedTask.Title, taskFromDb.Title);
            Assert.Equal(addedTask.Description, taskFromDb.Description);
        }

        [Fact]
        public async Task GetTaskByIdAsync_Valid_ShouldReturnTask()
        {
            var taskItem = new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                IsComplete = false,
                DueDate = DateTime.Now
            };

            var addedTask = await _repository.AddTaskItemAsync(taskItem);

            var retrievedTask = await _repository.GetTaskItemByIdAsync(addedTask.Id);

            Assert.NotNull(retrievedTask);
            Assert.Equal(addedTask.Id, retrievedTask.Id);
            Assert.Equal(addedTask.Title, retrievedTask.Title);
            Assert.Equal(addedTask.Description, retrievedTask.Description);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            var taskItem1 = new TaskItem { Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now };
            var taskItem2 = new TaskItem { Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now };
            await _repository.AddTaskItemAsync(taskItem1);
            await _repository.AddTaskItemAsync(taskItem2);

            var tasks = await _repository.GetAllTaskItemsAsync();

            Assert.NotNull(tasks);
            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTask()
        {
            var taskItem = new TaskItem { Title = "Old Title", Description = "Old Description", DueDate = DateTime.Now };
            var addedTaskItem = await _repository.AddTaskItemAsync(taskItem);

            addedTaskItem.Title = "New Title";
            addedTaskItem.Description = "New Description";
            var updatedTask = await _repository.UpdateTaskItemAsync(addedTaskItem);

            Assert.NotNull(updatedTask);
            Assert.Equal("New Title", updatedTask.Title);
            Assert.Equal("New Description", updatedTask.Description);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldRemoveTask()
        {
            var taskItem = new TaskItem { Title = "Task to Delete", Description = "Delete Description", DueDate = DateTime.Now };
            var addedTask = await _repository.AddTaskItemAsync(taskItem);

            await _repository.DeleteTaskItemAsync(addedTask.Id);
            var deletedTask = await _context.TaskItems.FindAsync(addedTask.Id);

            Assert.Null(deletedTask);
        }
    }
}
