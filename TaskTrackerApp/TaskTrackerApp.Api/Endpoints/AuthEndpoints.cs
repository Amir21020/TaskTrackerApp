using TaskTrackerApp.Api.Filters;
using TaskTrackerApp.Application.DTOs;
using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/google-login", GoogleLoginAsync);
        group.MapPost("/sign-up", RegisterAsync).AddEndpointFilter<ValidationFilter<RegisterRequest>>();
        group.MapPost("/login", LoginAsync).AddEndpointFilter<ValidationFilter<LoginRequest>>();
        group.MapPost("/refresh", RefreshAsync);
    }

    private static async Task<IResult> RegisterAsync(IAuthService authService, RegisterRequest request, CancellationToken ct = default)
    {
        await authService.RegisterAsync(request, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> LoginAsync(IAuthService authService, LoginRequest request, HttpResponse response, CancellationToken ct = default)
    {
        var loginResult = await authService.LoginAsync(request, ct);

        SetTokenCookies(response, loginResult);

        return Results.Ok(loginResult.User);
    }

    private static async Task<IResult> GoogleLoginAsync(IAuthService authService, GoogleLoginRequest request, HttpResponse response, CancellationToken ct = default)
    {
        var loginResult = await authService.LoginWithGoogleAsync(request, ct);

        SetTokenCookies(response, loginResult);

        return Results.Ok(loginResult.User);
    }

    private static async Task<IResult> RefreshAsync(IAuthService authService,
        HttpRequest request,
        HttpResponse response,
        CancellationToken ct = default)
    {
        if (!request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            return Results.Unauthorized();

        var loginResult = await authService.RefreshAsync(refreshToken, ct);

        SetTokenCookies(response, loginResult);

        return Results.Ok(loginResult.User);
    }

    private static void SetTokenCookies(HttpResponse response, LoginResponse loginResult)
    {
        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = loginResult.AccessToken.Expiry
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = loginResult.RefreshToken.Expiry
        };

        response.Cookies.Append("access_token", loginResult.AccessToken.Token, accessCookieOptions);
        response.Cookies.Append("refresh_token", loginResult.RefreshToken.Token, refreshCookieOptions);
    }
}