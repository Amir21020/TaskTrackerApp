namespace TaskTrackerApp.Application.DTOs;

public sealed record VerifyEmailRequest(string Email, string Code);