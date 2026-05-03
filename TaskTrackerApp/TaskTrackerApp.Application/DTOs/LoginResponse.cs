namespace TaskTrackerApp.Application.DTOs;

public record LoginResponse(
    TokenDto AccessToken,
    TokenDto RefreshToken,
    UserResponse User
);