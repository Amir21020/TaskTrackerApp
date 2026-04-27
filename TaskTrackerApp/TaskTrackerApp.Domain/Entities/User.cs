namespace TaskTrackerApp.Domain.Entities;

public sealed class User : BaseEntity
{
    public User()
    {

    }

    public User(string email, string firstName, string lastName, string passwordHash, string code)
    {
        Email = email.ToLowerInvariant().Trim();
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PasswordHash = passwordHash;
    }

    public string Email { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public string? AvatarUrl { get; set; }

    public bool IsEmailConfirmed { get; set; } = false;
    public string? VerificationCode { get; set; }
    public DateTimeOffset? VerificationCodeExpiresAt { get; set; }
}