using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.Roles)
                .WithMany(u => u.Users)
                .UsingEntity<UserRole>(
                    l => l.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId),
                    r => r.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId)
        );
    }
}