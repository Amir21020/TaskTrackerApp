using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public sealed class UserRoleRepository(AppDbContext context) : IUserRoleRepository
{
    public async Task AddAsync(UserRole userRole, CancellationToken ct = default)
        => await context.UserRoles.AddAsync(userRole, ct);
}