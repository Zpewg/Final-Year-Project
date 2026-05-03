using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class TaskSuggestionRepository
{
    private readonly MyDBContext _context;
    
    public  TaskSuggestionRepository(MyDBContext context)
    {
        _context = context;
    }
    
    public TaskSuggestionRepository(){}
    
    //"CREATE" operation, it basically enables that a user can receive notifications
    public async Task EnableNotifications(TaskSuggestion taskSuggestion)
    {
        // 1. Verificăm dacă există deja o intrare pentru acest utilizator
        var existingConfig = await _context.TaskSuggestion
            .FirstOrDefaultAsync(x => x.UserId == taskSuggestion.UserId);

        if (existingConfig != null)
        {
            // 2. Dacă există, doar actualizăm flag-ul
            existingConfig.IsEnabled = true;
            _context.Entry(existingConfig).State = EntityState.Modified;
        }
        else
        {
            taskSuggestion.TaskSuggestionId = 0; 
            await _context.TaskSuggestion.AddAsync(taskSuggestion);
        }

        await _context.SaveChangesAsync();
    }
    
    
    public async Task DisableNotifications(TaskSuggestion taskSuggestion)
    {
        _context.Entry(taskSuggestion).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<TaskSuggestion>> GetNotificationEnabledByUserId(int userId)
    {
        return await _context.TaskSuggestion.Where(n => n.UserId == userId).ToListAsync();
    }
}