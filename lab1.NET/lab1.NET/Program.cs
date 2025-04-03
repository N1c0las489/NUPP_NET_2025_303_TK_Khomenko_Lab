using System;

// Базовий клас Person
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine($"Name: {Name}, Age: {Age}");
    }
}

// Похідний клас Student
class Student : Person
{
    public string University { get; set; }

    public Student(string name, int age, string university) : base(name, age)
    {
        University = university;
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($"University: {University}");
    }
}

// Похідний клас Teacher
class Teacher : Person
{
    public string Subject { get; set; }

    public Teacher(string name, int age, string subject) : base(name, age)
    {
        Subject = subject;
    }

    public override void ShowInfo()
    {
        base.ShowInfo();
        Console.WriteLine($"Subject: {Subject}");
    }
}

class Program
{
    static void Main()
    {
        Person person = new Person("Ivan", 30);
        Student student = new Student("Maria", 20, "KNU");
        Teacher teacher = new Teacher("Oleg", 45, "Mathematics");

        person.ShowInfo();
        Console.WriteLine();
        student.ShowInfo();
        Console.WriteLine();
        teacher.ShowInfo();
    }
}
