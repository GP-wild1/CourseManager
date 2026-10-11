using System.ComponentModel.DataAnnotations;

namespace CourseManager.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    public string Semester { get; set; } = string.Empty;

    public int Hours { get; set; }
    [Required]
    public int MaxStudents { get; set; }
    public CourseType Type { get; set; }
    public CourseForm Form { get; set; }

    // Subject kapcsolat
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;

    // Oktató
    public int TeacherId { get; set; }
    public User Teacher { get; set; } = null!;

    // Hallgatók 
    public List<User> Students { get; set; } = new();

    // Órarend
    public List<Schedule> Schedules { get; set; } = new();
}