using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class UserTasksRepository
{
    private readonly MyDBContext _context;

    public UserTasksRepository(MyDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<UserTasks>> GetUserTasksByUserId(int userId)
    {
        return await _context.UserTasks.Where(t=> t.UserId == userId).ToListAsync();
    }
    
    public async Task AddUserTask(UserTasks userTasks)
    {
        await _context.UserTasks.AddAsync(userTasks);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateUserTask(UserTasks userTasks)
    {
        _context.Entry(userTasks).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteUserTask(UserTasks userTasks)
    {
        var taskToDelete = await _context.UserTasks.FindAsync(userTasks.UserTaskId);
        if (taskToDelete != null)
        {   Console.WriteLine("user task deleted");
            _context.UserTasks.Remove(taskToDelete);
        }

        await _context.SaveChangesAsync();
    }
}