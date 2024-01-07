using JogoVelha.Domain.Entities.Base;
using JogoVelha.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JogoVelha.Infrastructure.Repositories.Implentation;

public class RepositoryBase<TEntity, TContext>(TContext context) : IRepository<TEntity> where TEntity: EntityBase where TContext: DbContext
{   
    protected TContext _context = context;

    private DbSet<TEntity> _dataset = context.Set<TEntity>();

    public TEntity Create(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> FindAll()
    {
        throw new NotImplementedException();
    }

    public TEntity FindById(int id)
    {
        throw new NotImplementedException();
    }

    public TEntity Update(TEntity entity)
    {
        throw new NotImplementedException();
    }
}