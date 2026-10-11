using CourseManager.Models;

namespace CourseManager.DTOs;

public class CourseUpdateDto
{
    public string CourseCode { get; set; } = string.Empty;

    public string Semester { get; set; } = string.Empty;

    public int MaxStudents { get; set; }

    public CourseForm Form { get; set; }
    public CourseType Type { get; set; }
    public int Hours { get; set; }
}