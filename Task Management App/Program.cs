using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Repository;
using Task_Management_App.Service;
using Task_Management_App.Validators;
using NetTopologySuite.IO.Converters;
using StackExchange.Redis;
using Task_Management_App.Hubs;
using Task_Management_App.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;



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
builder.Services.AddScoped<TaskSuggestionRepository>();
builder.Services.AddScoped<TaskSuggestionService>();
builder.Services.AddScoped<ScheduleOptimizerService>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<NotificationWorker>();

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


builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing SQL Connection"),
        name: "Database (SQL Server)",
        tags: new[] { "db", "sql", "ready" })
    .AddRedis(
        builder.Configuration.GetConnectionString("RedisConnection") ?? throw new InvalidOperationException("Missing Redis Connection"),
        name: "Redis Cache",
        tags: new[] { "cache", "redis", "ready" });


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
        var context = services.GetRequiredService<MyDBContext>(); // Schimbă cu numele contextului tău (ex: TaskDbContext)
        context.Database.Migrate(); // Asta aplică automat ultima migrare!
        Console.WriteLine("Baza de date a fost updatată cu succes!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Eroare la migrare: {ex.Message}");
    }   
}
app.MapHealthChecks("/health", new HealthCheckOptions
{
    // Aici formatăm răspunsul ca să fie un JSON frumos, nu doar un text chior cu "Healthy"
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(e => new
            {
                component = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});


app.Run();
