using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManager.Data;
using CourseManager.Models;
using CourseManager.DTOs;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public IActionResult GetAll()
    {
        var courses = _context.Courses
            .Include(c => c.Subject)
            .Include(c => c.Students)
            .ToList();

        return Ok(courses);
    }

    // =========================
    // GET BY ID
    // =========================
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var course = _context.Courses
            .Include(c => c.Subject)
            .Include(c => c.Students)
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
            return NotFound();

        return Ok(course);
    }

    // =========================
    // CREATE COURSE
    // =========================
    [HttpPost]
    public IActionResult Create(CourseCreateDto course)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == course.SubjectId);

        if (subject == null)
            return BadRequest("Subject not found");

        if (!subject.IsActive)
            return BadRequest("Subject is inactive");

        course.Students = new List<User>();

            var newCourse = new Course
            {
                CourseCode = course.CourseCode,
                Semester = course.Semester,
                MaxStudents = course.MaxStudents,
                Form = course.Form,
                SubjectId = course.SubjectId,
                Type = CourseType.Lecture, // Default érték, később lehet módosítani
                Hours = 0 // Default érték, később lehet módosítani
            };

        _context.Courses.Add(newCourse);
        _context.SaveChanges();

        return Ok(newCourse);
    }

    // =========================
    // UPDATE COURSE
    // =========================
    [HttpPut("{id}")]
    public IActionResult Update(int id, CourseUpdateDto updated)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);

        if (course == null)
            return NotFound();

        course.CourseCode = updated.CourseCode;
        course.Semester = updated.Semester;
        course.MaxStudents = updated.MaxStudents;
        course.Form = updated.Form;
        course.Type = updated.Type;
        course.Hours = updated.Hours;

        _context.SaveChanges();

        return Ok(course);
    }

    // =========================
    // DELETE COURSE
    // =========================
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var course = _context.Courses
            .Include(c => c.Students)
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
            return NotFound();

        if (course.Students.Any())
            return BadRequest("Cannot delete course with students");

        _context.Courses.Remove(course);
        _context.SaveChanges();

        return Ok("Deleted");
    }

    // =========================
    // ENROLL STUDENT
    // =========================
    [HttpPost("{courseId}/enroll")]
    public IActionResult Enroll(int courseId, int studentId)
    {
        var course = _context.Courses
            .Include(c => c.Students)
            .FirstOrDefault(c => c.Id == courseId);

        var student = _context.Users.FirstOrDefault(u => u.Id == studentId);

        if (course == null || student == null)
            return NotFound();

        if (student.Type != UserType.Student)
            return BadRequest("Only students can enroll");

        if (course.Students.Count >= course.MaxStudents)
            return BadRequest("Course is full");

        if ((int)course.Form != (int)student.StudyForm)        {
            return BadRequest("Study form mismatch");
        }

        if (course.Students.Any(s => s.Id == studentId))
            return BadRequest("Already enrolled");

        course.Students.Add(student);
        _context.SaveChanges();

        return Ok("Enrolled successfully");
    }

    // =========================
    // UNENROLL
    // =========================
    [HttpPost("{courseId}/unenroll")]
    public IActionResult Unenroll(int courseId, int studentId)
    {
        var course = _context.Courses
            .Include(c => c.Students)
            .FirstOrDefault(c => c.Id == courseId);

        if (course == null)
            return NotFound();

        var student = course.Students.FirstOrDefault(s => s.Id == studentId);

        if (student == null)
            return BadRequest("Student not enrolled");

        course.Students.Remove(student);
        _context.SaveChanges();

        return Ok("Unenrolled");
    }
}