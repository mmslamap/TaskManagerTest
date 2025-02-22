namespace TaskManager.Tests
{
    using Microsoft.EntityFrameworkCore;
    using TaskManager.Data;
    using Xunit;
    using TaskManager.Models;
    public class ApplicationDbContextTest : IDisposable
    {
        private readonly ApplicationDbContext _context;
        public ApplicationDbContextTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
        }

        public void Dispose() 
        { 
            _context?.Dispose();
        }

        [Fact]
        public void CanInsertTask()
        {
            var taskItem = new TaskItem
            {
                Title = "Sample Task",
                Description = "Sample Description",
                IsComplete = false,
                DueDate = DateTime.Now
            };

            _context.Tasks.Add(taskItem);
            _context.SaveChanges();

            Assert.True(taskItem.Id > 0);
        }

        [Fact]
        public void CanRetrieveTask() {

            var taskItem = new TaskItem
            {
                Title = "Sample Task",
                Description = "Sample Description",
                IsComplete = false,
                DueDate = DateTime.Now
            };

            _context.Tasks.Add(taskItem);
            _context.SaveChanges();

            var taskFromDb = _context.Tasks.Find(taskItem.Id);

            Assert.NotNull(taskFromDb);
            Assert.Equal(taskItem.Title, taskFromDb.Title);
            Assert.Equal(taskItem.Description, taskFromDb.Description);
            Assert.Equal(taskItem.IsComplete, taskFromDb.IsComplete);
            Assert.Equal(taskItem.DueDate, taskFromDb.DueDate);
        }
    }
}
