using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class ProductValidator : AbstractValidator<ProductEM>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Code).NotEmpty().WithMessage("This field is required");
            RuleFor(p => p.Name).NotEmpty().WithMessage("This field is required");
            RuleFor(p => p.CategoryId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.InitialQuantity).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Cost).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Price).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Discount).LessThanOrEqualTo(p => p.Price).WithMessage("Discount should be less than or equal to price");
        }
    }
}
