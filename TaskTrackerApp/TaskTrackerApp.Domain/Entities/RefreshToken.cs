namespace TaskTrackerApp.Domain.Entities;

public sealed class RefreshToken : BaseEntity<Guid>
{
    public RefreshToken()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public string TokenHash { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public static RefreshToken Create(string tokenHash, Guid userId, DateTime daysToExpire)
    {
        return new RefreshToken
        {
            TokenHash = tokenHash,
            UserId = userId,
            ExpiresAt = daysToExpire,
            IsRevoked = false 
        };
    }
}