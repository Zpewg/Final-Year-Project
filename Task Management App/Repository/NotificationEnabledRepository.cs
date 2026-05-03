using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class NotificationEnabledRepository
{
    private readonly MyDBContext _context;
    
    public  NotificationEnabledRepository(MyDBContext context)
    {
        _context = context;
    }
    
    public NotificationEnabledRepository(){}
    
    //"CREATE" operation, it basically enables that a user can receive notifications
    public async Task EnableNotifications(NotificationEnabled notificationEnabled)
    {
        // 1. Verificăm dacă există deja o intrare pentru acest utilizator
        var existingConfig = await _context.NotificationEnabled
            .FirstOrDefaultAsync(x => x.UserId == notificationEnabled.UserId);

        if (existingConfig != null)
        {
            // 2. Dacă există, doar actualizăm flag-ul
            existingConfig.IsEnabled = true;
            _context.Entry(existingConfig).State = EntityState.Modified;
        }
        else
        {
            notificationEnabled.NotificationId = 0; 
            await _context.NotificationEnabled.AddAsync(notificationEnabled);
        }

        await _context.SaveChangesAsync();
    }
    
    
    public async Task DisableNotifications(NotificationEnabled notificationEnabled)
    {
        _context.Entry(notificationEnabled).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<NotificationEnabled>> GetNotificationEnabledByUserId(int userId)
    {
        return await _context.NotificationEnabled.Where(n => n.UserId == userId).ToListAsync();
    }
    
}