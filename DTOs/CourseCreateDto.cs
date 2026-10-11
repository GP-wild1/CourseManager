using CourseManager.Models;

namespace CourseManager.DTOs;

public class CourseCreateDto
{
    public string CourseCode { get; set; } = string.Empty;

    public int SubjectId { get; set; }

    public string Semester { get; set; } = string.Empty;

    public int MaxStudents { get; set; }

    public CourseForm Form { get; set; }
    public List<User> Students { get; set; }
}