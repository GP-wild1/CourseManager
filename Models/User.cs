namespace CourseManager.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Student / Teacher / Admin
    public UserType Type { get; set; }
    // csak hallgatóknál számít
    public bool IsFullTime { get; set; } 

    // nappali / levelező (csak studentnél releváns)
    public TrainingType? TrainingType { get; set; }

    public bool IsActive { get; set; } = true;
    public StudyForm StudyForm { get; set; }
}

public enum UserType
{
    Student,
    Teacher,
    Admin
}

public enum TrainingType
{
    FullTime,
    PartTime
}