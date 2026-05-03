using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_App.Entities;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TaskSuggestionController : ControllerBase
{
    private  readonly TaskSuggestionService _taskSuggestionService;
    
    public TaskSuggestionController(TaskSuggestionService taskSuggestionService)
    {
        _taskSuggestionService = taskSuggestionService;
    }

    [HttpPost("enable")]
    public async Task<ActionResult<string>> EnableNotification([FromBody] TaskSuggestion notificationEnabled)
    {
        string msg = await _taskSuggestionService.EnableNotifications(notificationEnabled);

        if (msg == null)
        {
            return BadRequest("User not found");
        }

        return Ok(msg);
    }

    [HttpPost("disable")]
    public async Task<ActionResult<string>> DisableNotification([FromBody] TaskSuggestion taskSuggestion)
    {
        string msg = await _taskSuggestionService.DisableNotifications(taskSuggestion);

        if (msg == null)
        {
            return BadRequest("User not found");
        }
        return Ok(msg);
    }

    [HttpPost("get")]
    public async Task<ActionResult<List<TaskSuggestion>>> GetNotificationEnabled(int userId)
    {
        List<TaskSuggestion> list = await _taskSuggestionService.GetNotificationEnabledByUserId(userId);
        if (list.IsNullOrEmpty())
        {
            return BadRequest(list);
        }
        return Ok(list);
    }
}