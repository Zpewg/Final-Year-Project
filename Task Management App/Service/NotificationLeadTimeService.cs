using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Service;

public class NotificationLeadTimeService
{
    private readonly NotificationLeadTimeRepository _notificationEnabledRepository;

    public NotificationLeadTimeService(NotificationLeadTimeRepository notificationEnabledRepository)
    {
        _notificationEnabledRepository = notificationEnabledRepository;
      
    }

    public NotificationLeadTimeService(){}

    private string CheckTime(int time)
    {
        if (time < 1 || time > 10080)
        {
            return "Time interval has to be between 1 minute and 7 days";
        }

        return null;
    }

    public async Task<string> AddNotificationLeadTime(NotificationLeadTime notificationLeadTime)
    {
       string msg = CheckTime(notificationLeadTime.NotificationTime);
       if (msg == null)
       {
           await _notificationEnabledRepository.AddNotificationLeadTime(notificationLeadTime);

           return null;
       }
       return msg;
    }

    public async Task<string> UpdateNotificationLeadTime(NotificationLeadTime notificationLeadTime)
    {
        string msg = CheckTime(notificationLeadTime.NotificationTime);
        if (msg == null)
        {
            await _notificationEnabledRepository.UpdateNotificationLeadTime(notificationLeadTime);
            return null;
        }
        return msg;
    }

    public async Task<List<NotificationLeadTime>> GetNotificationLeadTime(int userId)
    {
        List <NotificationLeadTime> notifications = await _notificationEnabledRepository.GetAllNotificationAlertsByUserId(userId);
        return notifications;
    }

    public async Task<string> DeleteNotificationLeadTime(NotificationLeadTime notificationLeadTime)
    {
        await _notificationEnabledRepository.RemoveNotificationLeadTime(notificationLeadTime);
        return "removed successfully";
    }
    
}