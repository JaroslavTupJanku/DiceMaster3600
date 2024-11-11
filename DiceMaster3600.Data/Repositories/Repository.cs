using DiceMaster3600.Data.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiceMaster3600.Data.Repositories
{
    public abstract class Repository<TEntity> : IDisposable where TEntity : class 
    {
        protected readonly SqlEFDataContext context;
        public Repository(SqlEFDataContext context) => this.context = context;

        public async Task InsertAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity is ISoftDeleteEntity softDeleteEntity)
            {
                softDeleteEntity.DeletedDate = DateTime.UtcNow;
            }
            else
            {
                context.Set<TEntity>().Remove(entity);
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            var entitiesArray = entities as TEntity[] ?? entities.ToArray();

            foreach (var entity in entitiesArray)
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

            await context.SaveChangesAsync();
        }


        public void Dispose() => context.Dispose();
        public IEnumerable<TEntity> GetAll() => context.Set<TEntity>().ToList();
        public TEntity? GetById(int id) => context.Set<TEntity>().Find(id);
    }
}
