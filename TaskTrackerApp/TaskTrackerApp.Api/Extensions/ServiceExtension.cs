using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TaskTrackerApp.Api.Exceptions;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Application.Validators;
using TaskTrackerApp.Domain.Interfaces;
using TaskTrackerApp.Infrastructure.Configuration;
using TaskTrackerApp.Infrastructure.Services;
using TaskTrackerApp.Persistence.Data;
using TaskTrackerApp.Persistence.Repositories;

namespace TaskTrackerApp.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddCors(c =>
        {
            c.AddPolicy("AllowVueApp", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
        services.AddScoped<INumericCodeGenerator, NumericCodeGenerator>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.Configure<SmtpOptions>(config.GetSection(nameof(SmtpOptions)));
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<GoogleOptions>(config.GetSection(nameof(GoogleOptions)));
        services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
        services.AddScoped<ITokenProvider, TokenProvider>();
        var jwtOptions = config.GetSection(nameof(JwtOptions)).Get<JwtOptions>();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("access_token", out var cookieToken))
                            context.Token = cookieToken;

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<AuthService>>();
                        logger.LogInformation("Token validated for user: {UserId}", context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<AuthService>>();
                        logger.LogError(context.Exception, "Authentication failed");
                        return Task.CompletedTask;
                    }

                };
            });

        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
        );
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}
