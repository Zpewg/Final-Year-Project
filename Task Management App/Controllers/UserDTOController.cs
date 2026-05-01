using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_App.Entities;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserDTOController : ControllerBase
{
   
    private readonly UserService _userService;

    public UserDTOController(UserService userService)
    {
       
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<List<string>>> UserDTORegister([FromBody] UserDTO userDTO)
    {
        Console.WriteLine(userDTO);
        List<string> errors  =  await _userService.CreateUser(userDTO);
        
        if (errors.IsNullOrEmpty())
        {
            return Ok(new { errors });
        }
        return BadRequest(errors);
    }
}