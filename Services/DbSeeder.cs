using CourseManager.Data;
using CourseManager.Models;

namespace CourseManager.Services;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Users.Count() >= 5 || context.Subjects.Count() >= 5 || context.Courses.Count() >= 5)
            return;

        // USERS
        var teacher1 = new User
        {
            Username = "Teacher1",
            Email = "teacher@test.com",
            Password = "123",
            Type = UserType.Teacher,
            StudyForm = StudyForm.FullTime,
            IsActive = true
        };

        var teacher2 = new User
        {
            Username = "Teacher2",
            Email = "teacher.2@test.com",
            Password = "456",
            Type = UserType.Teacher,
            StudyForm = StudyForm.PartTime,
            IsActive = true
        };

        var student = new User
        {
            Username = "Student1",
            Email = "student@test.com",
            Password = "123",
            Type = UserType.Student,
            StudyForm = StudyForm.FullTime,
            IsActive = true
        };

        var student2 = new User
        {
            Username = "Student2",
            Email = "student.2@test.com",
            Password = "456",
            Type = UserType.Student,
            StudyForm = StudyForm.FullTime,
            IsActive = true
        };

        var student3 = new User
        {
            Username = "Student3",
            Email = "student.3@test.com",
            Password = "789",
            Type = UserType.Student,
            StudyForm = StudyForm.PartTime,
            IsActive = true
        };

        context.Users.AddRange(teacher1, teacher2, student, student2, student3);

        // SUBJECT
        var subject = new Subject
        {
            Code = "PROG1",
            Name = "Programming 1",
            Credits = 5,
            IsActive = true
        };

        var subject2 = new Subject
        {
            Code = "WEB-PROG1",
            Name = "Web-Programming 1",
            Credits = 3,
            IsActive = true
        };

        var subject3 = new Subject
        {
            Code = "MATH1",
            Name = "Matematics 1",
            Credits = 6,
            IsActive = true
        };

        context.Subjects.AddRange(subject, subject2, subject3);
        context.SaveChanges();

        // COURSE
        var course = new Course
        {
            CourseCode = "PROG1-01",
            Semester = "2025/1",
            MaxStudents = 30,
            Hours = 30,
            Type = CourseType.Lecture,
            Form = CourseForm.FullTime,
            SubjectId = subject.Id,
            TeacherId = teacher1.Id
        };

        var course2 = new Course
        {
            CourseCode = "WEB-PROG1-01",
            Semester = "2025/1",
            MaxStudents = 20,
            Hours = 25,
            Type = CourseType.Lecture,
            Form = CourseForm.FullTime,
            SubjectId = subject2.Id,
            TeacherId = teacher2.Id
        };

        var course3 = new Course
        {
            CourseCode = "MATH1-01",
            Semester = "2025/1",
            MaxStudents = 40,
            Hours = 30,
            Type = CourseType.Lecture,
            Form = CourseForm.FullTime,
            SubjectId = subject3.Id,
            TeacherId = teacher2.Id
        };

        context.Courses.AddRange(course, course2, course3);
        context.SaveChanges();

        // ENROLLMENT
        course.Students.AddRange(student, student2);
        context.SaveChanges();
        course2.Students.AddRange(student2, student3);
        context.SaveChanges();
        course3.Students.AddRange(student, student2, student3);
        context.SaveChanges();

        // SCHEDULE
        context.Schedules.Add(new Schedule
        {
            CourseId = course.Id,
            Day = DayOfWeek.Monday,
            StartTime = new TimeSpan(15, 0, 0),
            EndTime = new TimeSpan(16, 0, 0)
        });

        context.Schedules.Add(new Schedule
        {
            CourseId = course2.Id,
            Day = DayOfWeek.Wednesday,
            StartTime = new TimeSpan(12, 0, 0),
            EndTime = new TimeSpan(14, 0, 0)
        });

        context.Schedules.Add(new Schedule
        {
            CourseId = course3.Id,
            Day = DayOfWeek.Friday,
            StartTime = new TimeSpan(13, 0, 0),
            EndTime = new TimeSpan(14, 0, 0)
        });

        context.SaveChanges();
    }
}