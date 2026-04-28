using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.DTOs;

namespace TaskTrackerApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/sign-up", RegisterAsync);
    }

    private static async Task<IResult> RegisterAsync(IAuthService authService, RegisterRequest request, CancellationToken ct = default)
    {
        await authService.RegisterAsync(request, ct);
        return Results.NoContent();
    }
}