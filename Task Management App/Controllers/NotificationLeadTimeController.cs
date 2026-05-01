using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_App.Entities;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationLeadTimeController : ControllerBase
{
    
    private readonly NotificationLeadTimeService _notificationLeadTimeService;

    public NotificationLeadTimeController(NotificationLeadTimeService notificationLeadTimeService)
    {
        _notificationLeadTimeService = notificationLeadTimeService;
    }

    [HttpPost("insert")]
    public async Task<ActionResult> InsertNotificationLeadTime([FromBody] NotificationLeadTime msg)
    {
        string message = await _notificationLeadTimeService.AddNotificationLeadTime(msg);
        if(message == null)
        {
            return Ok();
        }
        return BadRequest(message);
    }

    [HttpPost("update")]
    public async Task<ActionResult> UpdateNotificationLeadTime([FromBody] NotificationLeadTime msg)
    {
        string message = await _notificationLeadTimeService.UpdateNotificationLeadTime(msg);
        if (message == null)
        {
            return Ok();
        }
        return BadRequest(message);
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteNotificationLeadTime([FromBody] NotificationLeadTime msg)
    {
        if (msg != null)
        {
            await _notificationLeadTimeService.DeleteNotificationLeadTime(msg);
            return Ok();
        }
        
        return BadRequest("Notification is null");
    }

    [HttpGet("get")]
    public async Task<ActionResult<List<NotificationLeadTime>>> GetNotificationLeadTime(int userId)
    {
        List<NotificationLeadTime> notificationList =
            await _notificationLeadTimeService.GetNotificationLeadTime(userId);

        if (notificationList.IsNullOrEmpty())
        {
            return BadRequest("No notification timers");
        }
        return Ok(notificationList);
    }
}