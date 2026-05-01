using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Application.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}