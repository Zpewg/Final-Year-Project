using System.Text.RegularExpressions;
using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Validators;

public class UserTasksGlobalValidator
{
    private readonly UserTasksGlobalRepository _userTasksGlobalRepository;
    
    public UserTasksGlobalValidator(UserTasksGlobalRepository userTasksGlobalRepository)
    {
        _userTasksGlobalRepository = userTasksGlobalRepository;
    }

    public async Task<List<string>> ValidateUserTasksGlobal(UserTasksGlobal userTasksGlobal)
    {
        List<string> errors = new List<string>();
        
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
        
        Regex regexTaskName = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+{}\[\]:;<>,.?\/~`=\s-]{1,40}$");
        
        if (!regexTaskName.IsMatch(userTasksGlobal.NameOfTask))
        {
            errors.Add("Invalid Task Name");
        }

        if (userTasksGlobal.Date < currentDate)
        {
            errors.Add("Invalid Date Range");
        }

        if (userTasksGlobal.Date <= currentDate && userTasksGlobal.Time < currentTime)
        {
            errors.Add("You cannot add a time in the past");
        }

        if (userTasksGlobal.Location == null)
        {
            errors.Add("Location is required");
        }
        
        
        return errors;
    }
}