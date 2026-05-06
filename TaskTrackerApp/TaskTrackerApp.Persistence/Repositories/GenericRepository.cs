using Microsoft.EntityFrameworkCore;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public class GenericRepository<TEntity, TId>(AppDbContext context) : IGenericRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity)
        => _dbSet.Remove(entity);

    public async Task<IReadOnlyList<TEntity>?> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id").Equals(id), cancellationToken);
    public void Update(TEntity entity)
        => _dbSet.Update(entity);
}
