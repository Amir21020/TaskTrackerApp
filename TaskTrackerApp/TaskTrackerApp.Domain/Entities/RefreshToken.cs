namespace TaskTrackerApp.Domain.Entities;

public sealed class RefreshToken : BaseEntity
{
    public string TokenHash { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

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