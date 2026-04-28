using Microsoft.EntityFrameworkCore;
using Task_Management_App.Controllers;
using Task_Management_App.DB;
using Task_Management_App.Repository;
using Task_Management_App.Service;
using Task_Management_App.Validators;
using NetTopologySuite.IO.Converters;
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
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Creăm un scope pentru a accesa serviciile înregistrate
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // ATENȚIE: Înlocuiește "TaskContext" cu numele real al clasei tale DbContext!
        var context = services.GetRequiredService<MyDBContext>(); 
        
        // Dacă folosești Entity Framework Migrations (recomandat):
        context.Database.Migrate(); 
        
        // Dacă NU folosești Migrations și vrei doar să forțezi crearea tabelelor direct:
        // context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "A apărut o eroare la crearea bazei de date.");
    }
}

app.Run();
