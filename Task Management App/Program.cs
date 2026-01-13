using Microsoft.EntityFrameworkCore;
using Task_Management_App.Controllers;
using Task_Management_App.DB;
using Task_Management_App.Repository;
using Task_Management_App.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<VerifyMessageRepository>();
builder.Services.AddScoped<VerifyMessageService>();
builder.Services.AddScoped<CodeFromUserService>();
builder.Services.AddScoped<MessageCleanupService>();
builder.Services.AddScoped<MailingService>();



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin => 
                origin.StartsWith("http://localhost")
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();



    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
