using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;

namespace Task_Management_App.Repository;


using Task_Management_App.Entities;


public class UserRepository
{
    
    private readonly MyDBContext _context;

    public UserRepository(MyDBContext context)
    {
        _context = context;
    }

    public UserRepository(){}
    
    //method to add user
    public async Task AddUser(User user)
    {
        user.ToString();
        Console.WriteLine("Am ajuns in repo");
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    //method to retrieve all users
    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<int> GetUserIdByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email).Select(u => (int) u.UserId).FirstOrDefaultAsync();
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email).Select(u => (User) u).FirstOrDefaultAsync();
    }
    public async Task UpdateUserStatus(int userId, bool status)
    {
        var user = await _context.Users.Where(u=> u.UserId == userId).FirstOrDefaultAsync();
        
        user.Active = status;
        _context.Entry(user).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserPassword(int userId, string  password)
    {
        var user = await _context.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        
        user.Password = password;
        _context.Entry(user).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
    }
}