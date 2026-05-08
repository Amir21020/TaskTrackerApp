using TaskTrackerApp.Domain.Entities;

namespace TaskTrackerApp.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task AddAsync(UserRole userRole, CancellationToken ct = default);
}