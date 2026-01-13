using System.Text.RegularExpressions;
using Task_Management_App.Repository;

namespace Task_Management_App.Validators;
using Task_Management_App.Entities;
using Task_Management_App.Service;


public class UserTasksValidator
{
    
    private readonly UserTasksRepository _userTasks;

    public UserTasksValidator(UserTasksRepository userTasks)
    {
        _userTasks = userTasks;
    }
    public async Task<List<string>> ValidateUserTasks(UserTasks userTasks)
    {
        List<string> errors = new List<string>();

        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
        
        
        //Any character max 40ch
        Regex regexTaskName = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+{}\[\]:;<>,.?\/~`=\s-]{1,40}$");
        
        if (!regexTaskName.IsMatch(userTasks.NameOfTask))
        {
            errors.Add("Invalid Task Name");
        }

        if (userTasks.Date < currentDate)
        {
            errors.Add("Invalid Date Range");
        }

        if (userTasks.Date <= currentDate && userTasks.Time < currentTime)
        {
            errors.Add("You cannot add a time in the past");
        }



        return errors;
    }
}