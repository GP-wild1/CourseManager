using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CourseManager.Data;
using CourseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Services;

public class NotificationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var now = DateTime.Now.TimeOfDay;
                var in30min = now.Add(TimeSpan.FromMinutes(30));

                var schedules = context.Schedules
                    .Include(s => s.Course)
                        .ThenInclude(c => c.Students)
                    .Include(s => s.Course)
                        .ThenInclude(c => c.Teacher)
                    .ToList();

                foreach (var s in schedules)
                {
                    var start = s.StartTime;

                    if (start > now && start <= in30min)
                    {
                        var course = s.Course;

                        // tanár
                        context.Notifications.Add(new Notification
                        {
                            UserId = course.TeacherId,
                            CourseId = course.Id,
                            Message = $"30 percen belül kezdődik: {course.CourseCode}"
                        });

                        // hallgatók
                        foreach (var student in course.Students)
                        {
                            context.Notifications.Add(new Notification
                            {
                                UserId = student.Id,
                                CourseId = course.Id,
                                Message = $"30 percen belül kezdődik: {course.CourseCode}"
                            });
                        }
                    }
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("NotificationService hiba: " + ex.Message);
            }

            await Task.Delay(60000, stoppingToken); // 1 perc
        }
    }
}