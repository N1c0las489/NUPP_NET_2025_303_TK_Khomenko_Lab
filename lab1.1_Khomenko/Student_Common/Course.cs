using System;

namespace Student.Common
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Credits { get; set; }
        public Teacher Instructor { get; set; }

        // Статичне поле для підрахунку курсів
        private static int _totalCourses = 0;

        // Статичний конструктор
        static Course()
        {
            _totalCourses = 0;
        }

        // Конструктор
        public Course()
        {
            Id = Guid.NewGuid();
            _totalCourses++;
        }

        // Конструктор з параметрами
        public Course(string name, string code, int credits) : this()
        {
            Name = name;
            Code = code;
            Credits = credits;
        }

        // Статичний метод
        public static int GetTotalCoursesCount()
        {
            return _totalCourses;
        }

        // Метод для перевірки валідності курсу
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Code) &&
                   Credits > 0;
        }

        public override string ToString()
        {
            var instructorName = Instructor?.GetFullName() ?? "No instructor assigned";
            return $"{Name} ({Code}) - {Credits} credits, Instructor: {instructorName}";
        }
    }
}