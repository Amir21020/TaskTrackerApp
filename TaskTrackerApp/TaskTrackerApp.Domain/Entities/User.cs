namespace TaskTrackerApp.Domain.Entities;

public sealed class User : BaseEntity<Guid>
{
    public User()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public static User CreateWithCredentials(string email, string firstName, string lastName, string passwordHash, string code)
    {
        return new User
        {
            Email = email.ToLowerInvariant().Trim(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            PasswordHash = passwordHash,
            VerificationCode = code,
            VerificationCodeExpiresAt = DateTimeOffset.UtcNow.AddMinutes(15)
        };
    }

    public static User CreateWithGoogle(string email, string firstName, string lastName, string googleId, string? avatarUrl)
    {
        return new User
        {
            Email = email.ToLowerInvariant().Trim(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            GoogleId = googleId,
            AvatarUrl = avatarUrl,
            IsEmailConfirmed = true
        };
    }

    public string Email { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public string? PasswordHash { get; set; }
    public string? AvatarUrl { get; set; }
    public string? GoogleId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public bool IsEmailConfirmed { get; set; } = false;
    public string? VerificationCode { get; set; }
    public DateTimeOffset? VerificationCodeExpiresAt { get; set; }

    public ICollection<Role> Roles { get; set; } = [];
}