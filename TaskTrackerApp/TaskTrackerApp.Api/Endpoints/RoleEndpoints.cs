using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Api.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles");

        group.MapGet("", GetRoles);
    }

    private static async Task<IResult> GetRoles(IRoleService roleService, CancellationToken ct = default)
    {
        var roles = await roleService.GetRolesAsync(ct);
        return Results.Ok(roles);
    }
}
