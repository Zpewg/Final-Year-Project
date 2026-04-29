using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Geometries;
using Task_Management_App.DB;
using Task_Management_App.Entities;
using Task_Management_App.Service;


namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    
    private readonly MyDBContext _context;
    private readonly UserService _userService;

    
    public UserController(MyDBContext context, UserService userService)
    {
        _context = context;
        _userService = userService ??  throw new ArgumentNullException(nameof(userService));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return user;
    }
    
    
    
    //Method to delete user returns 204 code
    [HttpDelete("{userId}")]
    public async Task<ActionResult<User>> DeleteUser(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("location")]
    public async Task<ActionResult> UpdateLocation([FromBody] LocationDTO dto)
    {
        if (dto.Km <= 0 || dto.Km > 16)
            return BadRequest("Invalid km range");

        var point = new Point(dto.Longitude, dto.Latitude)
        {
            SRID = 4326
        };

        var result = await _userService.UpdateUserLocation(dto.UserId, point, dto.Km);

        return Ok(new
        {
            message = result,
            lat = dto.Latitude,
            lng = dto.Longitude
        });
    }


}