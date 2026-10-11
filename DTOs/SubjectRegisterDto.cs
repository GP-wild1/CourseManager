namespace CourseManager.DTOs;

public class SubjectRegisterDto
{
    public int StudentId { get; set; }

    public List<int> CourseIds { get; set; } = new();
}