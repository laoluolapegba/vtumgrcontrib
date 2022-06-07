using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Data
{
    public interface IRepository<T>
    {

        T Add(T obj);

        Task<T> AddAsync(T obj);

        void AddRange(IEnumerable<T> records);

        Task AddRangeAsync(IEnumerable<T> records);

        long Count(Expression<Func<T, bool>> predicate = null);

        Task<long> CountAsync(Expression<Func<T, bool>> predicate = null);

        bool Delete(Expression<Func<T, bool>> predicate);

        bool Delete(T obj);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(T obj);

        bool DeleteById(object id);

        Task DeleteByIdAsync(object id);

        bool DeleteRange(Expression<Func<T, bool>> predicate);

        bool DeleteRange(IEnumerable<T> records);

        Task DeleteRangeAsync(Expression<Func<T, bool>> predicate);

        Task DeleteRangeAsync(IEnumerable<T> records);

        void Dispose();

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params string[] includeProperties);

        Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params string[] includeProperties);

        T GetById(object id);

        Task<T> GetByIdAsync(object id);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);

        bool Any(Expression<Func<T, bool>> predicate = null);

        IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null);

        T GetSingleBy(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleByAsync(Expression<Func<T, bool>> predicate);

        int Save();

        Task<int> SaveAsync();

        T Update(T obj);

        Task<T> UpdateAsync(T obj);
    }
}
