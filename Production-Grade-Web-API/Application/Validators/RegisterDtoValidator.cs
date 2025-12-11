using Production_Grade_Web_API.Application.DTOs;

namespace Production.Grade.WebApi.Application.Validators;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be valid");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 100).WithMessage("Username must be between 3 and 100 characters");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .Length(2, 200).WithMessage("Full name must be between 2 and 200 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}
