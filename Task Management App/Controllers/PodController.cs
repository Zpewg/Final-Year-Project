using Microsoft.AspNetCore.Mvc;

namespace Task_Management_App.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PodController : ControllerBase
{
    [HttpGet("Kill")]
    public IActionResult KillPod()
    {
        Environment.Exit(1);
        return Ok();
    }
}