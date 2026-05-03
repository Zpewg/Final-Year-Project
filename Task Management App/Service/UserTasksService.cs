using Task_Management_App.Entities;
using Task_Management_App.Helpers;
using Task_Management_App.Repository;
using Task_Management_App.Validators;

namespace Task_Management_App.Service;

public class UserTasksService
{
    private readonly UserTasksRepository _userTasks;
    private readonly UserTasksValidator _userTasksValidation;
    private readonly ScheduleOptimizerService _optimizer;
    
    public UserTasksService(UserTasksRepository userTasks, UserTasksValidator userTasksValidation, ScheduleOptimizerService optimizer)
    {
        _userTasks = userTasks;
        _userTasksValidation = userTasksValidation;
        _optimizer = optimizer;
    }
    
    public async Task<List<UserTasks>> GetOptimizationSuggestions(int userId)
    {
        
        var allTasks = await _userTasks.GetUserTasksByUserId(userId);
        
      
        var futureTasks = allTasks
            .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now))
            .ToList();

       
        return _optimizer.OptimizeWeek(futureTasks);
    }

    public async Task<List<string>> CreateUserTasks(UserTasks userTasks)
    {
        List<string> errors = await _userTasksValidation.ValidateUserTasks(userTasks);
        if (!errors.Any())
        {
          
            userTasks.taskWeight = CalculateTaskWeight(userTasks);
            
            await _userTasks.AddUserTask(userTasks);
            return errors;
        }
        return errors;
    }
    
    public async Task<List<string>> UpdateUserTask(UserTasks userTasks)
    {
        List<string> error = await _userTasksValidation.ValidateUserTasks(userTasks);
        if (!error.Any())
        {
           
            userTasks.taskWeight = CalculateTaskWeight(userTasks);
            
            await _userTasks.UpdateUserTask(userTasks);
            return error;
        }
        return error;
    }

    public async Task DeleteUserTask(UserTasks userTasks)
    {
        await _userTasks.DeleteUserTask(userTasks);
    }

    public async Task<List<UserTasks>> GetUserTasksByUserId(int userId)
    {
        return await _userTasks.GetUserTasksByUserId(userId);
    }

 
    private double CalculateTaskWeight(UserTasks task)
    {
        return TaskWeightCalculator.Calculate(task.taskDifficulty, task.taskUrgency, task.taskLength);
    }
}