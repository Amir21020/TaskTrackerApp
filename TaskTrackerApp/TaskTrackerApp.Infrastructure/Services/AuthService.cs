using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
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
    IPasswordResetTokenRepository passwordResetTokenRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository) : IAuthService
{
    private readonly GoogleAuthorizationCodeFlow _flow = new(new GoogleAuthorizationCodeFlow.Initializer
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
        logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var user = await userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            logger.LogWarning("Login failed: Invalid credentials for {Email}", request.Email);
            throw new AuthenticationException("Неверный email или пароль.");
        }

        var accessTokenResult = tokenProvider.GenerateAccessToken(user);
        var refreshTokenValue = tokenProvider.GenerateRefreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(request.RememberMe ? 30 : 7);

        await SaveRefreshToken(refreshTokenValue, user.Id, expiryDate, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("User {UserId} logged in successfully", user.Id);
        return CreateLoginResponse(user, accessTokenResult.Token, accessTokenResult.Expiry, refreshTokenValue, expiryDate);
    }

    public async Task<LoginResponse> LoginWithGoogleAsync(GoogleLoginRequest request, CancellationToken ct = default)
    {
        logger.LogInformation("Processing Google login");

        var tokenResponse = await _flow.ExchangeCodeForTokenAsync("user", request.Code, "postmessage", ct);
        var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResponse.IdToken, new() { Audience = new[] { options.Value.ClientId } });

        var user = await userRepository.GetByGoogleIdAsync(payload.Subject, ct)
                   ?? await userRepository.GetByEmailAsync(payload.Email, ct);

        if (user is null)
        {
            user = User.CreateWithGoogle(payload.Email, payload.GivenName, payload.FamilyName, payload.Subject, payload.Picture);
            await userRepository.AddAsync(user, ct);
            await unitOfWork.SaveChangesAsync(ct);
            logger.LogInformation("New user registered via Google: {Email}", user.Email);
        }
        else if (user.GoogleId is null)
        {
            user.GoogleId = payload.Subject;
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(ct);
            logger.LogInformation("Google account linked to existing user: {Email}", user.Email);
        }

        var accessTokenResult = tokenProvider.GenerateAccessToken(user);
        var refreshTokenValue = tokenProvider.GenerateRefreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(7);

        await SaveRefreshToken(refreshTokenValue, user.Id, expiryDate, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return CreateLoginResponse(user, accessTokenResult.Token, accessTokenResult.Expiry, refreshTokenValue, expiryDate);
    }

    public async Task<LoginResponse> RefreshAsync(string refreshToken, CancellationToken ct = default)
    {
        var hashedToken = HashToken(refreshToken);
        var tokenEntity = await refreshTokenRepository.GetByTokenAsync(hashedToken, ct);

        if (tokenEntity is null || tokenEntity.ExpiresAt < DateTime.UtcNow || tokenEntity.IsRevoked)
        {
            logger.LogWarning("Refresh token attempt failed: token invalid, expired, or revoked");
            throw new AuthenticationException("Время сессии истекло. Выполните повторный вход.");
        }

        var user = await userRepository.GetByIdAsync(tokenEntity.UserId, ct);
        refreshTokenRepository.Delete(tokenEntity);

        var accessTokenResult = tokenProvider.GenerateAccessToken(user!);
        var newRefreshTokenValue = tokenProvider.GenerateRefreshToken();
        var newExpiry = DateTime.UtcNow.AddDays(7);

        await SaveRefreshToken(newRefreshTokenValue, user!.Id, newExpiry, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return CreateLoginResponse(user, accessTokenResult.Token, accessTokenResult.Expiry, newRefreshTokenValue, newExpiry);
    }

    public async Task RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        var existingUser = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser != null)
        {
            logger.LogWarning("Registration failed: User {Email} already exists", request.Email);
            throw new InvalidOperationException("Пользователь уже существует.");
        }

        var passwordHash = passwordHasher.Hash(request.Password);
        var code = numericCodeGenerator.Generate(6);
        var user = User.CreateWithCredentials(request.Email, request.FirstName, request.LastName, passwordHash, code);

        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await emailService.SendVerificationCodeAsync(request.Email, code, ct);
        logger.LogInformation("User {UserId} registered, verification code sent", user.Id);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        logger.LogInformation("Forgot password request for: {Email}", request.Email);

        var user = await userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null) return;

        var token = tokenProvider.GenerateRefreshToken();
        var hashedToken = HashToken(token);

        var resetToken = PasswordResetToken.Create(hashedToken, user.Id, request.Email, DateTime.UtcNow.AddMinutes(15));
        await passwordResetTokenRepository.AddAsync(resetToken, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var resetLink = $"http://localhost:5173/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(request.Email)}";
        await emailService.SendPasswordResetLinkAsync(request.Email, resetLink, ct);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        logger.LogInformation("Reset password attempt for: {Email}", request.Email);

        var hashedToken = HashToken(request.Token);
        var tokenEntity = await passwordResetTokenRepository.GetByEmailAndTokenAsync(request.Email, hashedToken, ct);

        if (tokenEntity is null || tokenEntity.IsUsed || tokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            logger.LogWarning("Password reset failed: Invalid or expired token for {Email}", request.Email);
            throw new AuthenticationException("Неверная или истекшая ссылка для сброса пароля.");
        }

        var user = await userRepository.GetByIdAsync(tokenEntity.UserId, ct);
        if (user is null) throw new AuthenticationException("Пользователя не существует.");

        user.PasswordHash = passwordHasher.Hash(request.NewPassword);
        userRepository.Update(user);

        tokenEntity.IsUsed = true;
        passwordResetTokenRepository.Update(tokenEntity);

        await unitOfWork.SaveChangesAsync(ct);
        logger.LogInformation("Password reset successful for user: {UserId}", user.Id);
    }

    public async Task<LoginResponse> VerifyEmailAsync(VerifyEmailRequest request, CancellationToken ct = default)
    {
        logger.LogInformation("Email verification attempt for: {Email}", request.Email);

        var user = await userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null || user.VerificationCode != request.Code || user.VerificationCodeExpiresAt < DateTime.UtcNow)
        {
            logger.LogWarning("Verification failed for {Email}: Invalid code or expired", request.Email);
            throw new AuthenticationException("Неверный или истекший код подтверждения.");
        }

        user.IsEmailConfirmed = true;
        user.VerificationCode = null;
        user.VerificationCodeExpiresAt = null;
        userRepository.Update(user);

        var accessTokenResult = tokenProvider.GenerateAccessToken(user);
        var refreshTokenValue = tokenProvider.GenerateRefreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(7);

        await SaveRefreshToken(refreshTokenValue, user.Id, expiryDate, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Email verified and user {UserId} logged in", user.Id);
        return CreateLoginResponse(user, accessTokenResult.Token, accessTokenResult.Expiry, refreshTokenValue, expiryDate);
    }

    public async Task ResendVerificationCodeAsync(string email, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);
        if (user is null || user.IsEmailConfirmed) return;

        var code = numericCodeGenerator.Generate(6);
        user.VerificationCode = code;
        user.VerificationCodeExpiresAt = DateTimeOffset.UtcNow.AddMinutes(15);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(ct);

        await emailService.SendVerificationCodeAsync(email, code, ct);
        logger.LogInformation("Verification code resent to {Email}", email);
    }

    private async Task SaveRefreshToken(string token, Guid userId, DateTime expiry, CancellationToken ct)
    {
        var hashedToken = HashToken(token);
        var tokenEntity = RefreshToken.Create(hashedToken, userId, expiry);
        await refreshTokenRepository.AddAsync(tokenEntity, ct);
    }

    private static LoginResponse CreateLoginResponse(User user, string accessToken, DateTime accessExpiry, string refreshToken, DateTime refreshExpiry)
    {
        return new LoginResponse(
            new TokenDto(accessToken, accessExpiry),
            new TokenDto(refreshToken, refreshExpiry),
            new UserResponse(user.FirstName, user.LastName, user.Email, user.AvatarUrl)
        );
    }

    private static string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(token)));
    }
}