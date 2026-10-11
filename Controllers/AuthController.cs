using Microsoft.AspNetCore.Mvc;
using CourseManager.Data;
using CourseManager.Models;
using CourseManager.Services;
using CourseManager.DTOs;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public AuthController(AppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
        {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == dto.Email && x.Password == dto.Password);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _authService.GenerateToken(user);

        return Ok(new { token });
    }
}