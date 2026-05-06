using TaskTrackerApp.Application.DTOs.RoleResponse;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.Interfaces;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public async Task<List<RoleResponse>> GetRolesAsync(CancellationToken ct = default)
    {
        var roles = await roleRepository.GetAllAsync(ct);
        return roles.Select(r => new RoleResponse(r.Id, r.Name, r.IconUrl, r.Description)).ToList();
    }
}