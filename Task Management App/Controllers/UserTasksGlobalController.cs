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
        // 1. Extragem obiectele din DTO-ul primit
        UserTasksGlobal userTasks = request.Task;
        User user = request.User;

        // 2. Aplicăm logica
        if (user.Location == null)
        {
            return BadRequest(new List<string> { "Locația lipsește. Activează 'Share Location' mai întâi." });
        }
        
        userTasks.Location = user.Location;
        userTasks.UserId = user.UserId; 

        List<string> errors = await _userTasksGlobalService.CreateUserTasksGlobal(userTasks);
        
        if (errors.IsNullOrEmpty())
        {
            return Ok(new { errors });
        }
        return BadRequest(errors);
    }
    
    [HttpGet("get")]
    public async Task<ActionResult<List<UserTasksGlobal>>> GetUserTasksByUserId(User user, int km)
    {
        List<UserTasksGlobal> userTasks = await _userTasksGlobalService.ReadUserTasksGlobal(user.Location, km);
        return Ok(userTasks);
    }
}