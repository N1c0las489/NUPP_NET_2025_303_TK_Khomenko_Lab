using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University2.REST.Models;
using University2.REST.Interfaces;

namespace University2.REST.Services
{
    public class FakeStudentService : ICrudServiceAsync<StudentModel>
    {
        private readonly ConcurrentDictionary<Guid, StudentModel> _students = new();
        private readonly FakeStudentAddressService _addressService;

        public FakeStudentService(FakeStudentAddressService addressService)
        {
            _addressService = addressService;

            // Тестові дані
            var student1 = new StudentModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Іван",
                LastName = "Петренко",
                BirthDate = new DateTime(2000, 5, 10),
                Address = new StudentAddressModel { Id = 1, Address = "вул. Центральна, 10", City = "Київ", Country = "Україна" }
            };

            var student2 = new StudentModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Марія",
                LastName = "Іваненко",
                BirthDate = new DateTime(2001, 8, 15),
                Address = new StudentAddressModel { Id = 2, Address = "пр. Перемоги, 25", City = "Львів", Country = "Україна" }
            };

            _students.TryAdd(student1.Id, student1);
            _students.TryAdd(student2.Id, student2);
            _addressService.CreateAsync(student1.Address);
            _addressService.CreateAsync(student2.Address);
        }

        public Task<bool> CreateAsync(StudentModel student)
        {
            if (student.Id == Guid.Empty)
                student.Id = Guid.NewGuid();

            if (student.Address != null)
                _addressService.CreateAsync(student.Address);

            return Task.FromResult(_students.TryAdd(student.Id, student));
        }

        public Task<StudentModel> ReadAsync(Guid id)
        {
            _students.TryGetValue(id, out var student);
            return Task.FromResult(student);
        }

        public Task<IEnumerable<StudentModel>> ReadAllAsync()
        {
            return Task.FromResult(_students.Values.AsEnumerable());
        }

        public Task<IEnumerable<StudentModel>> ReadAllAsync(int page, int amount)
        {
            return Task.FromResult(_students.Values
                .Skip((page - 1) * amount)
                .Take(amount));
        }

        public Task<bool> UpdateAsync(StudentModel student)
        {
            if (_students.ContainsKey(student.Id))
            {
                _students[student.Id] = student;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(StudentModel student)
        {
            if (student.Address != null)
                _addressService.RemoveAsync(student.Address);

            return Task.FromResult(_students.TryRemove(student.Id, out _));
        }

        public Task<bool> SaveAsync() => Task.FromResult(true);

        // Не використовується для студентів (ID - Guid)
        public Task<StudentModel> ReadAsync(int id) =>
            throw new NotSupportedException("Use ReadAsync(Guid id) for students");

      
    }
}