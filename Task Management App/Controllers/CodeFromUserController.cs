using Microsoft.AspNetCore.Mvc;
using Task_Management_App.Entities;
using Task_Management_App.Repository;
using Task_Management_App.Service;


namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CodeFromUserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly CodeFromUserService _codeFromUserService;

    public CodeFromUserController(UserRepository userRepository, CodeFromUserService codeFromUserService)
    {
        _userRepository = userRepository;
        _codeFromUserService = codeFromUserService;
    }
    [HttpPost("code")]
    public async Task<ActionResult<string>> Register([FromBody] CodeFromUser codeMessage)
    {
        Console.WriteLine("Test 1 controller");
        int userId = await _userRepository.GetUserIdByEmail(codeMessage.Mail);
        codeMessage.UserId = userId;
        Console.WriteLine("Test 2 controller");
        if (await _codeFromUserService.CheckValidCode(userId, codeMessage.Code))
        {
            await _userRepository.UpdateUserStatus(userId, true);
            return Ok("User registered");
        }

        return BadRequest("Wrong code");
    }
    
}