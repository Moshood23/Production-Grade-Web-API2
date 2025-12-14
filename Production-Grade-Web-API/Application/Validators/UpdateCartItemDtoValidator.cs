using FluentValidation;
using Production.Grade.WebApi.Application.Validators;
using Production.Grade.WebApi.Application.DTO;

namespace Production.Grade.WebApi.Application.Validators
{
    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(int.MaxValue).WithMessage("Quantity is too large");
        }
    }

}
