using FluentValidation;
using Production.Grade.WebApi.Application.Validators;
using Production.Grade.WebApi.Application.DTO;

namespace Production_Grade_Web_API.Application.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }

}
