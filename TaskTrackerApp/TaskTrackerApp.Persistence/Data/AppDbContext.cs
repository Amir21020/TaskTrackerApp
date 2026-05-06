using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Infrastructure.Configuration;
using TaskTrackerApp.Persistence.Configurations;

namespace TaskTrackerApp.Persistence.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> context, IOptions<AuthorizationOptions> authOptions) : DbContext(context)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
    }
}
