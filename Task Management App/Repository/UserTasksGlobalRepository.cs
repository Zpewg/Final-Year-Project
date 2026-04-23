using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class UserTasksGlobalRepository
{
    private readonly MyDBContext _context;
    
    public UserTasksGlobalRepository(MyDBContext context)
    {
        _context = context;
    }

    public async Task<List<UserTasksGlobal>> GetUserTasksByKm(Point userLocation, int km)
    {
        double distanceInMeters = km * 1000.0;

        return await _context.UserTasksGlobal
            // Filters tasks that have a location and are within the specified radius
            .Where(t => t.Location != null && t.Location.Distance(userLocation) <= distanceInMeters)
            .ToListAsync();
    }

    public async Task AddUserTask(UserTasksGlobal userTasksGlobal)
    {
        await _context.UserTasksGlobal.AddAsync(userTasksGlobal);
        await _context.SaveChangesAsync();
    }
}