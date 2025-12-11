using Production.Grade.WebApi.Application.Validators;

namespace Production_Grade_Web_API.Application.Validators
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .Length(1, 100).WithMessage("Category name must be between 1 and 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }

}
