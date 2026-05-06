namespace TaskTrackerApp.Domain.Entities;

public sealed class Role : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];
}