using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Management_App.DB;
using Task_Management_App.Entities;


namespace Task_Management_App.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    
    private readonly MyDBContext _context;

    public UserController(MyDBContext context)
    {
        _context = context;
    }
    /*
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }*/
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return user;
    }
    
    
    //Method to create user
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUsers", new { id = user.UserId }, user);
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
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDTO user)
    {
        Console.WriteLine(user.ToString());
        return Ok(new { message = "User registered" });
    }


}