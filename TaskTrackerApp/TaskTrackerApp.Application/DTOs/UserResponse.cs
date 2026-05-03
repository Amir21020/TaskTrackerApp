namespace TaskTrackerApp.Application.DTOs;

public sealed record UserResponse(
    string FirstName,
    string LastName,
    string Email,
    string? AvatarUrl
);