using Microsoft.EntityFrameworkCore;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Persistence.Data;

namespace TaskTrackerApp.Persistence.Repositories;

public sealed class PasswordResetTokenRepository(AppDbContext context) : GenericRepository<PasswordResetToken>(context), IPasswordResetTokenRepository
{
    public async Task<PasswordResetToken> GetByEmailAndTokenAsync(string email, string tokenHash, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(x => x.Email == email && x.TokenHash == tokenHash);
}