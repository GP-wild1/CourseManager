using CourseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Exam> Exams { get; set; }

    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // COURSE - STUDENT
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Students)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "CourseUser",
                j => j.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("StudentsId")
                    .OnDelete(DeleteBehavior.NoAction),

                j => j.HasOne<Course>()
                    .WithMany()
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.NoAction),

                j =>
                {
                    j.ToTable("CourseUser");
                    j.HasKey("CourseId", "StudentsId");
                }
            );
        
        // Course - Teacher
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.NoAction);

        // Course - Subject
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Subject)
            .WithMany(s => s.Courses)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Exam - Subject
        modelBuilder.Entity<Exam>()
            .HasOne(e => e.Subject)
            .WithMany()
            .HasForeignKey(e => e.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // EXAM - STUDENT
        modelBuilder.Entity<Exam>()
            .HasMany(e => e.Students)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ExamUser",
                j => j.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("StudentsId")
                    .OnDelete(DeleteBehavior.NoAction),

                j => j.HasOne<Exam>()
                    .WithMany()
                    .HasForeignKey("ExamId")
                    .OnDelete(DeleteBehavior.Cascade),

                j =>
                {
                    j.ToTable("ExamUser");
                    j.HasKey("ExamId", "StudentsId");
                }
            );

        // Grade
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Exam)
            .WithMany()
            .HasForeignKey(g => g.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany()
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Grade>()
            .HasIndex(g => new { g.ExamId, g.StudentId })
            .IsUnique();
    }
}
