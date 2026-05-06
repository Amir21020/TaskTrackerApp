using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>(
                l  => l.HasOne<Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                r  => r.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId)
            );
    }
}
