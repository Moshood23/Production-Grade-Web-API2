using FluentValidation;
using Production.Grade.WebApi.Application.DTOs.Category;

namespace Production.Grade.WebApi.Application.Validators
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

        private object RuleFor(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }

}
