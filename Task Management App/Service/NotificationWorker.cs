using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Task_Management_App.DB;
using Task_Management_App.Hubs;
using Task_Management_App.Service;

namespace Task_Management_App.Services;

public class NotificationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnectionMultiplexer _redis;
    private readonly IHubContext<NotificationHub> _hubContext;


    public NotificationWorker(IServiceScopeFactory scopeFactory, IConnectionMultiplexer redis, IHubContext<NotificationHub> hubContext)
    {
        _scopeFactory = scopeFactory;
        _redis = redis;
        _hubContext = hubContext;
       
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"[Worker] Worker started at: {DateTime.Now}");
        
        var dbRedis = _redis.GetDatabase();

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MyDBContext>();
                var now = DateTime.Now;

                //Users with notification on and their tasks and leadtimes
                var activeNotifications = await context.NotificationEnabled
                    .Where(ne => ne.IsEnabled)
                    .Select(ne => new
                    {
                        ne.UserId,
                        ne.NotificationId,
                        Tasks = context.UserTasks.Where(t => t.UserId == ne.UserId).ToList(),
                        LeadTimes = context.NotificationLeadTime.Where(lt => lt.NotificationId == ne.NotificationId).ToList()
                    })
                    .ToListAsync();

                foreach (var config in activeNotifications)
                {
                    foreach (var task in config.Tasks)
                    {
                     
                        DateTime taskDateTime = task.Date.ToDateTime(task.Time);

                        foreach (var lead in config.LeadTimes)
                        {
                            DateTime triggerTime = taskDateTime.AddMinutes(-lead.NotificationTime);

                          
                            if (now >= triggerTime.AddMinutes(-2) && now <= triggerTime.AddMinutes(2))
                            {
                                // We use redis to be sure the notification is sent only once
                                string redisKey = $"notif:{config.UserId}:{task.UserTaskId}:{lead.NotificationTime}";
                                
                                // SETNX: adds it if it doens't already exists
                                if (await dbRedis.StringSetAsync(redisKey, "sent", TimeSpan.FromHours(24), When.NotExists))
                                {
                                    
                                    //WE sent through websockets
                                    await _hubContext.Clients.Group(config.UserId.ToString())
                                        .SendAsync("ReceiveNotification", $"Reminder: {task.NameOfTask} ");

                                    // Get the user's email from the scope
                                    var user = await context.Users.FindAsync(config.UserId);
                                    if (user != null) 
                                    {
                                        
                                        var mailingService = scope.ServiceProvider.GetRequiredService<MailingService>();
                                        await mailingService.SendTaskReminderEmail(user.Email, task.NameOfTask, task.Description);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}