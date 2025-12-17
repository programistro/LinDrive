using FluentValidation;
using LinDrive.Contracts.Requestes;

namespace LinDrive.Application.Validators;

public class AuthValidator : AbstractValidator<AuthRequest>
{
    public AuthValidator()
    {
        RuleFor(x => x.Email).NotNull().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is required");
        RuleFor(x => x.Password).NotNull().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters");
    }
}