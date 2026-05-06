namespace TaskTrackerApp.Domain.Entities;

public sealed class Permission : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Role> Roles { get; set; } = [];
}
