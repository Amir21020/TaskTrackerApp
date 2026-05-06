using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Enums;
using TaskTrackerApp.Infrastructure.Configuration;

namespace TaskTrackerApp.Persistence.Data;

public sealed class RoleSeeder(AppDbContext context, IOptions<AuthorizationOptions> options, IBlobStorage blobStorage)
{
    public async Task SeedAsync()
    {
        foreach (var roleConfig in options.Value.RolePermissions)
        {
            if (!Enum.IsDefined(typeof(RoleType), roleConfig.Id))
            {
                continue; 
            }

            var roleType = (RoleType)roleConfig.Id;
            var roleName = roleType.ToString();

            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                role = new Role
                {
                    Id = (int)roleType, 
                    Name = roleName,
                    IconUrl = blobStorage.GetPublicUrl(roleConfig.Icon),
                    Description = roleConfig.Description
                };
                context.Roles.Add(role);
            }

            foreach (var permName in roleConfig.Permissions)
            {
                if (Enum.TryParse<PermissionType>(permName, out var permType))
                {
                    if (!await context.RolePermissions.AnyAsync(rp => rp.RoleId == role.Id && rp.PermissionId == (int)permType))
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = (int)permType
                        });
                    }
                }
            }
        }
        await context.SaveChangesAsync();
    }
}