namespace CourseManager.Models;

public class Exam
{
    public int Id { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    public string Semester { get; set; } = string.Empty;

    public DateTime ExamTime { get; set; }

    public List<User> Students { get; set; } = new();
}
