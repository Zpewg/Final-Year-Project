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
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<VerifyMessageRepository>();

                    await repository.RemoveExpiredMessagesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cleanup failed: {ex.Message}");
            }
        }
    }
    
}