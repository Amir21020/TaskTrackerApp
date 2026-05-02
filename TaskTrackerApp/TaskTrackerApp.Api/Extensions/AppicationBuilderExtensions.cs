using Serilog;
using TaskTrackerApp.Api.Endpoints;
using TaskTrackerApp.Api.Middlewares;

namespace TaskTrackerApp.Api;

public static class AppicationBuilderExtensions
{
    public static WebApplication UseDevelopmentTools(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        return app;
    }

    public static WebApplication UseCustomAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }

    public static WebApplication UseLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        return app;
    }

    public static WebApplication UseCorsConfiguration(this WebApplication app)
    {
        app.UseCors("AllowVueApp");
        return app;
    }

    public static WebApplication UseApplicationEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoint();

        return app;
    }
}
