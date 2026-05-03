namespace TaskTrackerApp.Application.DTOs;

public sealed record LoginRequest(string Email, string Password, bool RememberMe);