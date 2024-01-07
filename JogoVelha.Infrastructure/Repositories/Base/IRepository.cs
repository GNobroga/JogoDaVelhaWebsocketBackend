using JogoVelha.Domain.Entities.Base;

namespace JogoVelha.Infrastructure.Repositories.Base;

public interface IRepository<TEntity> where TEntity: EntityBase
{
    Task<List<TEntity>> FindAllAsync();

    Task<TEntity> FindByIdAsync(int id);

    Task<TEntity> CreateAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(int id);
}