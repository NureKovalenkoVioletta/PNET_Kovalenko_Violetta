using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> DbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbContext = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        DbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
