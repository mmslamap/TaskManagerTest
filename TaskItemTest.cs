using System.Diagnostics.Metrics;
using Xunit;
using TaskManager.Models;
using FluentAssertions;
using TaskItem = TaskManager.Models.TaskItem;

namespace TaskManager.Tests
{
    public class TaskItemTest
    {
        [Fact]
        public void NewTaskShouldHaveCurrentDate()
        {
            var before = DateTime.Now;
            TaskItem taskItem = new();
            var after = DateTime.Now;

            Assert.InRange(taskItem.CreatedAt, before, after);
        }

        [Fact]
        public void Task_ShouldHaveTitle()
        {
            var taskItem = new TaskItem { Title = "Sample Task" };

            taskItem.Title.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void Task_ShouldHaveDescription()
        {
            var taskItem = new TaskItem { Description = "Sample Description" };
            taskItem.Description.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void TaskWithDifferentTitle_ShouldNotBeEqual()
        {
            var taskItem1 = new TaskItem { Title = "Task 1" };
            var taskItem2 = new TaskItem { Title = "Task 2" };
            taskItem1.Should().NotBe(taskItem2);
        }
    }
}
