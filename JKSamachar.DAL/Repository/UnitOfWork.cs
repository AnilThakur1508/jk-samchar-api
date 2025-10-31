using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKSamachar.DAL.Data;
using JKSamachar.DAL.Enitity;
using JKSamachar.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JKSamachar.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public JKSamacharContext Context { get; private set; }

        public UnitOfWork(JKSamacharContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return new DbTransaction(Context.Database.BeginTransaction(isolationLevel));
        }
        public void Add<T>(T obj)
            where T : class
        {
            if (obj is BaseEntity entity)
            {
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.CreatedOn = DateTime.UtcNow;
                entity.ModifiedOn = DateTime.UtcNow;

            }
            var set = Context.Set<T>();
            set.Add(obj);
        }


        public void Update<T>(T obj)
            where T : class
        {
            if (obj is BaseEntity entity)
            {
                entity.ModifiedOn = DateTime.UtcNow;

            }
            var set = Context.Set<T>();
            set.Attach(obj);
            Context.Entry(obj).State = EntityState.Modified;
        }

        public void DeleteTemporary<T>(T obj)
            where T : class
        {
            if (obj is BaseEntity entity)
            {
                entity.IsDeleted = true;
                entity.ModifiedOn = DateTime.UtcNow;

            }
            var set = Context.Set<T>();
            set.Attach(obj);
            Context.Entry(obj).State = EntityState.Modified;
        }

        void IUnitOfWork.Remove<T>(T obj)
        {
            var set = Context.Set<T>();
            set.Remove(obj);
        }

        public IQueryable<T> Query<T>()
            where T : class
        {
            return Context.Set<T>().AsNoTracking();
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Attach<T>(T newUser) where T : class
        {
            var set = Context.Set<T>();
            set.Attach(newUser);
        }

        public void Dispose()
        {
            Context = null;
        }

        public T GetById<T>(Guid Id) where T : class
        {
            return Context.Find<T>(Id);
        }
    }
}
