using Microsoft.EntityFrameworkCore;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public sealed class RefreshTokenRepository(AppDbContext context) : GenericRepository<RefreshToken, Guid>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken> GetByTokenAsync(string tokenHash, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
}