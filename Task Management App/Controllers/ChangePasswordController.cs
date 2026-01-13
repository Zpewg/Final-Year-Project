using Microsoft.AspNetCore.Mvc;
using Task_Management_App.Repository;
using Task_Management_App.Entities;
using Task_Management_App.Service;
using Task_Management_App.Validators;

namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChangePasswordController : ControllerBase
{

    private readonly UserRepository _userRepository;
    private readonly MailingService _mailingService;
    private readonly VerifyMessageService _verifyMessageService;
    private readonly CodeFromUserService _codeFromUserService;
    private readonly UserService _userService;

    public ChangePasswordController(UserRepository userRepository, MailingService mailingService,  VerifyMessageService verifyMessageService, CodeFromUserService codeFromUserService, UserService userService)
    {
        _userRepository = userRepository;
        _mailingService = mailingService;
        _codeFromUserService = codeFromUserService;
        _verifyMessageService = verifyMessageService;
        _userService = userService;
    }
    
    [HttpPost("CheckMailExist")]
    public async Task<ActionResult> CheckMail([FromBody] string mail)
    {
        User user = await _userRepository.GetUserByEmail(mail);
        VerifyMessage message = new VerifyMessage();
        if (user != null)
        {
           string code = _mailingService.MailToUser(mail);
           message.UsersId = user.UserId;
           message.VerifysMessage = code;
           message.code = CodeType.RecoverPassword;
           await _verifyMessageService.SendVerifyMessageAsync(message);
            return Ok();
        }
        return BadRequest("No user associated to the email");
    }

    
    [HttpPost("CheckCodeExist")]
    public async Task<ActionResult> CheckCodeExist([FromBody] CodeFromUser codeMessage)
    {
        User user = await _userRepository.GetUserByEmail(codeMessage.Mail);
        codeMessage.UserId = user.UserId;
        if(await _codeFromUserService.CheckValidCode(user.UserId, codeMessage.Code))
        {
            return Ok("Code is valid");
        }
        
        return BadRequest("Wrong code");
    }

    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] NewPasswordFromUser passwordFromUser)
    {
        UserValidator _userValidator = new UserValidator(_userRepository);
        int userId = await _userRepository.GetUserIdByEmail(passwordFromUser.UserEmail);
        Console.WriteLine(passwordFromUser.NewPassword, passwordFromUser.UserEmail);
        string message = _userValidator.PasswordIsNotCorrect(passwordFromUser.NewPassword);
        if (message == null)
        {
            await _userService.UpdateUserPassword(userId, passwordFromUser.NewPassword);
            return Ok("Password changed");
        }
        return BadRequest(message);
    }
    
    
    
    
}