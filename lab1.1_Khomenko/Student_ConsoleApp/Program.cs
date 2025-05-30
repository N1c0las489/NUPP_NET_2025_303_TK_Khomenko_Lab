using System;
using System.Linq;
using Student.Common;

namespace Student.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("=== Демонстрація роботи CRUD сервісу ===\n");

            // Створення CRUD сервісів для різних типів
            var studentService = new CrudService<StudentEntity>(s => s.Id);
            var teacherService = new CrudService<Teacher>(t => t.Id);
            var courseService = new CrudService<Course>(c => c.Id);

            // Демонстрація роботи зі студентами
            DemonstrateStudentOperations(studentService);

            System.Console.WriteLine("\n" + new string('=', 50) + "\n");

            // Демонстрація роботи з викладачами
            DemonstrateTeacherOperations(teacherService);

            System.Console.WriteLine("\n" + new string('=', 50) + "\n");

            // Демонстрація роботи з курсами
            DemonstrateCourseOperations(courseService, teacherService);

            System.Console.WriteLine("\n=== Демонстрація завершена ===");
            System.Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            System.Console.ReadKey();
        }

        static void DemonstrateStudentOperations(CrudService<StudentEntity> service)
        {
            System.Console.WriteLine("--- Операції зі студентами ---");

            // Створення студентів
            var student1 = new StudentEntity("Іван", "Петренко", new DateTime(2000, 5, 15), "Комп'ютерні науки", 2);
            var student2 = new StudentEntity("Марія", "Іваненко", new DateTime(1999, 8, 22), "Математика", 3);
            var student3 = new StudentEntity("Олександр", "Сидоренко", new DateTime(2001, 2, 10), "Фізика", 1);

            // Підписка на події зміни оцінок
            student1.GradeChanged += OnGradeChanged;
            student2.GradeChanged += OnGradeChanged;
            student3.GradeChanged += OnGradeChanged;

            // Додавання студентів до сервісу
            service.Create(student1);
            service.Create(student2);
            service.Create(student3);

            System.Console.WriteLine($"Створено {service.Count} студентів");
            System.Console.WriteLine($"Загальна кількість студентів: {StudentEntity.GetTotalStudentsCount()}");

            // Оновлення оцінок (демонстрація подій)
            student1.UpdateGrade(92.5);
            student2.UpdateGrade(78.0);
            student3.UpdateGrade(85.5);

            // Виведення всіх студентів
            System.Console.WriteLine("\nСписок всіх студентів:");
            foreach (var student in service.ReadAll())
            {
                System.Console.WriteLine($"- {student}");
                System.Console.WriteLine($"  Статус: {student.GetAcademicStatus()}"); // Метод розширення
                System.Console.WriteLine($"  Відмінник: {(student.IsExcellentStudent() ? "Так" : "Ні")}"); // Метод розширення
                System.Console.WriteLine($"  Років до випуску: {student.GetYearsUntilGraduation()}"); // Метод розширення
            }

            // Читання конкретного студента
            System.Console.WriteLine($"\nПошук студента з ID {student2.Id}:");
            var foundStudent = service.Read(student2.Id);
            if (foundStudent != null)
            {
                System.Console.WriteLine($"Знайдено: {foundStudent}");
            }

            // Оновлення студента
            student1.Course = 3;
            student1.UpdateGrade(95.0);
            service.Update(student1);
            System.Console.WriteLine($"\nОновлено дані студента: {student1}");

            // Видалення студента
            service.Remove(student3);
            System.Console.WriteLine($"\nВидалено студента. Залишилось: {service.Count} студентів");
        }

        static void DemonstrateTeacherOperations(CrudService<Teacher> service)
        {
            System.Console.WriteLine("--- Операції з викладачами ---");

            // Створення викладачів
            var teacher1 = new Teacher("Володимир", "Коваленко", new DateTime(1975, 3, 20),
                                     "Комп'ютерні науки", "Професор", 75000);
            var teacher2 = new Teacher("Ольга", "Морозова", new DateTime(1980, 11, 8),
                                     "Математика", "Доцент", 55000);

            // Додавання предметів
            teacher1.AddSubject("Програмування");
            teacher1.AddSubject("Алгоритми");
            teacher1.AddSubject("Структури даних");

            teacher2.AddSubject("Вища математика");
            teacher2.AddSubject("Статистика");

            // Додавання до сервісу
            service.Create(teacher1);
            service.Create(teacher2);

            System.Console.WriteLine($"Створено {service.Count} викладачів");

            // Виведення всіх викладачів
            System.Console.WriteLine("\nСписок всіх викладачів:");
            foreach (var teacher in service.ReadAll())
            {
                System.Console.WriteLine($"- {teacher}");
                System.Console.WriteLine($"  Високо кваліфікований: {(teacher.IsHighlyQualified() ? "Так" : "Ні")}"); // Метод розширення
                System.Console.WriteLine($"  Рівень досвіду: {teacher.GetExperienceLevel()}"); // Метод розширення
            }

            // Оновлення викладача
            teacher2.AddSubject("Дискретна математика");
            teacher2.Salary = 60000;
            service.Update(teacher2);
            System.Console.WriteLine($"\nОновлено дані викладача: {teacher2}");
        }

        static void DemonstrateCourseOperations(CrudService<Course> service, CrudService<Teacher> teacherService)
        {
            System.Console.WriteLine("--- Операції з курсами ---");

            // Отримання викладача для призначення
            var teacher = teacherService.ReadAll().FirstOrDefault();

            // Створення курсів
            var course1 = new Course("Основи програмування", "CS101", 4);
            var course2 = new Course("Вища математика", "MATH201", 5);
            var course3 = new Course("Фізика", "PHYS101", 3);

            // Призначення викладача
            if (teacher != null)
            {
                course1.Instructor = teacher;
            }

            // Додавання до сервісу
            service.Create(course1);
            service.Create(course2);
            service.Create(course3);

            System.Console.WriteLine($"Створено {service.Count} курсів");
            System.Console.WriteLine($"Загальна кількість курсів: {Course.GetTotalCoursesCount()}");

            // Виведення всіх курсів
            System.Console.WriteLine("\nСписок всіх курсів:");
            foreach (var course in service.ReadAll())
            {
                System.Console.WriteLine($"- {course}");
                System.Console.WriteLine($"  Валідний: {(course.IsValid() ? "Так" : "Ні")}");
            }

            // Видалення курсу
            service.Remove(course3);
            System.Console.WriteLine($"\nВидалено курс. Залишилось: {service.Count} курсів");
        }

        // Обробник події зміни оцінки
        static void OnGradeChanged(StudentEntity student, double newGrade)
        {
            System.Console.WriteLine($"[ПОДІЯ] Оцінка студента {student.GetFullName()} змінена на {newGrade:F1}");
        }
    }
}