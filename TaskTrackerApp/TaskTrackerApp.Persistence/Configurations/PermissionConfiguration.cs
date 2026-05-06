using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Enums;

namespace TaskTrackerApp.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        var permissions = Enum
            .GetValues<PermissionType>()
            .Select(p => new Permission
            {
                Id = (int)p,
                Name = p.ToString()
            });
        builder.HasData(permissions);
    }
}
