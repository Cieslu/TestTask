using Microsoft.AspNetCore.Mvc;
using TestTask.ModelsDTO;

namespace TestTask.Interfaces.Services
{
    public interface ITaskService
    {
        public Task<List<ToDoDTO>> GetAllTasks();
        public Task<ToDoDTO?> GetSpecificTask(Guid taskId);
        public Task<List<ToDoDTO>?> GetIncomingTasks(int option);
        public Task<bool> CreateTask(NewToDoDTO newTask);
        public Task<bool> UpdateTask(UpdateToDoDTO updatedTask, Guid taskId);
        public Task<bool> SetTaskPercentComplete(Guid taskId, int percent);
        public Task<bool> DeleteTask(Guid taskId);
        public Task<bool> MarkTaskAsDone(Guid taskId);
    }
}
