using Production.Grade.WebApi.Application.Validators;

namespace Production_Grade_Web_API.Application.Validators
{
    public class UpdateUserProfileDtoValidator : AbstractValidator<UpdateUserProfileDto>
    {
        public UpdateUserProfileDtoValidator()
        {
            RuleFor(x => x.FullName)
                .Length(2, 200).WithMessage("Full name must be between 2 and 200 characters")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10,15}$").WithMessage("Phone number must be between 10 and 15 digits")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }

}
