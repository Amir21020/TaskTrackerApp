using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using TaskTrackerApp.Application.DTOs;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Infrastructure.Configuration;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class AuthService(
    INumericCodeGenerator numericCodeGenerator,
    IUnitOfWork unitOfWork,
    ILogger<AuthService> logger,
    IOptions<GoogleOptions> options,
    IEmailService emailService,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository) : IAuthService
{
    private readonly GoogleAuthorizationCodeFlow _flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
    {
        ClientSecrets = new Google.Apis.Auth.OAuth2.ClientSecrets
        {
            ClientId = options.Value.ClientId,
            ClientSecret = options.Value.ClientSecret,
        },
        Scopes = new[] { "openid", "email", "profile" }
    });

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, ct);
        const string authError = "Неверный email или пароль.";

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AuthenticationException(authError);
        }

        var accessTokenValue = tokenProvider.GenerateAccessToken(user);
        var refreshTokenValue = tokenProvider.GenerateRefreshToken();


        var expiryDate = DateTime.UtcNow.AddDays(request.RememberMe ? 30 : 7);

        var token = RefreshToken.Create(refreshTokenValue, user.Id, expiryDate);
        await refreshTokenRepository.AddAsync(token, ct);

        return new LoginResponse(
            new TokenDto(accessTokenValue.Token, accessTokenValue.Expiry),
            new TokenDto(refreshTokenValue, expiryDate),
            new UserResponse(user.FirstName, user.LastName, user.Email, user.AvatarUrl)
        );
    }
    public async Task<LoginResponse> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken ct = default)
    {
        var tokenResponse = await _flow.ExchangeCodeForTokenAsync("user", request.Code, "postmessage", ct);
        var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, new() { Audience = new[] { options.Value.ClientId } });

        var user = await userRepository.GetByGoogleIdAsync(payload.Subject, ct)
                   ?? await userRepository.GetByEmailAsync(payload.Email, ct);

        if (user is null)
        {
            user = User.CreateWithGoogle(payload.Email, payload.GivenName, payload.FamilyName, payload.Subject, payload.Picture);
            await userRepository.AddAsync(user, ct);
        }
        else if (user.GoogleId is null)
        {
            user.GoogleId = payload.Subject;
            userRepository.Update(user);
        }

        var accessTokenResult = tokenProvider.GenerateAccessToken(user);
        var refreshTokenValue = tokenProvider.GenerateRefreshToken();

        var refreshExpiry = DateTime.UtcNow.AddDays(7);

        await refreshTokenRepository.AddAsync(RefreshToken.Create(refreshTokenValue, user.Id, refreshExpiry), ct);
        await unitOfWork.SaveChangesAsync(ct);

        return new LoginResponse(
            new TokenDto(accessTokenResult.Token, accessTokenResult.Expiry),
            new TokenDto(refreshTokenValue, refreshExpiry),
            new UserResponse(user.FirstName, user.LastName, user.Email, user.AvatarUrl)
        );

    }

    public async Task RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser != null)
            throw new InvalidOperationException("Такой пользователь уже существует");
        
        var passwordHash = passwordHasher.Hash(request.Password);
        
        var code = numericCodeGenerator.Generate(6);

        var user = User.CreateWithCredentials(request.Email, request.FirstName, request.LastName, passwordHash, code); ;
        
        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);


        await emailService.SendVerificationCodeAsync(request.Email, code, ct);
        logger.LogInformation("User {Email} registered. Verification code sent.", request.Email);
    }
}