using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Infrastructure.Configuration;

namespace TaskTrackerApp.Persistence.Configurations;

public sealed class RolePermissionConfiguration(AuthorizationOptions options) : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(r => new { r.RoleId, r.PermissionId });
    }
}
