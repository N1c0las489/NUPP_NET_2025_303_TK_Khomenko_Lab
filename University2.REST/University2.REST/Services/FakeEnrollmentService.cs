using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University2.REST.Interfaces;
using University2.REST.Models;

namespace University2.REST.Services
{
    public class FakeEnrollmentService : ICrudServiceAsync<EnrollmentModel>
    {
        private readonly ConcurrentDictionary<int, EnrollmentModel> _enrollments = new();
        private readonly FakeStudentService _studentService;
        private readonly FakeCourseService _courseService;

        public FakeEnrollmentService(FakeStudentService studentService, FakeCourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;

            // Тестові дані
            var students = _studentService.ReadAllAsync().Result.ToList();
            var courses = _courseService.ReadAllAsync().Result.ToList();

            if (students.Any() && courses.Any())
            {
                var enrollment1 = new EnrollmentModel
                {
                    Id = 1,
                    StudentId = students[0].Id,
                    CourseId = courses[0].Id,
                    Grade = Grade.A
                };

                var enrollment2 = new EnrollmentModel
                {
                    Id = 2,
                    StudentId = students[1].Id,
                    CourseId = courses[1].Id,
                    Grade = Grade.B
                };

                _enrollments.TryAdd(enrollment1.Id, enrollment1);
                _enrollments.TryAdd(enrollment2.Id, enrollment2);
            }
        }

        public Task<bool> CreateAsync(EnrollmentModel enrollment)
        {
            if (enrollment.Id == 0)
                enrollment.Id = _enrollments.Count + 1;

            return Task.FromResult(_enrollments.TryAdd(enrollment.Id, enrollment));
        }

        public Task<EnrollmentModel> ReadAsync(int id)
        {
            _enrollments.TryGetValue(id, out var enrollment);
            return Task.FromResult(enrollment);
        }

        public Task<IEnumerable<EnrollmentModel>> ReadAllAsync()
        {
            return Task.FromResult(_enrollments.Values.AsEnumerable());
        }

        public Task<IEnumerable<EnrollmentModel>> ReadAllAsync(int page, int amount)
        {
            return Task.FromResult(_enrollments.Values
                .Skip((page - 1) * amount)
                .Take(amount));
        }

        public Task<bool> UpdateAsync(EnrollmentModel enrollment)
        {
            if (_enrollments.ContainsKey(enrollment.Id))
            {
                _enrollments[enrollment.Id] = enrollment;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(EnrollmentModel enrollment)
        {
            return Task.FromResult(_enrollments.TryRemove(enrollment.Id, out _));
        }

        public Task<bool> SaveAsync() => Task.FromResult(true);

        // Не використовується для записів (ID - int)
        public Task<EnrollmentModel> ReadAsync(Guid id) =>
            throw new NotSupportedException("Use ReadAsync(int id) for enrollments");

        
    }
}