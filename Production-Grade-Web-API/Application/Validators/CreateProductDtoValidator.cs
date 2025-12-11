using Production.Grade.WebApi.Application.Validators;
using Production_Grade_Web_API.Application.DTO;

namespace Production_Grade_Web_API.Application.Validators
{

    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .Length(1, 200).WithMessage("Product name must be between 1 and 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Product price is required")
                .GreaterThan(0).WithMessage("Product price must be greater than 0");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Product quantity is required")
                .GreaterThanOrEqualTo(0).WithMessage("Product quantity must be greater than or equal to 0");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid product status");
        }
    }

}
