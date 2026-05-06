namespace TaskTrackerApp.Application.DTOs.RoleResponse;

public sealed record RoleResponse(int Id, string Label, string IconUrl, string? Description);