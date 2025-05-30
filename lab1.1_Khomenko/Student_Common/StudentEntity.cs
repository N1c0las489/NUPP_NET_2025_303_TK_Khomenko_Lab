using System;

namespace Student.Common
{
    // Делегат для подій
    public delegate void GradeChangedHandler(StudentEntity student, double newGrade);

    public class StudentEntity : Person
    {
        public string StudentNumber { get; set; }
        public string Faculty { get; set; }
        public int Course { get; set; }
        public double AverageGrade { get; set; }

        // Статичне поле
        private static int _totalStudents = 0;

        // Статичний конструктор
        static StudentEntity()
        {
            _totalStudents = 0;
        }

        // Конструктор
        public StudentEntity() : base()
        {
            _totalStudents++;
            StudentNumber = GenerateStudentNumber();
        }

        // Конструктор з параметрами
        public StudentEntity(string firstName, string lastName, DateTime birthDate,
                           string faculty, int course) : base(firstName, lastName, birthDate)
        {
            _totalStudents++;
            Faculty = faculty;
            Course = course;
            StudentNumber = GenerateStudentNumber();
        }

        // Події
        public event GradeChangedHandler GradeChanged;

        // Статичний метод
        public static int GetTotalStudentsCount()
        {
            return _totalStudents;
        }

        // Метод для оновлення оцінки
        public void UpdateGrade(double newGrade)
        {
            if (newGrade < 0 || newGrade > 100)
                throw new ArgumentException("Grade must be between 0 and 100");

            AverageGrade = newGrade;

            // Виклик події
            GradeChanged?.Invoke(this, newGrade);
        }

        // Переосмислений метод з базового класу
        public override string GetFullName()
        {
            return $"Student: {base.GetFullName()} ({StudentNumber})";
        }

        // Приватний метод для генерації номеру студента
        private string GenerateStudentNumber()
        {
            return $"ST{DateTime.Now.Year}{_totalStudents:D4}";
        }

        public override string ToString()
        {
            return $"{GetFullName()}, Faculty: {Faculty}, Course: {Course}, Grade: {AverageGrade:F2}";
        }
    }
}