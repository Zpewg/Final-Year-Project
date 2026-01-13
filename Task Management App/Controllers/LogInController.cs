
using Microsoft.AspNetCore.Mvc;
using Task_Management_App.Entities;
using Task_Management_App.Repository;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogInController  : ControllerBase
{

    private readonly UserRepository _userRepository;
    private readonly UserService _userService;
    private readonly MailingService _mailService;
    private readonly VerifyMessageService _verifyMessageService;

    public LogInController(UserRepository userRepository, UserService userService, MailingService mailService, VerifyMessageService verifyMessageService)
    {
        _userRepository = userRepository;
        _userService = userService;
        _mailService = mailService;
        _verifyMessageService = verifyMessageService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginUser([FromBody] LogIn login)
    {
        User user = await _userRepository.GetUserByEmail(login.Mail);
        Console.WriteLine(user.ToString());
        if (user != null && await _userService.CheckPassword(user,  login.Password) && user.Active)
        {
            return Ok(new { data = user , message = "Login succesful"});
        }
        if (user.Active == false)
        {
           string code = _mailService.MailToUser(user.Email);

           VerifyMessage message = new VerifyMessage(code, user.UserId, CodeType.ActivateAccount);
           
           _verifyMessageService.SendVerifyMessageAsync(message);
           
           return Ok(new { data = user , message = "Login failed" });
        }
        return BadRequest(new {data=(object?) null, message = "Invalid email or password"});
    }
}