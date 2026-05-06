using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Infrastructure.Security;

public sealed class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequiremen>
{
    private readonly IServiceScopeFactory _seviceScopeFactory = serviceScopeFactory;
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequiremen requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
            c => c.Type == ClaimTypes.NameIdentifier);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }

        using var scope = _seviceScopeFactory.CreateScope();

        var permissionService = scope.ServiceProvider
            .GetRequiredService<IPermissionService>();

        var permissions = await permissionService.GetPermissionsAsync(id);

        if (permissions.Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
        }
    }
}