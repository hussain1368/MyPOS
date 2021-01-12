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
        }
    }
}
