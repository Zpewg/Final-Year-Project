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
    
    [HttpPost("get")]
    public async Task<ActionResult<List<UserTasksGlobal>>> GetUserTasksByUserId(User user)
    {
        List<UserTasksGlobal> userTasks = await _userTasksGlobalService.ReadUserTasksGlobal(user);
        if (userTasks.IsNullOrEmpty())
        {
            return BadRequest("no user Tasks");
        }
        return Ok(userTasks);
    }
}