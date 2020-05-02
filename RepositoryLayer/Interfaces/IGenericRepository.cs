using InfrastructureLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IGenericRepository<T, PKey> where T : IEntity<PKey>
    {
        IUnitOfWork Context { get; /*set;*/ }
        IQueryable<T> GetAll(bool asNoTracking = true, params string[] includePathEnities);
        Task<ICollection<T>> GetAllAsync(bool asNoTracking = true);
        T Get(PKey id);
        Task<T> GetAsync(PKey id);
        T GetByCondition(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities);
        Task<T> GetByConditionAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities);
        IQueryable<T> SelectBy(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities);
        IQueryable<TResult> SelectBy<TResult>(Expression<Func<T, TResult>> selector, bool asNoTracking = true, params string[] includePathEnities) where TResult : class;
        IQueryable<TResult> SelectBy<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, bool asNoTracking = true, params string[] includePathEnities) where TResult : class;
        Task<ICollection<T>> SelectByAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, params string[] includePathEnities);
        bool Add(T entity, bool autoSave = true);
        void AddAsync(T entity);
        bool Update(T entity, bool autoSave = true);
        void UpdateAsync(T entity);
        bool Delete(T entity, bool autoSave = true);
        void DeleteAsync(T entity);
        bool DeleteById(PKey id, bool autoSave = true);
        void DeleteByIdAsync(PKey id);
        ColumnType GetColumnValue<ColumnType>(Expression<Func<T, bool>> predicate, string columnName = "Id");
        //Dictionary<Enums.EntityKeys, PKey> CheckRelatedEntitiesToThisEntity(PKey id);
        int Commit();
    }

}
