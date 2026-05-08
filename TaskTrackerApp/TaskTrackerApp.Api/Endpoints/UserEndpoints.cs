using System.Security.Claims;
using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users").RequireAuthorization();

        group.MapPost("/me/onboarding", CompleteOnboardingAsync);
    }

    private static async Task<IResult> CompleteOnboardingAsync(
        IUserService userService,
        int roleId,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken ct = default)
    {
        var userIdString = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userId))
            return Results.Unauthorized();
        
        await userService.CompleteOnboardingAsync(userId, roleId, ct);

        return Results.NoContent();
    }
}