using TaskTrackerApp.Domain.Enums;

namespace TaskTrackerApp.Application.Interfaces;

public interface IPermissionService
{
    public Task<HashSet<PermissionType>> GetPermissionsAsync(Guid userId, CancellationToken ct = default);
}
