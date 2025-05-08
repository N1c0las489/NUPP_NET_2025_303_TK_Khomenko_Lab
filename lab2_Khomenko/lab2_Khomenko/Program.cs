using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

public interface ICrudServiceAsync<T> : IEnumerable<T>
{
    Task<bool> CreateAsync(T element);
    Task<T> ReadAsync(Guid id);
    Task<IEnumerable<T>> ReadAllAsync();
    Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
    Task<bool> UpdateAsync(T element);
    Task<bool> RemoveAsync(T element);
    Task<bool> SaveAsync();
}

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : IIdentifiable
{
    private readonly ConcurrentDictionary<Guid, T> _storage = new();
    private readonly string _filePath;
    private readonly object _fileLock = new();

    public CrudServiceAsync(string filePath)
    {
        _filePath = filePath;
        LoadFromFileAsync().Wait();
    }

    public async Task<bool> CreateAsync(T element)
    {
        if (element == null || element.Id == Guid.Empty)
            return false;

        return _storage.TryAdd(element.Id, element) && await SaveAsync();
    }

    public Task<T> ReadAsync(Guid id)
    {
        _storage.TryGetValue(id, out var element);
        return Task.FromResult(element);
    }

    public Task<IEnumerable<T>> ReadAllAsync()
    {
        return Task.FromResult(_storage.Values.AsEnumerable());
    }

    public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        if (page < 1 || amount < 1)
            return Task.FromResult(Enumerable.Empty<T>());

        return Task.FromResult(_storage.Values
            .Skip((page - 1) * amount)
            .Take(amount));
    }

    public async Task<bool> UpdateAsync(T element)
    {
        if (element == null || !_storage.ContainsKey(element.Id))
            return false;

        _storage[element.Id] = element;
        return await SaveAsync();
    }

    public async Task<bool> RemoveAsync(T element)
    {
        if (element == null)
            return false;

        return _storage.TryRemove(element.Id, out _) && await SaveAsync();
    }

    public Task<bool> SaveAsync()
    {
        try
        {
            lock (_fileLock)
            {
                var json = JsonSerializer.Serialize(_storage.Values);
                File.WriteAllText(_filePath, json);
            }
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private async Task LoadFromFileAsync()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            string json;
            lock (_fileLock)
            {
                json = File.ReadAllText(_filePath);
            }

            var elements = JsonSerializer.Deserialize<List<T>>(json);
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    _storage.TryAdd(element.Id, element);
                }
            }
        }
        catch
        {
            // Файл пошкоджений або не може бути прочитаний = продовжує з порожньою колекцією
        }
    }

    public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

public interface IIdentifiable
{
    Guid Id { get; set; }
}

//Завдання2

public class Student : IIdentifiable
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public double AverageGrade { get; set; }
    public int Course { get; set; }
    public string Faculty { get; set; }

    private static readonly Random _random = new();
    private static readonly string[] _firstNames = { "Іван", "Олександр", "Марія", "Ольга", "Андрій", "Наталія" };
    private static readonly string[] _lastNames = { "Петренко", "Іваненко", "Сидоренко", "Коваленко", "Павленко", "Шевченко" };
    private static readonly string[] _faculties = { "ФІОТ", "ФПМ", "ФЛ", "ФБМІ", "ФСП" };

    public static Student CreateNew()
    {
        var birthDate = DateTime.Now.AddYears(-_random.Next(17, 25)).AddMonths(-_random.Next(0, 12));

        return new Student
        {
            Id = Guid.NewGuid(),
            FirstName = _firstNames[_random.Next(_firstNames.Length)],
            LastName = _lastNames[_random.Next(_lastNames.Length)],
            BirthDate = birthDate,
            AverageGrade = Math.Round(_random.NextDouble() * 3 + 2, 2),
            Course = _random.Next(1, 6),
            Faculty = _faculties[_random.Next(_faculties.Length)]
        };
    }
}

//Завдання 3
class Program
{
    static async Task Main(string[] args)
    {
        var service = new CrudServiceAsync<Student>("students.json");

        var tasks = Enumerable.Range(0, 1000)
            .Select(_ => Task.Run(() => service.CreateAsync(Student.CreateNew())))
            .ToArray();

        await Task.WhenAll(tasks);
        Console.WriteLine($"Створено {tasks.Length} студентів.");

        var allStudents = (await service.ReadAllAsync()).ToList();

        if (allStudents.Any())
        {
            var minAge = DateTime.Now.Year - allStudents.Max(s => s.BirthDate).Year;
            var maxAge = DateTime.Now.Year - allStudents.Min(s => s.BirthDate).Year;
            var avgAge = DateTime.Now.Year - allStudents.Average(s => s.BirthDate.Year);

            var minGrade = allStudents.Min(s => s.AverageGrade);
            var maxGrade = allStudents.Max(s => s.AverageGrade);
            var avgGrade = allStudents.Average(s => s.AverageGrade);

            Console.WriteLine("\nАналіз віку студентів:");
            Console.WriteLine($"Мінімальний: {minAge}, Максимальний: {maxAge}, Середній: {avgAge:F1}");

            Console.WriteLine("\nАналіз успішності:");
            Console.WriteLine($"Мінімальний середній бал: {minGrade:F2}");
            Console.WriteLine($"Максимальний середній бал: {maxGrade:F2}");
            Console.WriteLine($"Середній бал по всіх студентах: {avgGrade:F2}");

            Console.WriteLine("\nРозподіл за курсами:");
            var coursesGroup = allStudents.GroupBy(s => s.Course)
                                        .OrderBy(g => g.Key);

            foreach (var group in coursesGroup)
            {
                Console.WriteLine($"{group.Key} курс: {group.Count()} студентів");
            }

            Console.WriteLine("\nРозподіл за факультетами:");
            var facultiesGroup = allStudents.GroupBy(s => s.Faculty)
                                          .OrderByDescending(g => g.Count());

            foreach (var group in facultiesGroup)
            {
                Console.WriteLine($"{group.Key}: {group.Count()} студентів");
            }
        }

        await service.SaveAsync();
        Console.WriteLine("\nДані збережено у файл.");
    }
}