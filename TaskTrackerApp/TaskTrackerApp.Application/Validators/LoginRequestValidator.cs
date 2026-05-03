using FluentValidation;
using TaskTrackerApp.Application.DTOs;

namespace TaskTrackerApp.Application.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Введить Email.")
            .EmailAddress().WithMessage("Неверный формат Email адреса")
            .MaximumLength(100).WithMessage("Email не может быть длиннее 100 символов");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Введите пароль");
    }
}
