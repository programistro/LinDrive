using FluentValidation;
using LinDrive.Contracts.Requestes;

namespace LinDrive.Application.Validators;

public class AuthValidator : AbstractValidator<AuthRequest>
{
    public AuthValidator()
    {
        RuleFor(x => x.Challenge).NotNull().WithMessage("Challenge is required")
            .EmailAddress().WithMessage("Email is required");
        RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");
    }
}