using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Domain.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string refreshToken, CancellationToken ct = default);
}