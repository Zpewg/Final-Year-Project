using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Service;

public class NotificationEnabledService
{
    private readonly NotificationEnabledRepository _notificationEnabledRepository;
    private readonly UserRepository _userRepository;

    public NotificationEnabledService(NotificationEnabledRepository notificationEnabledRepository, UserRepository userRepository)
    {
        _notificationEnabledRepository = notificationEnabledRepository;
        _userRepository = userRepository;
    }

    public async Task<string> EnableNotifications(NotificationEnabled notificationEnabled)
    {
        var usr = await _userRepository.FindUserById(notificationEnabled.UserId);

        if (usr == null)
        {
            return null;
        }
        
        await _notificationEnabledRepository.EnableNotifications(notificationEnabled);

        return "Notification Enabled";
    }

    public async Task<string> DisableNotifications(NotificationEnabled notificationEnabled)
    {
        var usr = await _userRepository.FindUserById(notificationEnabled.UserId);

        if (usr == null)
        {
            return null;
        }
        
        await _notificationEnabledRepository.DisableNotifications(notificationEnabled);
        return "Notification Disabled";
    }

    public async Task<List<NotificationEnabled>> GetNotificationEnabledByUserId(int userId)
    {
        var usr = await _userRepository.FindUserById(userId);
        if (usr == null)
        {
            return null;
        }
        List<NotificationEnabled> list = await  _notificationEnabledRepository.GetNotificationEnabledByUserId(userId);
        return list;
    }
    
    
}