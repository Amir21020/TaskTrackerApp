using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Enums;

namespace TaskTrackerApp.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken ct = default);
    Task<HashSet<PermissionType>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default);
}