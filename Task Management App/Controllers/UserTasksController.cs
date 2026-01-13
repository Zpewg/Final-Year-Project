using Microsoft.IdentityModel.Tokens;
using Task_Management_App.DB;

namespace Task_Management_App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Task_Management_App.Entities;
using Task_Management_App.Service;

[Route("api/[controller]")]
[ApiController]
public class UserTasksController : ControllerBase
{
    private readonly MyDBContext _context;
    private readonly UserTasksService _userTaskService;

    public UserTasksController(MyDBContext context, UserTasksService userTaskService)
    {
        _context = context;
        _userTaskService = userTaskService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<List<string>>> CreateUserTask(UserTasks userTasks)
    {
        List<string> errors = await _userTaskService.CreateUserTasks(userTasks);
        if (errors.IsNullOrEmpty())
        {
            return Ok(new { errors });
        }
        return BadRequest(errors);
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteUserTask(int userTaskId)
    {
        await _userTaskService.DeleteUserTask(userTaskId);
        return Ok();
    }

    [HttpPost("update")]
    public async Task<ActionResult> UpdateUserTask(UserTasks userTasks)
    {
        await _userTaskService.UpdateUserTask(userTasks);
        return Ok();
    }

    [HttpGet("get")]
    public async Task<ActionResult<List<UserTasks>>> GetUserTasksByUserId(int userId)
    {
        List<UserTasks> userTasks = await _userTaskService.GetUserTasksByUserId(userId);
        return Ok(userTasks);
    }
}