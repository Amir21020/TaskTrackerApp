using TaskTrackerApp.Application.DTOs;
using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/sign-up", RegisterAsync);
        group.MapPost("/google-login", GoogleLoginAsync);
    }

    private static async Task<IResult> RegisterAsync(IAuthService authService, RegisterRequest request, CancellationToken ct = default)
    {
        await authService.RegisterAsync(request, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> GoogleLoginAsync(IAuthService authService, GoogleLoginRequest request, CancellationToken ct = default)
    {
        var tokenResponse = await authService.LoginWithGoogleAsync(request, ct);
        return Results.Ok(tokenResponse);
    }
}