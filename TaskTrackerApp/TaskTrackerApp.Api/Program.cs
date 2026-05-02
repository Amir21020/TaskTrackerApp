using Serilog;
using TaskTrackerApp.Api;
using TaskTrackerApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));

var config = builder.Configuration;

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);
builder.Services.AddAuth(config);
builder.Services.AddPersistence(config);
builder.Services.AddErrorHandling();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();       
app.UseDevelopmentTools();       
app.UseCorsConfiguration();
app.UseLogging();
app.UseCustomAuthentication();   
app.UseApplicationEndpoints();   

app.Run();