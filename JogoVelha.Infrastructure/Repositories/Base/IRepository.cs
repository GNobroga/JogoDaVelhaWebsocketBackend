using JogoVelha.Domain.Entities.Base;

namespace JogoVelha.Infrastructure.Repositories.Base;

public interface IRepository<TEntity> where TEntity: EntityBase
{
    List<TEntity> FindAll();

    TEntity FindById(int id);

    TEntity Create(TEntity entity);

    TEntity Update(TEntity entity);

    bool Delete(int id);
}