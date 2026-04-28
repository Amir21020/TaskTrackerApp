using TaskTrackerApp.Application.Interfaces;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
    

    public bool Verify(string password, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}