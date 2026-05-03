using FluentValidation;
using TaskTrackerApp.Application.DTOs;

namespace TaskTrackerApp.Application.Validators;

public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .EmailAddress().WithMessage("Некорректный формат Email.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Токен отсутствует или недействителен.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Введите новый пароль.")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage("Пароли не совпадают.");
    }
}
