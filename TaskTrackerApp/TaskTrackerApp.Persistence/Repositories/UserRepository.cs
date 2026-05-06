using Microsoft.EntityFrameworkCore;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Enums;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public sealed class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(u => u.GoogleId == googleId, ct);

    public async Task<HashSet<PermissionType>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default)
    {
        var roles = await _dbSet.AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync(ct);

        return roles.SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (PermissionType)p.Id)
            .ToHashSet();
    }
}