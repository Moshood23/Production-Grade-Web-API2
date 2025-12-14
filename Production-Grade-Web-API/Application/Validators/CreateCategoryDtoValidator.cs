using FluentValidation;
using Production.Grade.WebApi.Application.Validators;
using Production.Grade.WebApi.Application.DTOs;

namespace Production.Grade.WebApi.Application.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .Length(1, 100).WithMessage("Category name must be between 1 and 100 characters");

            RuleFor(static x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }

}
