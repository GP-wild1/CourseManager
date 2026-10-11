using System.ComponentModel.DataAnnotations;

namespace CourseManager.Models;

public class Subject
{
    public int Id { get; set; }

    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public int Credits { get; set; }

    public bool IsActive { get; set; } = true;

    public List<Course> Courses { get; set; } = new();
}