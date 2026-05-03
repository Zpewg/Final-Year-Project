using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Service;

public class TaskSuggestionService
{
    private readonly TaskSuggestionRepository _taskSuggestionRepository;
    private readonly UserRepository _userRepository;

    public TaskSuggestionService(TaskSuggestionRepository taskSuggestionRepository, UserRepository userRepository)
    {
        _taskSuggestionRepository = taskSuggestionRepository;
        _userRepository = userRepository;
    }

    public async Task<string> EnableNotifications(TaskSuggestion taskSuggestion)
    {
        var usr = await _userRepository.FindUserById(taskSuggestion.UserId);

        if (usr == null)
        {
            return null;
        }
        
        await _taskSuggestionRepository.EnableNotifications(taskSuggestion);

        return "Notification Enabled";
    }

    public async Task<string> DisableNotifications(TaskSuggestion taskSuggestion)
    {
        var usr = await _userRepository.FindUserById(taskSuggestion.UserId);

        if (usr == null)
        {
            return null;
        }
        
        await _taskSuggestionRepository.DisableNotifications(taskSuggestion);
        return "Notification Disabled";
    }

    public async Task<List<TaskSuggestion>> GetNotificationEnabledByUserId(int userId)
    {
        var usr = await _userRepository.FindUserById(userId);
        if (usr == null)
        {
            return null;
        }
        List<TaskSuggestion> list = await _taskSuggestionRepository.GetNotificationEnabledByUserId(userId);
        return list;
    }
}