using CourseManager.Models;

namespace CourseManager.DTOs;

public class UserCreateDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public UserType Type { get; set; }
    public TrainingType? TrainingType { get; set; }
}