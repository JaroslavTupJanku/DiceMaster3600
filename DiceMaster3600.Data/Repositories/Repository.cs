using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceMaster3600.Data.Repositories
{
    public abstract class Repository<TEntity> : IDisposable where TEntity : class 
    {
        protected readonly SqlEFDataContext context;
        public Repository(SqlEFDataContext context) => this.context = context;

        public void Insert(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity is ISoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.DeletedDate = DateTime.UtcNow;
            }
            else
            {
                context.Set<TEntity>().Remove(entity);
            }

            context.SaveChanges();
        }

        public void Delete(TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                if (entity is ISoftDeleteEntity softDeleteEntity)
                {
                    softDeleteEntity.DeletedDate = DateTime.UtcNow;
                }
                else
                {
                    context.Set<TEntity>().Remove(entity);
                }
            }

            context.SaveChanges();
        }


        public void Dispose() => context.Dispose();
        public IEnumerable<TEntity> GetAll() => context.Set<TEntity>().ToList();
        public TEntity? GetById(int id) => context.Set<TEntity>().Find(id);
    }
}
