using System;

namespace Student.Common
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        // Конструктор
        public Person()
        {
            Id = Guid.NewGuid();
        }

        // Конструктор з параметрами
        public Person(string firstName, string lastName, DateTime birthDate) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        // Метод
        public virtual string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        // Метод для обчислення віку
        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}