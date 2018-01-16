using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RV.Test.Infra.Repositories
{
    public interface IRepository<T> where  T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        void Update(T entity);
        Task<ICollection<T>> GetWhereAsync(Expression<Func<T, bool>> func);
        Task<ICollection<T>> GetAllAsync();
        Task SaveAsync();
    }
}
