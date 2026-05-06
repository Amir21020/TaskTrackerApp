using TaskTrackerApp.Application.DTOs.RoleResponse;

namespace TaskTrackerApp.Application.Interfaces;

public interface IRoleService
{
    Task<List<RoleResponse>> GetRolesAsync(CancellationToken ct = default);
}
