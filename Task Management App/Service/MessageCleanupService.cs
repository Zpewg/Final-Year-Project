using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Task_Management_App.Repository;

namespace Task_Management_App.Service;

public class MessageCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MessageCleanupService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Pasul 1");
        // 1. Rulare imediată la pornirea aplicației (opțional)
        await RunCleanupAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        // 2. Bucla continuă la fiecare minut
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunCleanupAsync(stoppingToken);
        }
    }

    private async Task RunCleanupAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Pasul 2");
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<VerifyMessageRepository>();

            // Pasează token-ul mai departe
            await repository.RemoveExpiredMessagesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cleanup failed: {ex.Message}");
        }
    }
    
}