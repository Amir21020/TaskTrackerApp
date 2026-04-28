using TaskTrackerApp.Api;
using TaskTrackerApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);
builder.Services.AddAuth(config);
builder.Services.AddPersistence(config);

var app = builder.Build();

app.UseHttpsRedirection();       
app.UseDevelopmentTools();       
app.UseCorsConfiguration();      
app.UseCustomAuthentication();   
app.UseApplicationEndpoints();   

app.Run();