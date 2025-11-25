namespace RealStateApp.Core.Domain.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);
    Task<TEntity?> UpdateAsync(int id,TEntity entity);
    Task DeleteAsync(int id);
    IQueryable<TEntity> GetAllQueryable();
}