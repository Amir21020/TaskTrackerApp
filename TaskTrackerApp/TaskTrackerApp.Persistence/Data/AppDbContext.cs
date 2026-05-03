using Microsoft.EntityFrameworkCore;
using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Persistence.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> context) : DbContext(context)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
}
