namespace TaskTrackerApp.Domain.Entities;

public sealed class PasswordResetToken : BaseEntity<Guid>
{
    public PasswordResetToken()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public string Email { get; set; }
    public string TokenHash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public bool IsUsed { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public static PasswordResetToken Create(string tokenHash, Guid userId, string email, DateTime expiry)
    {
        return new PasswordResetToken
        {
            TokenHash = tokenHash,
            UserId = userId,
            Email = email,
            ExpiresAt = expiry,
            IsUsed = false
        };
    }
}