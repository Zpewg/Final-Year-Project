using NetTopologySuite.Geometries;
using Task_Management_App.Entities;
using Task_Management_App.Repository;
using Task_Management_App.Validators;

namespace Task_Management_App.Service;

public class UserTasksGlobalService
{
    private readonly UserTasksGlobalRepository _userTasksGlobal;
    private readonly UserTasksGlobalValidator _userTasksGlobalValidation;
    
    public UserTasksGlobalService(UserTasksGlobalRepository userTasksGlobal, UserTasksGlobalValidator userTasksGlobalValidation)
    {
        _userTasksGlobal = userTasksGlobal;
        _userTasksGlobalValidation = userTasksGlobalValidation;
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

    public async Task<List<UserTasksGlobal>> ReadUserTasksGlobal(Point userLocation, int km)
    {
        return await _userTasksGlobal.GetUserTasksByKm(userLocation, km);
    }
}