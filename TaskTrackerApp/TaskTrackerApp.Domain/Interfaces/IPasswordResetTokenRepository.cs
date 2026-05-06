using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Domain.Interfaces;

public interface IPasswordResetTokenRepository : IGenericRepository<PasswordResetToken, Guid>
{
    Task<PasswordResetToken> GetByEmailAndTokenAsync(string email, string tokenHash, CancellationToken ct = default);
}