using Microsoft.Extensions.Logging;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Domain.DTOs;
using TaskTrackerApp.Domain.Entities;
using TaskTrackerApp.Domain.Interfaces;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class AuthService(
    INumericCodeGenerator numericCodeGenerator,
    IUnitOfWork unitOfWork,
    ILogger<AuthService> logger,
    IEmailService emailService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository) : IAuthService
{
    public async Task RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUser != null)
            throw new InvalidOperationException("Такой пользователь уже существует");
        
        var passwordHash = passwordHasher.Hash(request.Password);
        
        var code = numericCodeGenerator.Generate(6);
        
        var user = new User(
            request.Email,
            request.FirstName,
            request.LastName,
            passwordHash,
            code);
        
        await userRepository.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);


        await emailService.SendVerificationCodeAsync(request.Email, code, ct);
        logger.LogInformation("User {Email} registered. Verification code sent.", request.Email);
    }
}