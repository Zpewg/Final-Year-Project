using Microsoft.EntityFrameworkCore;
using Serilog;
using Task_Management_App.DB;
using Task_Management_App.Repository;
using Task_Management_App.Service;
using Task_Management_App.Validators;
using NetTopologySuite.IO.Converters;
using StackExchange.Redis;
using Task_Management_App.Hubs;
using Task_Management_App.Services;

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
builder.Services.AddScoped<UserTasksGlobalRepository>(); 
builder.Services.AddScoped<UserTasksGlobalService>();
builder.Services.AddScoped<UserTasksGlobalValidator>();
builder.Services.AddScoped<NotificationEnabledRepository>();
builder.Services.AddScoped<NotificationEnabledService>();
builder.Services.AddScoped<NotificationLeadTimeRepository>();
builder.Services.AddScoped<NotificationLeadTimeService>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<NotificationWorker>();
builder.Host.UseSerilog();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
{
    var connectionString = builder.Configuration.GetConnectionString("RedisConnection");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Redis connection string 'RedisConnection' is missing from configuration.");
    }
    
    return ConnectionMultiplexer.Connect(connectionString);
});

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Add the GeoJsonConverterFactory to handle Point, Polygon, etc.
        var geoJsonConverterFactory = new GeoJsonConverterFactory();
        options.JsonSerializerOptions.Converters.Add(geoJsonConverterFactory);
        
        // This is the specific setting the error message suggested as a fallback, 
        // though the GeoJson converter usually solves the root cause.
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
    });


var app = builder.Build();
Console.WriteLine("CONN: " + app.Configuration.GetConnectionString("DefaultConnection"));


    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
    
        var context = services.GetRequiredService<MyDBContext>(); 
        
       
        context.Database.Migrate(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "A apărut o eroare la crearea bazei de date.");
    }
}



app.Run();
