namespace TaskTrackerApp.Application.DTOs;

public sealed record TokenResponse(string AuthToken, string RefreshToken);