using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestTask.Interfaces.Controller;
using TestTask.Interfaces.Services;
using TestTask.ModelsDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase, ITaskController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            this._taskService = taskService;
        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult> CreateTask([FromForm]NewToDoDTO newTask)
        {
            try
            {
                bool result = await _taskService.CreateTask(newTask);

                return result ? Ok("Successfully added the new task.") : BadRequest("Invalid data provided.");   
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("DeleteTask/{taskId}")]
        public async Task<ActionResult> DeleteTask(Guid taskId)
        {
            try
            {
                bool result = await _taskService.DeleteTask(taskId);

                return result ? Ok($"Successfully deleted the task with Id: {taskId}.") : BadRequest("Not deleted successfully.");
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("GetAllTasks")]

        public async Task<ActionResult<List<ToDoDTO>>> GetAllTasks()
        {
            try
            {
                List<ToDoDTO> tasks = await _taskService.GetAllTasks();
                return Ok(tasks.Count() > 0 ? tasks : "The list of tasks is empty.");
            }
            catch
            {
                return StatusCode(500, "An unecpected error occurred.");
            }
        }

        /// <summary>
        /// Type in:
        ///  - 1 - If you want to see tasks from this day. 
        ///  - 2 - If you want to see tasks from the next day. 
        ///  - 3 - If you want to see tasks from this week. 
        /// </summary>
        [HttpGet("GetIncomingTasks/{option}")]
        public async Task<ActionResult<List<ToDoDTO>>> GetIncomingTasks(int option)
        {
            try
            {
                List<ToDoDTO>? tasks = await _taskService.GetIncomingTasks(option);

                if (tasks is null)
                {
                    return BadRequest("The character entered is incorrect.");
                }
                else
                {
                    return Ok(tasks.Count() > 0 ? tasks : "The list of tasks is empty.");
                }
            }
            catch
            {
                return StatusCode(500, "An unecpected error occurred.");
            }
        }

        [HttpGet("GetSpecificTask/{taskId}")]
        public async Task<ActionResult<ToDoDTO?>> GetSpecificTask(Guid taskId)
        {
            try
            {
                ToDoDTO? task = await _taskService.GetSpecificTask(taskId);
                return task is not null ? Ok(task) : NotFound($"The task with the Id: {taskId} not found.");
            }
            catch
            {
                return StatusCode(500, "An unecpected error occurred.");
            }
        }

        [HttpPut("MarkTaskAsDone/{taskId}")]
        public async Task<ActionResult> MarkTaskAsDone(Guid taskId)
        {
            try
            {
                bool result = await _taskService.MarkTaskAsDone(taskId);
                return result ? Ok("Successfully marked the task as done.") : NotFound($"The task with the Id: {taskId} not found.");
            }
            catch
            {
                return StatusCode(500, "An unecpected error occurred.");
            }
        }

        [HttpPut("SetTaskPercentComplete/{taskId}/{percent}")]
        public async Task<ActionResult> SetTaskPercentComplete(Guid taskId, int percent)
        {
            try
            {
                bool result = await _taskService.SetTaskPercentComplete(taskId, percent);
                return result ? Ok("Successfully set the percent for the task.") : NotFound($"The task with the Id: {taskId} not found or the percent is greater than 100.");
            }
            catch
            {
                return StatusCode(500, "An unecpected error occurred.");
            }
        }

        [HttpPut("UpdateTask/{taskId}")]
        public async Task<ActionResult> UpdateTask([FromForm]UpdateToDoDTO updatedTask, Guid taskId)
        {
            try
            {
                bool result = await _taskService.UpdateTask(updatedTask, taskId);
                return result ? Ok("Successfully updated the task.") : BadRequest("Invalid data provided.");
            }
            catch
            {
                return StatusCode(500, "An unecpected error occurred.");
            }
        }
    }
}
