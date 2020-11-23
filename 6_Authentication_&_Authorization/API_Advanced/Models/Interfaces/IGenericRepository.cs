using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Advanced.Models.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        IQueryable<T> GetQueryable();

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAll();


    }
}