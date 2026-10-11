using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManager.Data;
using CourseManager.DTOs;
using CourseManager.Models;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/exams")]
public class ExamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExamsController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET ALL
    // =========================
    [HttpGet]
    public IActionResult GetAll()
    {
        var exams = _context.Exams
            .Include(e => e.Subject)
            .Include(e => e.Students)
            .ToList();

        return Ok(exams);
    }

    // =========================
    // CREATE EXAM
    // =========================
    [HttpPost]
    public IActionResult Create(ExamCreateDto dto)
    {
        var subject = _context.Subjects.Find(dto.SubjectId);

        if (subject == null)
            return BadRequest("Subject not found");

        var hasCourseInSemester = _context.Courses
            .Any(c => c.SubjectId == dto.SubjectId && c.Semester == dto.Semester);

        if (!hasCourseInSemester)
            return BadRequest("Subject has no course in this semester");

        var exam = new Exam
        {
            SubjectId = dto.SubjectId,
            Semester = dto.Semester,
            ExamTime = dto.ExamTime
        };

        _context.Exams.Add(exam);
        _context.SaveChanges();

        return Ok(exam);
    }

    // =========================
    // DELETE EXAM
    // =========================
    [HttpDelete("{examId}")]
    public IActionResult Delete(int examId)
    {
        var exam = _context.Exams
            .Include(e => e.Students)
            .FirstOrDefault(e => e.Id == examId);

        if (exam == null)
            return NotFound();

        _context.Exams.Remove(exam);
        _context.SaveChanges();

        return Ok("Deleted");
    }

    // =========================
    // REGISTER STUDENT
    // =========================
    [HttpPost("{examId}/registration")]
    public IActionResult Register(int examId, ExamRegistrationDto dto)
    {
        var exam = _context.Exams
            .Include(e => e.Students)
            .FirstOrDefault(e => e.Id == examId);

        if (exam == null)
            return NotFound("Exam not found");

        var user = _context.Users.Find(dto.UserId);

        if (user == null)
            return NotFound("User not found");

        if (user.Type != UserType.Student)
            return BadRequest("Only students can register for exams");

        var studentHasSubjectInSemester = _context.Courses
            .Include(c => c.Students)
            .Any(c =>
                c.SubjectId == exam.SubjectId &&
                c.Semester == exam.Semester &&
                c.Students.Any(s => s.Id == dto.UserId));

        if (!studentHasSubjectInSemester)
            return BadRequest("Student has not registered for this subject in this semester");

        if (exam.Students.Any(s => s.Id == dto.UserId))
            return BadRequest("Student already registered for this exam");

        exam.Students.Add(user);
        _context.SaveChanges();

        return Ok("Registered successfully");
    }
}

[ApiController]
[Route("api/subjects/{subjectId}/exams")]
public class SubjectExamsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectExamsController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET BY SUBJECT AND SEMESTER
    // =========================
    [HttpGet]
    public IActionResult GetBySubjectAndSemester(int subjectId, [FromQuery] string semester)
    {
        var subjectExists = _context.Subjects.Any(s => s.Id == subjectId);

        if (!subjectExists)
            return NotFound("Subject not found");

        var exams = _context.Exams
            .Include(e => e.Subject)
            .Include(e => e.Students)
            .Where(e => e.SubjectId == subjectId && e.Semester == semester)
            .ToList();

        return Ok(exams);
    }
}
