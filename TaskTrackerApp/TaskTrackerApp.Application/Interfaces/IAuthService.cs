using TaskTrackerApp.Application.DTOs;

namespace TaskTrackerApp.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<TokenResponse> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken ct = default);
}
