using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManager.Data;
using CourseManager.DTOs;
using CourseManager.Models;

namespace CourseManager.Controllers;

[ApiController]
[Route("api/grades")]
public class GradeController : ControllerBase
{
    private readonly AppDbContext _context;

    public GradeController(AppDbContext context)
    {
        _context = context;
    }
    // =========================
    // GET GRADES BY EXAM
    // =========================
    [HttpGet]
    [Route("exam/{examId}")]
    public IActionResult GetGradesByExam(int examId)
    {
        var grades = _context.Grades
            .Include(g => g.Exam)
            .Include(g => g.Student)
            .Where(g => g.Exam.Id == examId)
            .ToList();

        return Ok(grades);
    }
    // =========================
    // wRITE GRADE FOR STUDENT
    // =========================
    [HttpPost]
    [Route("exam/{examId}/student/{studentId}")]
    public IActionResult AddGrade(int examId, int studentId, [FromBody] GradeCreateDto dto)
    {
        var exam = _context.Exams.Find(examId);
        var student = _context.Users.Find(studentId);

        if (exam == null || student == null)
            return NotFound();

        var grade = new Grade
        {
            ExamId = examId,
            StudentId = studentId,
            GradeValue = dto.Value
        };

        _context.Grades.Add(grade);
        _context.SaveChanges();

        return Ok(grade);
    }
}
