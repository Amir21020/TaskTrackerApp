namespace TaskTrackerApp.Application.Interfaces;

public interface IUserService
{
    Task CompleteOnboardingAsync(Guid userId, int roleId, CancellationToken ct = default);
}
