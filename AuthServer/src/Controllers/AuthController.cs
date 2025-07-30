using Microsoft.AspNetCore.Mvc;
using AuthServer.src.Data;
using AuthServer.src.Models;
using AuthServer.src.Helpers;

namespace AuthServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        if (_context.Users.Any(u => u.Username == user.Username))
            return BadRequest("Username already exists.");

        user.PasswordHash = PasswordHasher.Hash(user.PasswordHash);
        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok("Registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login(User user)
    {
        var existing = _context.Users.FirstOrDefault(u => u.Username == user.Username);
        if (existing == null || !PasswordHasher.Verify(user.PasswordHash, existing.PasswordHash))
            return Unauthorized("Invalid username or password.");

        return Ok("Login successful.");
    }

    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _context.Users
            .Select(u => new { u.Id, u.Username }) // chỉ trả về thông tin cần thiết
            .ToList();

        return Ok(users);
    }


}
