using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManager.Data;
using CourseManager.Models;
using CourseManager.DTOs;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/subjects")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Subjects.ToList());
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var subject = _context.Subjects.Find(id);

        if (subject == null)
            return NotFound();

        return Ok(subject);
    }

    // =========================
    // CREATE
    // =========================
    [HttpPost]
    public IActionResult Create(Subject subject)
    {
        _context.Subjects.Add(subject);
        _context.SaveChanges();

        return Ok(subject);
    }

    // =========================
    // UPDATE
    // =========================
    [HttpPut("{id}")]
    public IActionResult Update(int id, Subject updated)
    {
        var subject = _context.Subjects.Find(id);

        if (subject == null)
            return NotFound();

        subject.Code = updated.Code;
        subject.Name = updated.Name;
        subject.Credits = updated.Credits;

        _context.SaveChanges();

        return Ok(subject);
    }

    // =========================
    // DEACTIVATE
    // =========================
    [HttpPost("{id}/deactivate")]
    public IActionResult Deactivate(int id)
    {
        var subject = _context.Subjects.Find(id);

        if (subject == null)
            return NotFound();

        subject.IsActive = false;
        _context.SaveChanges();

        return Ok("Subject deactivated");
    }

    // =========================
    // REACTIVATE
    // =========================
    [HttpPost("{id}/reactivate")]
    public IActionResult Reactivate(int id)
    {
        var subject = _context.Subjects.Find(id);

        if (subject == null)
            return NotFound();

        subject.IsActive = true;
        _context.SaveChanges();

        return Ok("Subject reactivated");
    }

    // =========================
    // REGISTER
    // =========================
    [HttpPost("{subjectId}/register")]
    public IActionResult Register(int subjectId, SubjectRegisterDto dto)
    {
        var student = _context.Users.Find(dto.StudentId);
        if (student == null)
            return NotFound("Student not found");

        var courses = _context.Courses
            .Where(c => dto.CourseIds.Contains(c.Id))
            .Include(c => c.Students)
            .ToList();

        if (!courses.Any())
            return BadRequest("Courses not found");

        // 1. ugyanahhoz a tárgyhoz tartozzanak
        if (courses.Any(c => c.SubjectId != subjectId))
            return BadRequest("Courses must belong to same subject");

        // 2. ugyanaz a félév
        var semester = courses.First().Semester;
        if (courses.Any(c => c.Semester != semester))
            return BadRequest("Courses must be in same semester");

        // 3. képzési forma ellenőrzés
        foreach (var course in courses)
        {
            if (course.Form != CourseForm.Both &&
                (
                    (course.Form == CourseForm.FullTime && student.IsFullTime == false) ||
                    (course.Form == CourseForm.PartTime && student.IsFullTime == true)
                ))
            {
                return BadRequest("Invalid study form for this course");
            }       
        }

        // 4. max létszám
        foreach (var course in courses)
        {
            if (course.Students.Count >= course.MaxStudents)
                return BadRequest($"Course {course.Id} is full");
        }

        // 5. már felvette ezt a tárgyat?
        var existingCourses = _context.Courses
            .Include(c => c.Students)
            .Where(c => c.SubjectId == subjectId && c.Students.Any(s => s.Id == student.Id))
            .ToList();

        if (existingCourses.Any())
            return BadRequest("Already registered for this subject");

        // 6. FELVÉTEL
        foreach (var course in courses)
        {
            course.Students.Add(student);
        }

        _context.SaveChanges();

        return Ok("Registered successfully");
    }

    // =========================
    // UNREGISTER
    // =========================
    [HttpPost("{subjectId}/unregister")]
    public IActionResult Unregister(int subjectId, int studentId)
    {
        var courses = _context.Courses
            .Include(c => c.Students)
            .Where(c => c.SubjectId == subjectId && c.Students.Any(s => s.Id == studentId))
            .ToList();

        if (!courses.Any())
            return NotFound();

        foreach (var course in courses)
        {
            var student = course.Students.First(s => s.Id == studentId);
            course.Students.Remove(student);
        }

        _context.SaveChanges();

        return Ok("Unregistered");
    }
}