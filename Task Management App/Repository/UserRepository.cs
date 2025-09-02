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

}