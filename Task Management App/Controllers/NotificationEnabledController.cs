using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_App.Entities;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationEnabledController : ControllerBase
{
    private  readonly NotificationEnabledService _notificationEnabledService;
    
    public NotificationEnabledController(NotificationEnabledService notificationEnabled)
    {
        _notificationEnabledService = notificationEnabled;
    }

    [HttpPost("enable")]
    public async Task<ActionResult<string>> EnableNotification([FromBody] NotificationEnabled notificationEnabled)
    {
        string msg = await _notificationEnabledService.EnableNotifications(notificationEnabled);

        if (msg == null)
        {
            return BadRequest("User not found");
        }

        return Ok(msg);
    }

    [HttpPost("disable")]
    public async Task<ActionResult<string>> DisableNotification([FromBody] NotificationEnabled notificationEnabled)
    {
        string msg = await _notificationEnabledService.DisableNotifications(notificationEnabled);

        if (msg == null)
        {
            return BadRequest("User not found");
        }
        return Ok(msg);
    }

    [HttpPost("get")]
    public async Task<ActionResult<List<NotificationEnabled>>> GetNotificationEnabled(int userId)
    {
        List<NotificationEnabled> list = await _notificationEnabledService.GetNotificationEnabledByUserId(userId);
        if (list.IsNullOrEmpty())
        {
            return BadRequest(list);
        }
        return Ok(list);
    }
}