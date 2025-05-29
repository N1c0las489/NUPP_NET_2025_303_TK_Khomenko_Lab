using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University2.REST.Interfaces
{
    public interface ICrudServiceAsync<T>
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(int id);  // Для CourseModel (int Id)
        Task<T> ReadAsync(Guid id); // Для StudentModel (Guid Id)
        Task<IEnumerable<T>> ReadAllAsync();
        Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }
}