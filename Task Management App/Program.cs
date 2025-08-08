using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with connection string from env variable or appsettings
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();



app.UseHttpsRedirection();

// TODO: Define your Task endpoints here
// Example:
// app.MapGet("/tasks", (MyDBContext db) => db.Tasks.ToList());

app.Run();