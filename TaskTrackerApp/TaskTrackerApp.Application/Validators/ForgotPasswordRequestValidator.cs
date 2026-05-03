using FluentValidation;
using TaskTrackerApp.Application.DTOs;

namespace TaskTrackerApp.Application.Validators;

public sealed class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email обязателен для заполнения.")
            .EmailAddress().WithMessage("Введите корректный адрес электронной почты.")
            .MaximumLength(254).WithMessage("Email слишком длинный.");
    }
}
