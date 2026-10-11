namespace CourseManager.Models;

public class Grade
{
    public int Id { get; set; }

    public int GradeValue { get; set; }

    public int ExamId { get; set; }
    public Exam Exam { get; set; } = null!;

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public DateTime DateRecorded { get; set; }
}
