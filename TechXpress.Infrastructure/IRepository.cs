using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechXpress.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity, Action<string> LogAction);
        Task UpdateAsync(T entity, Action<string> LogAction);
        Task DeleteAsync(int id, Action<string> LogAction);
    }
}
