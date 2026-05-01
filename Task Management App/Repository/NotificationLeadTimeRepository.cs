using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class NotificationLeadTimeRepository
{
    private readonly MyDBContext _context;
    public NotificationLeadTimeRepository(MyDBContext context)
    {
        _context = context;
    }
    
    public NotificationLeadTimeRepository(){}


    public async Task AddNotificationLeadTime(NotificationLeadTime notificationLeadTime)
    {
        await _context.NotificationLeadTime.AddAsync(notificationLeadTime);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateNotificationLeadTime(NotificationLeadTime notificationLeadTime)
    {
        _context.Entry(notificationLeadTime).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveNotificationLeadTime(NotificationLeadTime notificationLeadTime)
    {
        
        var entity = await _context.NotificationLeadTime.FindAsync(notificationLeadTime.NotificationLeadTimeId);

        if (entity != null)
        {
            _context.NotificationLeadTime.Remove(entity);
            await _context.SaveChangesAsync();
        }
      
    }
    
    public async Task<List<NotificationLeadTime>> GetAllNotificationAlertsByUserId(int userId)
    {
        return await _context.NotificationLeadTime
            .Join(
                _context.NotificationEnabled,
                leadTime => leadTime.NotificationId,    // PK din NotificationLeadTime
                enabled => enabled.NotificationId,      // FK din NotificationEnabled
                (leadTime, enabled) => new { leadTime, enabled } // Rezultatul combinat
            )
            .Where(joined => joined.enabled.UserId == userId) // Filtrarea după User
            .Select(joined => joined.leadTime)               // Returnăm doar timpii
            .ToListAsync();
    }
}