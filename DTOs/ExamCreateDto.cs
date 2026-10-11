namespace CourseManager.DTOs;

public class ExamCreateDto
{
    public int SubjectId { get; set; }

    public string Semester { get; set; } = string.Empty;

    public DateTime ExamTime { get; set; }
}
