using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProyectoGatitos.Repositories
{
    public interface IEntityBaseRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        int Count();
        void SaveChanges();
        Task SaveChangesAsync();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void AddRange(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate); 
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}