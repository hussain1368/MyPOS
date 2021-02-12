using FluentValidation;
using POS.WPF.Models;

namespace POS.WPF.ModelValidators
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Code).NotEmpty().WithMessage("This field is required");
            RuleFor(p => p.Name).NotEmpty().WithMessage("This field is required");
            RuleFor(p => p.CategoryId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.CodeStatus).NotNull().WithMessage("This field is required");
            RuleFor(p => p.InitialQuantity).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Cost).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Price).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Discount).LessThanOrEqualTo(p => p.Price).WithMessage("Discount should be less than or equal to price");
        }
    }
}
