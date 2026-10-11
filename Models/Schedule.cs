namespace CourseManager.Models;

public class Schedule
{
    public int Id { get; set; }

    public DayOfWeek Day { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}