using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_App.DB;
using Task_Management_App.Entities;
using Task_Management_App.Service;

namespace Task_Management_App.Controllers;


[Route("api/[controller]")]
[ApiController]
public class WriteInJournalController : ControllerBase
{
    
    private readonly MyDBContext _context;
    private readonly JournalService _journalService;

    public WriteInJournalController(MyDBContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
    }

    [HttpPost("createJournal")]
    public async Task<ActionResult<string?>> CreateJournal([FromBody] Journal journal)
    {
        string? errorMsg = await _journalService.CreateJournal(journal);
        if (errorMsg.IsNullOrEmpty())
        {
            return Ok();
        }
        return BadRequest(errorMsg);
    }

    [HttpPost("updateJournal")]
    public async Task<ActionResult<string?>> UpdateJournal([FromBody] Journal journal)
    {
        string? errorMsg = await _journalService.Updatejournal(journal);
        if (errorMsg.IsNullOrEmpty())
        {
            return Ok();
        }
        return BadRequest(errorMsg);
    }

    [HttpGet("getJournal")]
    public async Task<ActionResult<List<Journal>>> GetJournal( int userId)
    {
        
        List<Journal> journals = await _journalService.GetAllJournals(userId);
        
        return Ok(journals);
    }

    [HttpPost("DeleteJournal")]
    public async Task<ActionResult<string>> DeleteJournal([FromBody] Journal journal)
    {
        if (await _journalService.DeleteJournal(journal))
        {
            return Ok();
        }
        return BadRequest();
    }
    
    
}