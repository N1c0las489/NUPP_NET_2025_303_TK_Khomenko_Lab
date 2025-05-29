using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University2.REST.Interfaces;
using University2.REST.Models;

namespace University2.REST.Services
{
    public class FakeCourseService : ICrudServiceAsync<CourseModel>
    {
        private readonly ConcurrentDictionary<int, CourseModel> _courses = new();

        public FakeCourseService()
        {
            // Тестові дані
            var course1 = new CourseModel { Id = 1, Title = "Програмування", Credits = 5 };
            var course2 = new CourseModel { Id = 2, Title = "Математика", Credits = 4 };
            var course3 = new CourseModel { Id = 3, Title = "Фізика", Credits = 3 };

            _courses.TryAdd(course1.Id, course1);
            _courses.TryAdd(course2.Id, course2);
            _courses.TryAdd(course3.Id, course3);
        }

        public Task<bool> CreateAsync(CourseModel course)
        {
            if (course.Id == 0)
                course.Id = _courses.Count + 1;

            return Task.FromResult(_courses.TryAdd(course.Id, course));
        }

        public Task<CourseModel> ReadAsync(int id)
        {
            _courses.TryGetValue(id, out var course);
            return Task.FromResult(course);
        }

        public Task<IEnumerable<CourseModel>> ReadAllAsync()
        {
            return Task.FromResult(_courses.Values.AsEnumerable());
        }

        public Task<IEnumerable<CourseModel>> ReadAllAsync(int page, int amount)
        {
            return Task.FromResult(_courses.Values
                .Skip((page - 1) * amount)
                .Take(amount));
        }

        public Task<bool> UpdateAsync(CourseModel course)
        {
            if (_courses.ContainsKey(course.Id))
            {
                _courses[course.Id] = course;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(CourseModel course)
        {
            return Task.FromResult(_courses.TryRemove(course.Id, out _));
        }

        public Task<bool> SaveAsync() => Task.FromResult(true);

        // Не використовується для курсів (ID - int)
        public Task<CourseModel> ReadAsync(Guid id) =>
            throw new NotSupportedException("Use ReadAsync(int id) for courses");

      
    }
}