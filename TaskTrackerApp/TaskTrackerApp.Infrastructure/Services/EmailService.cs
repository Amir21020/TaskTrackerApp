using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.RegularExpressions;
using TaskTrackerApp.Application.Interfaces;
using TaskTrackerApp.Infrastructure.Configuration;

namespace TaskTrackerApp.Infrastructure.Services;

public sealed class EmailService(
    IOptions<SmtpOptions> options,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpOptions _options = options.Value;
    public async Task SendVerificationCodeAsync(string to, string code, CancellationToken ct = default)
    {
        logger.LogInformation("Sending verification code to {Email}", to);
        var body = await GetTemplateAsync(
            "EmailConfirmation",
            new() { ["ConfirmationCode"] = code, ["AppName"] = _options.FromName },
            ct);
        await SendEmailInternalAsync(to, "Your Verification Code", body, ct);
    }
    private async Task SendEmailInternalAsync(
        string to,
        string subject,
        string htmlBody,
        CancellationToken ct)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.FromName, _options.FromAddress));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = "Your email client does not support HTML messages."
        };
        message.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls, ct);
            await client.AuthenticateAsync(_options.UserName, _options.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
            logger.LogInformation("Email successfully sent to {Email}", to);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Email sending was canceled for {Email}", to);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "SMTP error while sending email to {Email}", to);
            throw;
        }
    }
    private async Task<string> GetTemplateAsync(
        string templateName,
        Dictionary<string, string> placeholders,
        CancellationToken ct)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Templates", $"{templateName}.html");
        if (!File.Exists(path))
        {
            logger.LogError("Email template file missing: {Path}", path);
            throw new FileNotFoundException("Email шаблона не существует", path);
        }
        var content = await File.ReadAllTextAsync(path, ct);
        foreach (var entry in placeholders)
        {
            var pattern = $@"\{{\{{\s*{Regex.Escape(entry.Key)}\s*\}}\}}";
            content = Regex.Replace(
                content,
                pattern,
                _ => entry.Value ?? string.Empty,
                RegexOptions.IgnoreCase);
        }
        return content;
    }

    public async Task SendPasswordResetLinkAsync(string to, string resetLink, CancellationToken ct = default)
    {
        logger.LogInformation("Sending password reset link to {Email}", to);
        var body = await GetTemplateAsync("ResetPassword", new() { ["ResetLink"] = resetLink, ["AppName"] = _options.FromName }, ct);
        await SendEmailInternalAsync(to, "Password Reset Request", body, ct);
    }
}