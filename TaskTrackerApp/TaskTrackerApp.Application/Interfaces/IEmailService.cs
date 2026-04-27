namespace TaskTrackerApp.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationCodeAsync(string to, string code, CancellationToken ct = default);
}
