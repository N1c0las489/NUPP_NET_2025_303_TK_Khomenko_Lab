using System;
using System.Linq;

namespace Student.Common
{
    // Метод розширення
    public static class StudentExtensions
    {
        public static bool IsExcellentStudent(this StudentEntity student)
        {
            return student.AverageGrade >= 90;
        }

        public static string GetAcademicStatus(this StudentEntity student)
        {
            return student.AverageGrade switch
            {
                >= 90 => "Excellent",
                >= 75 => "Good",
                >= 60 => "Satisfactory",
                _ => "Needs Improvement"
            };
        }

        public static int GetYearsUntilGraduation(this StudentEntity student)
        {
            return Math.Max(0, 4 - student.Course);
        }
    }

    public static class TeacherExtensions
    {
        public static bool IsHighlyQualified(this Teacher teacher)
        {
            return teacher.Subjects.Count >= 3 && teacher.Salary > 50000;
        }

        public static string GetExperienceLevel(this Teacher teacher)
        {
            var yearsOfExperience = DateTime.Now.Year - teacher.BirthDate.Year - 25;
            return yearsOfExperience switch
            {
                >= 20 => "Senior",
                >= 10 => "Experienced",
                >= 5 => "Mid-level",
                _ => "Junior"
            };
        }
    }
}