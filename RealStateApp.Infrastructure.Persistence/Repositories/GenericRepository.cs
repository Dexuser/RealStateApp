using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Domain.Interfaces;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence.Repositories;


public class GenericRepository<TEntity> : IGenericRepository<TEntity>  where TEntity : class
{
    protected readonly RealStateAppContext Context;

    public GenericRepository(RealStateAppContext context)
    {
        this.Context = context;
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id) 
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
    {
        await Context.Set<TEntity>().AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        return entities;
    }

    public virtual async Task<TEntity?> UpdateAsync(int id, TEntity entity)
    {
        var entityToUpdate = await Context.Set<TEntity>().FindAsync(id);
        if (entityToUpdate != null)
        {
            Context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();
            return entityToUpdate;
        }

        return null;
    }

    public virtual async Task DeleteAsync(int entity)
    {
        var entityToDelete = await Context.Set<TEntity>().FindAsync(entity);
        if (entityToDelete != null)
        {
            Context.Set<TEntity>().Remove(entityToDelete);
            await Context.SaveChangesAsync();
        }
    }

    public IQueryable<TEntity> GetAllQueryable()
    {
        return Context.Set<TEntity>().AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }
}