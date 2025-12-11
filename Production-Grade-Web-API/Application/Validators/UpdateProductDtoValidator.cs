using Production.Grade.WebApi.Application.Validators;
using Production_Grade_Web_API.Application.DTO;

namespace Production_Grade_Web_API.Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .Length(1, 200).WithMessage("Product name must be between 1 and 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than 0")
                .When(x => x.Price.HasValue && x.Price > 0);

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Product quantity must be greater than or equal to 0")
                .When(x => x.Quantity.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid product status")
                .When(x => x.Status.HasValue);
        }
    }

}
