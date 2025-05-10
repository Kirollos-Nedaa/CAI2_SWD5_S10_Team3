using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechXpress.Infrastructure.Contexts;

namespace TechXpress.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity, Action<string> LogAction)
        {
            LogAction?.Invoke($"Adding {typeof(T).Name} to database");
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, Action<string> LogAction)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is not null)
            {
                LogAction?.Invoke($"Deleting {typeof(T).Name} from database!");
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity, Action<string> LogAction)
        {
            LogAction?.Invoke($"Updating {typeof(T).Name} in database");
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
