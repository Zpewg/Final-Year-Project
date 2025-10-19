using Microsoft.AspNetCore.Mvc;
using Task_Management_App.Entities;


namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerifyMessageController : ControllerBase
{
    [HttpPost("register")]
    public ActionResult<string> Register([FromBody] VerifyMessage verifyMessage)
    {
        Console.WriteLine(verifyMessage.ToString());
        
        return Ok(new { message = "message registered" }); 
    }
    
}