using JogoVelha.Domain.Entities.Base;
using JogoVelha.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JogoVelha.Infrastructure.Repositories.Implementation;

public class RepositoryBase<TEntity, TContext>(TContext context) : IRepository<TEntity> where TEntity: EntityBase where TContext: DbContext
{   
    protected TContext _context = context;

    private DbSet<TEntity> _dataset = context.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
       await _dataset.AddAsync(entity);
       await _context.SaveChangesAsync();
       return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dataset.FindAsync(id);
        _dataset.Remove(entity!);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<TEntity>> FindAllAsync()
    {
        return await _dataset.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> FindByIdAsync(int id)
    {
        return (await _dataset.FindAsync(id))!;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dataset.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}