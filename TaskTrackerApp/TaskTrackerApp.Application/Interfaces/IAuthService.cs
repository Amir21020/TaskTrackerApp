using TaskTrackerApp.Domain.DTOs;

namespace TaskTrackerApp.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request, CancellationToken ct = default);
}
