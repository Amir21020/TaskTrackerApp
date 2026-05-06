namespace TaskTrackerApp.Infrastructure.Configuration;

public sealed class AuthorizationOptions
{
    public RolePermissions[] RolePermissions { get; set; } = [];
}

public sealed class RolePermissions
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set;} = string.Empty; 
    public string[] Permissions { get; set; } = [];
}