using FluentValidation;
using Production.Grade.WebApi.Application.Validators;
using Production.Grade.WebApi.Application.DTOs;

namespace Production.Grade.WebApi.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(static x => x.EmailOrUsername)
                .NotEmpty().WithMessage("Email or username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }

        private object RuleFor(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }

}
