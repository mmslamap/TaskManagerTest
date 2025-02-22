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
            
            var addedTask = await _repository.AddTaskAsync(taskItem);

            Assert.NotNull(addedTask);
            Assert.Equal("Test Task", addedTask.Title);
            Assert.Equal("Test Description", addedTask.Description);
            Assert.True(addedTask.Id > 0);

            var taskFromDb = await _context.Tasks.FindAsync(addedTask.Id);

            Assert.NotNull(taskFromDb);
            Assert.Equal(addedTask.Id, taskFromDb.Id);
            Assert.Equal(addedTask.Title, taskFromDb.Title);
            Assert.Equal(addedTask.Description, taskFromDb.Description);
        }

        [Fact]
        public async Task GetTaskByIdAsync_Valid_ShouldReturnTask()
        {
            var task = new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                IsComplete = false,
                DueDate = DateTime.Now
            };

            var addedTask = await _repository.AddTaskAsync(task);

            var retrievedTask = await _repository.GetTaskByIdAsync(addedTask.Id);

            Assert.NotNull(retrievedTask);
            Assert.Equal(addedTask.Id, retrievedTask.Id);
            Assert.Equal(addedTask.Title, retrievedTask.Title);
            Assert.Equal(addedTask.Description, retrievedTask.Description);
        }
    }
}
