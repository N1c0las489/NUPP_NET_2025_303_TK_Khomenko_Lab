using System;
using System.Collections.Generic;

namespace Student.Common
{
    public class Teacher : Person
    {
        public string Department { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
        public List<string> Subjects { get; set; }

        // Конструктор
        public Teacher() : base()
        {
            Subjects = new List<string>();
        }

        // Конструктор з параметрами
        public Teacher(string firstName, string lastName, DateTime birthDate,
                      string department, string position, double salary) : base(firstName, lastName, birthDate)
        {
            Department = department;
            Position = position;
            Salary = salary;
            Subjects = new List<string>();
        }

        // Метод для додавання предмету
        public void AddSubject(string subject)
        {
            if (!string.IsNullOrWhiteSpace(subject) && !Subjects.Contains(subject))
            {
                Subjects.Add(subject);
            }
        }

        // Метод для видалення предмету
        public bool RemoveSubject(string subject)
        {
            return Subjects.Remove(subject);
        }

        // Переосмислений метод
        public override string GetFullName()
        {
            return $"{Position} {base.GetFullName()}";
        }

        public override string ToString()
        {
            return $"{GetFullName()}, Department: {Department}, Subjects: {string.Join(", ", Subjects)}";
        }
    }
}