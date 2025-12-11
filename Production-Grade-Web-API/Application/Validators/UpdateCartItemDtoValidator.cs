using Production.Grade.WebApi.Application.Validators;
using Production_Grade_Web_API.Application.DTO;

namespace Production_Grade_Web_API.Application.Validators
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
