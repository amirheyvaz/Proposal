using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<T> Set<T>() where T : class;
        DbSet Set(Type entityType);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        DbContextConfiguration Configuration { get; }
        Database Database { get; }
    }
}
