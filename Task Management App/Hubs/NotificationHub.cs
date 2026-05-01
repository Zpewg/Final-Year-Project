using Microsoft.AspNetCore.SignalR;

namespace Task_Management_App.Hubs;

public class NotificationHub : Hub
{
    
    public async Task JoinUserGroup(int userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
    }
}