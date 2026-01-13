using Task_Management_App.Entities;
using Task_Management_App.Repository;
using Task_Management_App.Validators;

namespace Task_Management_App.Service;

public class UserTasksService
{
    private readonly UserTasksRepository _userTasks;
    private readonly UserTasksValidator _userTasksValidation;
    
    public UserTasksService(UserTasksRepository userTasks, UserTasksValidator userTasksValidation)
    {
        _userTasks = userTasks;
        _userTasksValidation = userTasksValidation;
    }

    public async Task<List<string>> CreateUserTasks(UserTasks userTasks)
    {
        List<string> errors = await _userTasksValidation.ValidateUserTasks(userTasks);
        if (!errors.Any())
        {
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
            await _userTasks.UpdateUserTask(userTasks);
            return error;
        }
        return error;
    }

    public async Task DeleteUserTask(int id)
    {
        UserTasks userTasks = await _userTasks.GetUserTask(id);
        await _userTasks.DeleteUserTask(userTasks);
    }

    public async Task<List<UserTasks>> GetUserTasksByUserId(int userId)
    {
        return await _userTasks.GetUserTasksByUserId(userId);
    }
}