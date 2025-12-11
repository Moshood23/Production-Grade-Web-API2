using Production.Grade.WebApi.Application.Validators;
using Production_Grade_Web_API.Application.DTO;

namespace Production_Grade_Web_API.Application.Validators
{
    public class CreateCartItemDtoValidator : AbstractValidator<CreateCartItemDto>
    {
        public CreateCartItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(int.MaxValue).WithMessage("Quantity is too large");
        }
    }

}
