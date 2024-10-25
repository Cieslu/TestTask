using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TestTask.AutoMapper;
using TestTask.Data;
using TestTask.Interfaces.Services;
using TestTask.Models;
using TestTask.ModelsDTO;
using TestTask.Services;
using Xunit;

namespace TestTask.xUnit
{
    public class Tests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options;
            
            var dbContext = new ApplicationDbContext(options);
            await dbContext.Database.OpenConnectionAsync();
            await dbContext.Database.EnsureCreatedAsync();

            if (await dbContext.Tasks.CountAsync() <= 0)
            {
                for(int i = 1; i < 7; i++)
                {
                    dbContext.Tasks.Add(
                        new ToDo()
                        {
                            DateAndTimeOfExpiry = DateTime.Now.AddDays(i),
                            Title = $"Test{i}",
                            Description = $"Test{i}"
                        }           
                    );
                    await dbContext.SaveChangesAsync();
                }
            }
            return dbContext;
        }

        private IMapper GetMapper() 
        {
            var config = new MapperConfiguration(x => {
                x.AddProfile<AutoMapperProfile>();
            });

            return config.CreateMapper();
        }

        [Fact]
        public async Task CreateTaskTest()//This test checks that the result is true;
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            NewToDoDTO newTaskDTO = new()
            {
                DateAndTimeOfExpiry = DateTime.Now.AddHours(2),
                Title = "Test7",
                Description = "Test7"
            };

            bool result = await taskService.CreateTask(newTaskDTO);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTaskTest()//This test checks that the result is true;
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            Guid taskId = await dbContext.Tasks
                .Where(t => t.Description.Contains("Test4"))
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            bool result = await taskService.DeleteTask(taskId);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllTasksTest()//This test checks that the tasksDTO is not null and that it is of type ToDoDTO.
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            List<ToDoDTO> tasksDTO = await taskService.GetAllTasks();

            Assert.NotNull(tasksDTO);
            Assert.IsType<List<ToDoDTO>>(tasksDTO);
        }

        [Fact]
        public async Task GetIncomingTasksTest()//This test checks that the tasksDTO is not null or that it is null.
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            List<int> options = new List<int> { 1, 2, 3 };

            options.ForEach(async o =>
            {
                List<ToDoDTO>? tasksDTO = await taskService.GetIncomingTasks(o);

                if (o is 1)
                {
                    Assert.NotNull(tasksDTO);
                }
                else if (o is 2)
                {
                    Assert.NotNull(tasksDTO);
                }
                else if (o is 3)
                {
                    Assert.NotNull(tasksDTO);
                }
                else
                {
                    Assert.Null(tasksDTO);
                }
            });
        }

        [Fact]
        public async Task GetSpecificTaskTest()//This test checks that the taskDTO is not null, that it is of type ToDoDTO and that the title has the value "Test2". 
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            Guid taskId = await dbContext.Tasks
                .Where(t => t.Title.Contains("Test2"))
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            ToDoDTO? taskDTO = await taskService.GetSpecificTask(taskId);

            Assert.NotNull(taskDTO);
            Assert.IsType<ToDoDTO>(taskDTO);
            Assert.Equal("Test2", taskDTO.Title);
        }

        [Fact]
        public async Task MarkTaskAsDoneTest()//This test checks that the result is true;
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            Guid taskId = await dbContext.Tasks
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            bool result = await taskService.MarkTaskAsDone(taskId);

            Assert.True(result);
        }

        [Fact]
        public async Task SetTaskPercentCompleteTest()//This test checks that the result is true;
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            Guid taskId = await dbContext.Tasks
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            bool result = await taskService.SetTaskPercentComplete(taskId, 35);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateTaskTest()//This test checks that the result is true;
        {
            var dbContext = await GetDbContext();
            var mapper = GetMapper();
            var taskService = new TaskService(dbContext, mapper);

            Guid taskId = await dbContext.Tasks
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            UpdateToDoDTO updatedTaskDTO = new()
            {
                Title = "TestEdycja",
                Description = "TestEdycja"
            };

            bool result = await taskService.UpdateTask(updatedTaskDTO, taskId);

            Assert.True(result);
        }
    }
}
