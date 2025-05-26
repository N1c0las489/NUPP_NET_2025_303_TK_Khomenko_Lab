using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Models;

namespace University.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UniversityContext _context;
        protected readonly DbSet<T> _dbSet;
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<CourseModel> Courses { get; set; }
        public Repository(UniversityContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
