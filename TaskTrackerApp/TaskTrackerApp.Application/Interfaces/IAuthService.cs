using TaskTrackerApp.Application.DTOs;

namespace TaskTrackerApp.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<LoginResponse> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken ct = default);
    Task<LoginResponse> RefreshAsync(string refreshToken, CancellationToken ct = default);
}
