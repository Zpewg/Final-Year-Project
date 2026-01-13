using Microsoft.EntityFrameworkCore;
using Task_Management_App.Entities;

namespace Task_Management_App.DB;

public class MyDBContext : DbContext
{
    public MyDBContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet <User> Users { get; set; }
    public DbSet <Journal>  Journals { get; set; }
    public DbSet<UserTasks> UserTasks { get; set; }
    public DbSet<VerifyMessage> VerifyMessages { get; set; }
}