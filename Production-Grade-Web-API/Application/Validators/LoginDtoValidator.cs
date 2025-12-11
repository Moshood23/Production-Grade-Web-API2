using Production.Grade.WebApi.Application.Validators;

namespace Production_Grade_Web_API.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty().WithMessage("Email or username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }

}
