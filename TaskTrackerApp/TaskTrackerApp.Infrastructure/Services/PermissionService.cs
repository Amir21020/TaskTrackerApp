using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.Enums;
using TaskTrackerApp.Domain.Interfaces;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class PermissionService(IUserRepository userRepository) : IPermissionService
{
    public async Task<HashSet<PermissionType>> GetPermissionsAsync(Guid userId, CancellationToken ct = default)
        => await userRepository.GetUserPermissionsAsync(userId, ct); 
}