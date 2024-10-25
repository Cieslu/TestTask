using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TestTask.Data;
using TestTask.Interfaces.Services;
using TestTask.Models;
using TestTask.ModelsDTO;
using TestTask.Validators;

namespace TestTask.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(ApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        //The method for creating a task.
        public async Task<bool> CreateTask(NewToDoDTO newTaskDTO)//This is an asynchronous method. I used it here, because the mainly thread is not blocked.
        {
            try//This is used for catching exceptions and throwing them.
            {
                NewToDoDTOValidator validator = new NewToDoDTOValidator();
                ValidationResult result = validator.Validate(newTaskDTO);//This is used to validate data from a form.

                if (result.IsValid)
                {
                    ToDo task = _mapper.Map<ToDo>(newTaskDTO);//Mapping ToDoDTO object to ToDo object with AutoMapper. 

                    await _context.Tasks.AddAsync(task);//Adding the mapped object to the ToDos table.
                    await _context.SaveChangesAsync();//Saving changes.

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in CreateToDo method of ToDoService! Message: {e}");
            }
        }

        //The method for deleting a task.
        public async Task<bool> DeleteTask(Guid taskId)
        {
            try
            {
                int result = await _context.Tasks//If this object is in the database, the method deletes it. The ExecuteDeleteAsync method does not retrieve the objects to a memory, but it executes operations directly in the database. 
                    .Where(t => t.Id == taskId)
                    .ExecuteDeleteAsync();

                return result > 0 ? true : false;//If the result variable is greater than 0, the ExecuteDeleteAsync method will delete the row and the DeleteToDo method will return true, otherwise it will return false.
            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in DeleteToDo method of ToDoService! Message: {e}");
            }
        }

        //The method for retrieving all tasks.
        public async Task<List<ToDoDTO>> GetAllTasks()
        {
            try
            {
                List<ToDo> tasks = await _context.Tasks//This method uses "AsNoTracking()", because it does not require tracking. 
                    .AsNoTracking()
                    .ToListAsync();

                List<ToDoDTO> tasksDTO = _mapper.Map<List<ToDoDTO>>(tasks);//AutoMapper automatically converts data to a different type.

                return tasksDTO;
            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in GetAllToDo method of ToDoService! Message: {e}");
            }
        }

        //The method for retrieving a incoming tasks.
        public async Task<List<ToDoDTO>?> GetIncomingTasks(int option)
        {
            try
            {
                List<ToDoDTO>? tasksDTO = option switch
                {
                    1 => await _context.Tasks//If the case is 1, display today's tasks.
                        .Where(t => t.DateAndTimeOfExpiry.Date == DateTime.Today.Date)
                        .Select(t => _mapper.Map<ToDoDTO>(t))
                        .ToListAsync(),
                    2 => await _context.Tasks//If the case is 2, display tomorrow's tasks.
                        .Where(t => t.DateAndTimeOfExpiry.Date == DateTime.Today.Date.AddDays(1))
                        .Select(t => _mapper.Map<ToDoDTO>(t)).ToListAsync(),
                    3 => await _context.Tasks//If the case is 3, display tasks for the current week.
                        .Select(t => _mapper.Map<ToDoDTO>(t))
                        .ToListAsync(),
                    _ => null//If the case is different from the above, the list will be null.
                };

                if (option is 3)//If the option is 3, the list is additionally filtered.
                {
                    tasksDTO = tasksDTO!.Where(t => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(t.DateAndTimeOfExpiry.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Today.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).ToList();
                }

                return tasksDTO;
            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in GetIncomingToDo method of ToDoService! Message: {e}");
            }
        }

        //The method for retrieving a specific task.
        public async Task<ToDoDTO?> GetSpecificTask(Guid taskId)
        {
            try
            {
                ToDo? task = await _context.Tasks.FindAsync(taskId);//FindAsync() is used in this case because this method offers very fast searching.
                ToDoDTO? taskDTO = _mapper.Map<ToDoDTO>(task);
                
                return taskDTO;
            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in GetSpecificToDo method of ToDoService! Message: {e}");
            }
        }

        //The method for marking a task as done.
        public async Task<bool> MarkTaskAsDone(Guid taskId)
        {
            try
            {
                int result = await _context.Tasks//The ExecuteUpdateAsync() is efficient, because it does not load the data into memory, instead it operates directly in the database.  
                    .Where(t => t.Id == taskId)
                    .ExecuteUpdateAsync(st => st
                        .SetProperty(t => t.Complete, 100));

                return result > 0 ? true : false;//If the result is different from 0, return true, otherwise, return false.
            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in MarkToDoAsDone method of ToDoService! Message: {e}");
            }
        }

        //The method for updating a task.
        public async Task<bool> UpdateTask(UpdateToDoDTO updatedTask, Guid taskId)
        {
            try
            {
                UpdateToDoDTOValidator validator = new UpdateToDoDTOValidator();
                ValidationResult result = validator.Validate(updatedTask);

                if (result.IsValid)
                { 
                    ToDo? task = await _context.Tasks.FindAsync(taskId);

                    if (task is null)
                    {
                        return false;
                    }
                    else
                    {
                        task.Title = updatedTask.Title ?? task.Title;//The title is equal to task.Title, if updatedTask.Title is null.  
                        task.Description = updatedTask.Description ?? task.Description;
                        task.DateAndTimeOfExpiry = updatedTask.DateAndTimeOfExpiry ?? task.DateAndTimeOfExpiry;

                        await _context.SaveChangesAsync();

                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in UpdateToDo method of ToDoService! Message: {e}");
            }
        }

        //The method for setting percent complete of a task.
        public async Task<bool> SetTaskPercentComplete(Guid taskId, int percent)
        {
            try
            {
                if (percent > 100)//The perecent has to be a maximum of one hundred.
                {
                    return false;
                }
                else
                {
                    int result = await _context.Tasks
                        .Where(t => t.Id == taskId)
                        .ExecuteUpdateAsync(st => st
                            .SetProperty(t => t.Complete, percent));

                    return result > 0 ? true : false;
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Error occurred in SetToDoPercentComplete method of ToDoService! Message: {e}");
            }
        }
    }
}
