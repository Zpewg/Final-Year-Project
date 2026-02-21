using Microsoft.EntityFrameworkCore;
using Task_Management_App.Controllers;
using Task_Management_App.DB;
using Task_Management_App.Repository;
using Task_Management_App.Service;
using Task_Management_App.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), 
        x => x.UseNetTopologySuite() 
    ));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<VerifyMessageRepository>();
builder.Services.AddScoped<VerifyMessageService>();
builder.Services.AddScoped<CodeFromUserService>();
builder.Services.AddHostedService<MessageCleanupService>();
builder.Services.AddScoped<MailingService>();
builder.Services.AddScoped<UserTasksRepository>();
builder.Services.AddScoped<UserTasksService>();
builder.Services.AddScoped<UserTasksValidator>();
builder.Services.AddScoped<JournalRepository>();
builder.Services.AddScoped<JournalService>();
builder.Services.AddScoped<JournalValidator>();



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
