using InfrastructureLayer.AbstractModels;
using InfrastructureLayer.Interfaces;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories
{
    public abstract class GenericRepository<T, PKey> : IDisposable, IGenericRepository<T, PKey>
    where T : TEntity<PKey>
    {
        private readonly IUnitOfWork _unitOfWork;
        //public GenericRepository()
        //{

        //}

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(typeof(IUnitOfWork).Name, @"Unit of work cannot be null");
            }

            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
        }

        public IUnitOfWork Context
        {
            get { return _unitOfWork; }
            //set { _unitOfWork = value; }
        }

        public virtual T Get(PKey id)
        {
            var entity = _unitOfWork.Set<T>().Find(id);
            //var state = _context.Entry(entity).State;
            //_context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async virtual Task<T> GetAsync(PKey id)
        {
            var entity = _unitOfWork.Set<T>().FindAsync(id);
            //var state = _context.Entry(entity).State;
            //_context.Entry(entity).State = EntityState.Detached;
            return await entity;
        }

        /// <summary>
        /// این متد یک موجودیت را با استفاده از شناسه آن پیدا کرده و اجازه نمی دهد که انتیتی فریمورک آن را تراک کند-NoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includePathEnities"></param>
        /// <returns></returns>
        public virtual T GetByCondition(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities)
        {
            //_db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var entities = _unitOfWork.Set<T>();
                foreach (string includePath in includePathEnities)
                    entities.Include(includePath);

                T entity = default(T);
                if (asNoTracking)
                    entity = entities.AsNoTracking().FirstOrDefault(predicate);
                else
                    entity = entities.FirstOrDefault(predicate);

                if (entity != null)
                {
                    if (asNoTracking)
                    {
                        var state = _unitOfWork.Entry(entity).State;
                        _unitOfWork.Entry(entity).State = EntityState.Detached;
                    }
                    return entity;
                }

                return null;

            }
            catch (Exception exp)
            {
                return null;
            }
        }

        /// <summary>
        /// این متد یک موجودیت را با استفاده از شناسه آن پیدا کرده و اجازه نمی دهد که انتیتی فریمورک آن را تراک کند-NoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includePathEnities"></param>
        /// <returns></returns>
        public async virtual Task<T> GetByConditionAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities)
        {
            try
            {
                var entities = _unitOfWork.Set<T>();
                foreach (string includePath in includePathEnities)
                    entities.Include(includePath);

                Task<T> entity = null;
                if (asNoTracking)
                    entity = entities.AsNoTracking().FirstOrDefaultAsync(predicate);
                else
                    entity = entities.FirstOrDefaultAsync(predicate);

                if (entity != null)
                {
                    if (asNoTracking)
                    {
                        var state = _unitOfWork.Entry(entity).State;
                        _unitOfWork.Entry(entity).State = EntityState.Detached;
                    }
                    return await entity;
                }

                return default(T);
            }
            catch (Exception exp)
            {
                return default(T);
            }
        }

        public virtual IQueryable<T> GetAll(bool asNoTracking = true, params string[] includePathEnities)
        {
            IQueryable<T> query = _unitOfWork.Set<T>();

            foreach (string includePath in includePathEnities)
                query.Include(includePath);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        public async Task<ICollection<T>> GetAllAsync(bool asNoTracking = true)
        {
            IQueryable<T> query = _unitOfWork.Set<T>();
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public IQueryable<T> SelectBy(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities)
        {
            IQueryable<T> query = _unitOfWork.Set<T>();

            foreach (string includePath in includePathEnities)
                query.Include(includePath);

            if (asNoTracking)
                return query.AsNoTracking().Where(predicate);

            return query.Where(predicate);
        }

        public IQueryable<TResult> SelectBy<TResult>(Expression<Func<T, TResult>> selector, bool asNoTracking = true, params string[] includePathEnities) where TResult : class
        {
            IQueryable<T> query = _unitOfWork.Set<T>();

            foreach (string includePath in includePathEnities)
                query.Include(includePath);

            if (asNoTracking)
                return query.AsNoTracking().Select<T, TResult>(selector);

            return query.Select<T, TResult>(selector);
        }

        public IQueryable<TResult> SelectBy<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, bool asNoTracking = true, params string[] includePathEnities) where TResult : class
        {
            IQueryable<T> query = _unitOfWork.Set<T>();

            foreach (string includePath in includePathEnities)
                query.Include(includePath);

            if (asNoTracking)
                return query.AsNoTracking().Where(predicate).Select<T, TResult>(selector);

            return query.Where(predicate).Select<T, TResult>(selector);
        }

        public async Task<ICollection<T>> SelectByAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities)
        {
            IQueryable<T> query = _unitOfWork.Set<T>();

            foreach (string includePath in includePathEnities)
                query.Include(includePath);

            if (asNoTracking)
                return await query.AsNoTracking().Where(predicate).ToListAsync();

            return await query.Where(predicate).ToListAsync();
        }

        public virtual bool Add(T entity, bool autoSave = true)
        {
            _unitOfWork.Set<T>().Add(entity);
            if (autoSave)
                return Convert.ToBoolean(Commit());

            return true;
        }

        public async virtual void AddAsync(T entity)
        {
            _unitOfWork.Set<T>().Add(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual bool Update(T entity, bool autoSave = true)
        {
            try
            {
                //var typeName = typeof(PKey).FullName;
                //var local = _context.Set<T>().Local.FirstOrDefault(f => (int)Utilities.ConvertToType(f.Id, typeName) == (int)Utilities.ConvertToType(entity.Id, typeName));
                var local = _unitOfWork.Set<T>().Local.FirstOrDefault(f => (int)((object)f.ID) == (int)((object)entity.ID));
                if (local != null)
                    _unitOfWork.Entry(local).State = EntityState.Detached;

                _unitOfWork.Entry(entity).State = EntityState.Modified;

                if (autoSave)
                    return Convert.ToBoolean(Commit());

                return true;
            }
            catch (Exception exp)
            {
                //return false;
                var ctx = ((IObjectContextAdapter)_unitOfWork).ObjectContext;
                ctx.Refresh(RefreshMode.ClientWins, _unitOfWork.Set<T>());
                return Convert.ToBoolean(ctx.SaveChanges());
            }
        }

        public async virtual void UpdateAsync(T entity)
        {
            _unitOfWork.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual bool Delete(T entity, bool autoSave = true)
        {
            try
            {
                _unitOfWork.Set<T>().Remove(entity);
                if (autoSave)
                    Commit();

                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public virtual bool DeleteRange(List<T> entities, bool autoSave = true)
        {
            try
            {

                foreach(var entity in entities)
                {
                    _unitOfWork.Set<T>().Remove(entity);
                }

                if (autoSave)
                    Commit();

                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public virtual async void DeleteAsync(T entity)
        {
            _unitOfWork.Set<T>().Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual bool DeleteById(PKey id, bool autoSave = true)
        {
            T entity = Get(id);
            if (entity != null)
            {
                return Delete(entity, autoSave);
            }

            return false;
        }

        public virtual async void DeleteByIdAsync(PKey id)
        {
            T entity = Get(id);
            if (entity != null)
            {
                _unitOfWork.Set<T>().Remove(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public ColumnType GetColumnValue<ColumnType>(Expression<Func<T, bool>> predicate, string columnName = "Id")
        {
            try
            {
                if (_unitOfWork.Set<T>().Any())
                {
                    Type entityType = typeof(T);
                    if (entityType.GetProperty(columnName) != null)
                    {
                        T entity = _unitOfWork.Set<T>().Where(predicate).FirstOrDefault();
                        if (entity != null)
                        {
                            var proprtyInfo = entityType.GetProperty(columnName);
                            //if (Utilities.IsNullableType(proprtyInfo.GetType()))
                            //{
                            //    ColumnType date = (ColumnType)proprtyInfo.GetValue(entity, null);
                            //}
                            //else
                            return (ColumnType)proprtyInfo.GetValue(entity, null);
                        }

                        return default(ColumnType);
                    }
                    else
                        return default(ColumnType);
                }
                else
                    return default(ColumnType);
            }
            catch (Exception exp)
            {
                return default(ColumnType);
            }
        }

       
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public virtual int Commit()
        {
            return (_unitOfWork.SaveChanges());
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unitOfWork != null)
                {
                    _unitOfWork.Dispose();
                    //_unitOfWork = null;
                }
            }
        }
        ~GenericRepository()
        {
            Dispose(false);
        }
    }

}
