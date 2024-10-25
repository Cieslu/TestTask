using Microsoft.AspNetCore.Mvc;
using TestTask.ModelsDTO;

namespace TestTask.Interfaces.Controller
{
    public interface ITaskController
    {
        public Task<ActionResult<List<ToDoDTO>>> GetAllTasks();
        public Task<ActionResult<ToDoDTO?>> GetSpecificTask(Guid taskId);
        public Task<ActionResult<List<ToDoDTO>>> GetIncomingTasks(int option);
        public Task<ActionResult> CreateTask(NewToDoDTO newTask);
        public Task<ActionResult> UpdateTask(UpdateToDoDTO updatedTask, Guid taskId);
        public Task<ActionResult> SetTaskPercentComplete(Guid taskId, int percent);
        public Task<ActionResult> DeleteTask(Guid taskId);
        public Task<ActionResult> MarkTaskAsDone(Guid taskId);
    }
}
