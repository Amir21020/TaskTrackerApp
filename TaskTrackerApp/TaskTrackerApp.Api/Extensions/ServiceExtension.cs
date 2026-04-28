using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
        services.AddAuthentication(); 
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"))
        );
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
