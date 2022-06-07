using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Chams.Vtumanager.Provisioning.Data
{
   public class Repository<T> :IRepository<T>  where T : class
    {

        private readonly DbContext _dbContext;

        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        #region AddToRepo
        public virtual T Add(T obj)
        {
            try
            {
                _dbSet.Add(obj);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Adds a list of objects to the repo</summary>
        /// <param name="obj">the object to be added</param>
        public virtual void AddRange(IEnumerable<T> records)
        {
            try
            {
                _dbSet.AddRange(records);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Adds an object and saves changes to the DB</summary>
        /// <param name="obj">the object to be saved</param>
        public virtual async Task<T> AddAsync(T obj)
        {
            Add(obj);
            await SaveAsync();
            return obj;
        }

        /// <summary>Adds a list of objects and saves changes to DB</summary>
        /// <param name="obj">the object to be saved</param>
        public virtual async Task AddRangeAsync(IEnumerable<T> records)
        {
            AddRange(records);
            await SaveAsync();
        }
        #endregion

        #region UpdateRepo
        /// <summary>Updates an object on the repo</summary>
        /// <param name="obj">the object to be updated</param>
        public virtual T Update(T obj)
        {
            try
            {
                _dbSet.Attach(obj);
                _dbContext.Entry<T>(obj).State = EntityState.Modified;
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Updates an object and saves changes to the DB</summary>
        /// <param name="obj">the object to be updated and saved</param>
        public virtual async Task<T> UpdateAsync(T obj)
        {
            Update(obj);
            await SaveAsync();
            return obj;
        }
        #endregion

        #region Delete Repo

        /// <summary>Deletes an object from the repo</summary>
        /// <param name="obj">the object to be deleted</param>
        public virtual bool Delete(T obj)
        {
            try
            {
                _dbSet.Remove(obj);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Deletes an object from the repo and saves changes to DB</summary>
        /// <param name="obj">the object to be deleted</param>
        public virtual async Task DeleteAsync(T obj)
        {
            Delete(obj);
            await SaveAsync();
        }

        /// <summary>Deletes an object from the repo that meets certain condition</summary>
        /// <param name="predicate">the required condition</param>
        public virtual bool Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var obj = GetSingleBy(predicate);
                if (obj != null)
                {
                    _dbSet.Remove(obj);
                    return true;
                }
                else
                    throw new Exception($"object does not exist");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Deletes an object from the repo that meets certain condition and saves changes to DB</summary>
        /// <param name="predicate">the required condition</param>
        public virtual async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            Delete(predicate);
            await SaveAsync();
        }

        /// <summary>Deletes a list of objects from the repo</summary>
        /// <param name="records">the object to be deleted</param>
        public virtual bool DeleteRange(IEnumerable<T> records)
        {
            try
            {
                _dbSet.RemoveRange(records);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Deletes a list of objects from the repo and saves changes to DB</summary>
        /// <param name="records">the object to be deleted</param>
        public virtual async Task DeleteRangeAsync(IEnumerable<T> records)
        {
            DeleteRange(records);
            await SaveAsync();
        }

        /// <summary>Deletes a list of objects that meet required condition the repo</summary>
        /// <param name="predicate">the condition to be met</param>
        public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
        {
            try
            {
                IEnumerable<T> records = from x in _dbSet.Where<T>(predicate) select x;

                if (records.Count() > 0)
                {
                    _dbSet.RemoveRange(records);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>Deletes a list of objects that meet required condition the repo and saves changes to DB</summary>
        /// <param name="predicate">the condition to be met</param>
        public virtual async Task DeleteRangeAsync(Expression<Func<T, bool>> predicate)
        {
            DeleteRange(predicate);
            await SaveAsync();
        }

        /// <summary>Deletes object with particular id from repo</summary>
        /// <param name="id">the id of the object</param>
        public virtual bool DeleteById(object id)
        {
            try
            {
                var obj = _dbSet.Find(id);
                if (obj != null)
                {
                    _dbSet.Remove(obj);
                    return true;
                }
                else
                    throw new Exception($"object with id {id} does not exist");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>Deletes object with particular id from repo and saves changes to DB</summary>
        /// <param name="id">the id of the object</param>
        public virtual async Task DeleteByIdAsync(object id)
        {
            DeleteById(id);
            await SaveAsync();
        }

        #endregion Delete Repo

        #region Get Repo

        /// <summary>
        /// Gets a single object from the repo that meets required condition
        /// </summary>
        /// <param name="predicate">the required condition</param>
        public virtual T GetSingleBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets a single object asynchronously from the repo that meets required condition
        /// </summary>
        /// <param name="predicate">the required condition</param>
        public virtual async Task<T> GetSingleByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Gets a single object from the repo with a specified id
        /// </summary>
        /// <param name="id">the id of the object</param>
        /// <returns>returns a <paramref name="T"/> object or null if id doesnt exist</returns>
        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Gets a single object asynchronously from the repo with a specified id
        /// </summary>
        /// <param name="id">the id of the object</param>
        /// <returns>returns a <paramref name="T"/> object or null if id doesnt exist</returns>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Gets a queryable object for running database queries
        /// </summary>
        /// <param name="predicate">the where condition for the query</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return _dbSet;
                else
                    return _dbSet.Where(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all object of type <paramref name="T"/> in repository that meet required conditions asynchronosly.
        /// </summary>
        /// <param name="predicate">the where condition</param>
        /// <param name="orderBy">the property to order objects by</param>
        /// <param name="skip">number of objects to skip</param>
        /// <param name="take">maximum number of objects to be returned</param>
        /// <param name="includeProperties">the properties to be included in query</param>
        public virtual IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params string[] includeProperties)
        {
            try
            {
                IQueryable<T> query = ConstructQuery(predicate, orderBy, skip, take, includeProperties);

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all object of type <paramref name="T"/> in repository that meet required conditions asynchronosly.
        /// </summary>
        /// <param name="predicate">the where condition</param>
        /// <param name="orderBy">the property to order objects by</param>
        /// <param name="skip">number of objects to skip</param>
        /// <param name="take">maximum number of objects to be returned</param>
        /// <param name="includeProperties">the properties to be included in query</param>
        public virtual async Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params string[] includeProperties)
        {
            try
            {
                IQueryable<T> query = ConstructQuery(predicate, orderBy, skip, take, includeProperties);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<T> ConstructQuery(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int? skip, int? take, params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            for (int i = 0; i < includeProperties.Length; i++)
            {
                query = query.Include(includeProperties[i]);
            }

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        /// <summary>
        /// Gets all object of type <paramref name="T"/> in repository.
        /// </summary>
        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all object of type <paramref name="T"/> in repository asynchronously.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Get Repo

        #region Any Repo

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return await _dbSet.AnyAsync();
            return await _dbSet.AnyAsync(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return _dbSet.Any();
            return _dbSet.Any(predicate);
        }

        #endregion Any Repo

        #region Count Repo

        /// <summary>
        /// Count the records that meet condition
        /// </summary>
        /// <param name="predicate">required condition</param>
        public virtual long Count(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return _dbSet.LongCount();
                return _dbSet.LongCount(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Count the records that meet condition asynchronously
        /// </summary>
        /// <param name="predicate">required condition</param>
        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await _dbSet.LongCountAsync();
                return await _dbSet.LongCountAsync(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Count Repo

        #region SaveRepo

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        public virtual int Save()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves all changes to the database asynchronously
        /// </summary>
        public virtual Task<int> SaveAsync()
        {
            try
            {
                return _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion SaveRepo

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
