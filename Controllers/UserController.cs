using CourseManager.Data;
using CourseManager.Models;
using CourseManager.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL USERS
    // =========================
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }
    

    // POST /api/users/register
    [HttpPost("register")]
    public IActionResult Register(UserCreateDto dto)
    {
        var exists = _context.Users.Any(x => x.Email == dto.Email);
        if (exists)
            return BadRequest("Email already exists");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Password = dto.Password,
            Type = dto.Type,
            TrainingType = dto.TrainingType,
            IsActive = true
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(user);
    }

    // GET /api/users/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _context.Users.Find(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // PUT /api/users/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, UserUpdateDto dto)
    {
        var user = _context.Users.Find(id);

        if (user == null)
            return NotFound();

        user.Username = dto.Username;
        user.Email = dto.Email;

        _context.SaveChanges();

        return Ok(user);
    }

    // POST /api/users/{id}/deactivate
    [HttpPost("{id}/deactivate")]
    public IActionResult Deactivate(int id)
    {
        var user = _context.Users.Find(id);

        if (user == null)
            return NotFound();

        user.IsActive = false;
        _context.SaveChanges();

        return Ok("User deactivated");
    }

    // POST /api/users/{id}/reactivate
    [HttpPost("{id}/reactivate")]
    public IActionResult Reactivate(int id)
    {
        var user = _context.Users.Find(id);

        if (user == null)
            return NotFound();

        user.IsActive = true;
        _context.SaveChanges();

        return Ok("User reactivated");
    }
}