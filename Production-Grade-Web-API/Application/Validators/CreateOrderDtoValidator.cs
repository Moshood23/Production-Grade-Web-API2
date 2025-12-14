using FluentValidation;
using Production.Grade.WebApi.Application.Validators;
using Production.Grade.WebApi.Application.DTO;
using Production_Grade_Web_API.Application.Validators;

namespace Production.Grade.WebApi.Application.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Order must contain at least one item")
                .Must(x => x.Count > 0).WithMessage("Order must contain at least one item");

            RuleForEach(x => x.OrderItems)
                .SetValidator(new CreateOrderItemDtoValidator());

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes must not exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }

}
