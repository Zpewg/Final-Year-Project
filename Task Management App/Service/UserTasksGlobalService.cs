using NetTopologySuite.Geometries;
using Task_Management_App.Entities;
using Task_Management_App.Repository;
using Task_Management_App.Validators;

namespace Task_Management_App.Service;

public class UserTasksGlobalService
{
    private readonly UserTasksGlobalRepository _userTasksGlobal;
    private readonly UserTasksGlobalValidator _userTasksGlobalValidation;
    private readonly UserRepository _userRepository;
    
    public UserTasksGlobalService(UserTasksGlobalRepository userTasksGlobal, UserTasksGlobalValidator userTasksGlobalValidation,  UserRepository userRepository)
    {
        _userTasksGlobal = userTasksGlobal;
        _userTasksGlobalValidation = userTasksGlobalValidation;
        _userRepository = userRepository;
    }

    public async Task<List<string>> CreateUserTasksGlobal(UserTasksGlobal userTasksGlobal)
    {
        List<string> errors = await _userTasksGlobalValidation.ValidateUserTasksGlobal(userTasksGlobal);
        if (!errors.Any())
        {
            await _userTasksGlobal.AddUserTask(userTasksGlobal);
        }
        return errors;
    }

    public async Task<List<UserTasksGlobal>> ReadUserTasksGlobal(User user)
    {
        user = await _userRepository.FindUserById(user.UserId);
        List<UserTasksGlobal> userList = new List<UserTasksGlobal>();
        userList = await _userTasksGlobal.GetUserTasksByKm(user.Location, user.Km);
        return userList;
    }

    public async Task<List<UserTasksGlobal>> ReadUserTasksGlobalById(User user)
    {
        user = await _userRepository.FindUserById(user.UserId);
        List<UserTasksGlobal> userListTasksGlobal = new List<UserTasksGlobal>();
        userListTasksGlobal = await _userTasksGlobal.GetUserTasksById(user.UserId);
        return userListTasksGlobal;
    }

    public async Task<string> DeleteUserTasksGlobal(UserTasksGlobal userTasksGlobal)
    {
        string message = await _userTasksGlobal.DeleteUserTask(userTasksGlobal);
        
        return message;
    }

    public async Task<List<string>> UpdateUserTasksGlobal(UserTasksGlobal userTasksGlobal)
    {
        List<string> message = new List<string>();
        message = await _userTasksGlobalValidation.ValidateUserTasksGlobal(userTasksGlobal);

        if (message.Any())
        {
            return message;
        }
        await  _userTasksGlobal.UpdateUserTask(userTasksGlobal);
        return message;
    }
}