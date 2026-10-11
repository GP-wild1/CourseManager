using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManager.Data;
using CourseManager.Models;
using CourseManager.DTOs;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/courses/{courseId}/schedule")]
public class ScheduleController : ControllerBase
{
    private readonly AppDbContext _context;

    public ScheduleController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET SCHEDULE
    // =========================
    [HttpGet]
    public IActionResult Get(int courseId)
    {
        var schedules = _context.Schedules
            .Where(s => s.CourseId == courseId)
            .ToList();

        return Ok(schedules);
    }

    // =========================
    // CREATE SCHEDULE
    // =========================
    [HttpPost]
    public IActionResult Create(int courseId, List<Schedule> schedules)
    {
        var course = _context.Courses.Find(courseId);

        if (course == null)
            return NotFound("Course not found");

        foreach (var schedule in schedules)
        {
            schedule.CourseId = courseId;
            _context.Schedules.Add(schedule);
        }

        _context.SaveChanges();

        return Ok("Schedule created");
    }

    // =========================
    // MODIFY SCHEDULE
    // =========================
    [HttpPost("modify")]
    public IActionResult Modify(int courseId, List<Schedule> newSchedules)
    {
        var oldSchedules = _context.Schedules
            .Where(s => s.CourseId == courseId)
            .ToList();

        _context.Schedules.RemoveRange(oldSchedules);

        foreach (var s in newSchedules)
        {
            s.CourseId = courseId;
            _context.Schedules.Add(s);
        }

        _context.SaveChanges();

        return Ok("Schedule updated");
    }
}