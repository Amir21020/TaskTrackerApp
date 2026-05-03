using TaskTrackerApp.Application.DTOs;
using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Application.Interfaces;

public interface ITokenProvider
{
    TokenResult GenerateAccessToken(User user);
    string GenerateRefreshToken();
}