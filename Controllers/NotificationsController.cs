using Microsoft.AspNetCore.Mvc;
using CourseManager.Data;
using CourseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationsController(AppDbContext context)
    {
        _context = context;
    }

    // Összes notification
    [HttpGet]
    public IActionResult Get(int? userId)
    {
        var query = _context.Notifications
            .Include(n => n.User)
            .Include(n => n.Course)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(n => n.UserId == userId.Value);
        }

        return Ok(query.ToList());
    }

    // Egy notification ID alapján
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var notification = _context.Notifications
            .Include(n => n.User)
            .Include(n => n.Course)
            .FirstOrDefault(n => n.Id == id);

        if (notification == null)
            return NotFound();

        return Ok(notification);
    }

    // Manuális létrehozás (teszteléshez)
    [HttpPost]
    public IActionResult Create(Notification notification)
    {
        _context.Notifications.Add(notification);
        _context.SaveChanges();

        return Ok(notification);
    }

    // Törlés
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var notification = _context.Notifications.Find(id);

        if (notification == null)
            return NotFound();

        _context.Notifications.Remove(notification);
        _context.SaveChanges();

        return Ok();
    }
}