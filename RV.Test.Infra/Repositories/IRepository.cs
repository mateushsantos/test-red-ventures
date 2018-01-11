using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RV.Test.Infra.Repositories
{
    public interface IRepository<T> where  T : class
    {
        Task<T> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        void Update(T entity);
        Task<ICollection<T>> GetWhereAsync(Func<T, bool> func);
        Task<ICollection<T>> GetAllAsync();
        Task SaveAsync();
    }
}
