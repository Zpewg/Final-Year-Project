using Microsoft.AspNetCore.Mvc;
using Task_Management_App.Service;

[ApiController]
[Route("api/[controller]")]
public class OptimizationController : ControllerBase
{
    private readonly UserTasksService _taskService;

    public OptimizationController(UserTasksService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("suggest/{userId}")]
    public async Task<IActionResult> GetSuggestions(int userId)
    {
        var suggestions = await _taskService.GetOptimizationSuggestions(userId);
        
        if (suggestions == null || !suggestions.Any())
        {
            return NoContent(); // Nicio optimizare necesară
        }

        return Ok(suggestions);
    }
}