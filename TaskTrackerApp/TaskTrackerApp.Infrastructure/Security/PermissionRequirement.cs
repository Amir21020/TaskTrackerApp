using Microsoft.AspNetCore.Authorization;
using TaskTrackerApp.Domain.Enums;

namespace TaskTrackerApp.Infrastructure.Security;

public class PermissionRequiremen(PermissionType[] permissions) : IAuthorizationRequirement
{
    public PermissionType[] Permissions { get; set; } = permissions;
}
