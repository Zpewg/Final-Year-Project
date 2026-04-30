using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_App.DB;
using Task_Management_App.Entities;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserTasksGlobalController : ControllerBase
{
    private readonly MyDBContext _context;
    private readonly UserTasksGlobalService _userTasksGlobalService;

    public UserTasksGlobalController(MyDBContext context, UserTasksGlobalService userTasksGlobalService)
    {
        _context = context;
        _userTasksGlobalService= userTasksGlobalService;
    }
    
    [HttpPost("create")]
    public async Task<ActionResult<List<string>>> CreateUserTask([FromBody] GlobalTaskRequestDto request)
    {
        Console.WriteLine("Am primit requestul");
       
        UserTasksGlobal userTasks = request.Task;
        User user = request.User;
        Console.WriteLine("Am primit obiectele din DTO-ul:");
        Console.WriteLine(userTasks.ToString());
        Console.WriteLine(user.ToString());

        
        if (user.Location == null)
        {
            return BadRequest(new List<string> { "Locația lipsește. Activează 'Share Location' mai întâi." });
        }
        
        Console.WriteLine(user.UserId);
        userTasks.Location = user.Location;
        userTasks.UserId = user.UserId; 

        List<string> errors = await _userTasksGlobalService.CreateUserTasksGlobal(userTasks);
        
        if (errors.IsNullOrEmpty())
        {
            return Ok(new { errors });
        }
        return BadRequest(errors);
    }

    [HttpPost("delete")]
    public async Task<ActionResult<List<string>>> DeleteUserTask([FromBody] UserTasksGlobal userTasks)
    {
        string message = await _userTasksGlobalService.DeleteUserTasksGlobal(userTasks);

        if (message.IsNullOrEmpty())
        {
            return BadRequest("no user Tasks");
        }
        return Ok(message);
    }
    
    [HttpPost("get")]
    public async Task<ActionResult<List<UserTasksGlobal>>> GetUserTasksByUserKm(User user)
    {
        List<UserTasksGlobal> userTasks = await _userTasksGlobalService.ReadUserTasksGlobal(user);
        if (userTasks.IsNullOrEmpty())
        {
            return BadRequest("no user Tasks");
        }
        return Ok(userTasks);
    }

    [HttpPost("getGlobalTasksById")]
    public async Task<ActionResult<List<UserTasksGlobal>>> GetGlobalTasksByUserId([FromBody]User user)
    {
        List<UserTasksGlobal> userTasksGlobals = await _userTasksGlobalService.ReadUserTasksGlobalById(user);
        if (userTasksGlobals.IsNullOrEmpty())
        {
            return BadRequest("no user Tasks");
        }
        return Ok(userTasksGlobals);
    }

    [HttpPost("update")]
    public async Task<ActionResult<List<string>>> UpdateUserTask([FromBody] UserTasksGlobal userTasks)
    {
        List<string> message = await _userTasksGlobalService.UpdateUserTasksGlobal(userTasks);
        if (message.Any())
        {
            return BadRequest(message);
        }
        return Ok("Task updated");
    }
}