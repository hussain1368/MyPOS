using FluentValidation;
using POS.DAL.Repository;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class ProductValidator : AbstractValidator<ProductEM>
    {
        public ProductValidator()
        {
            var query = ServiceLocator.Current.GetInstance<IProductRepository>();

            RuleFor(p => p.Code).MustAsync(async (model, code, token) =>
            {
                if (string.IsNullOrWhiteSpace(code)) return true;
                if (code.Length < 5) return false;
                var any = await query.CheckDuplicate(model.Id, code);
                return !any;
            })
            .WithMessage("Product code is invalid!");

            RuleFor(p => p.Name).NotEmpty().WithMessage("This field is required");
            RuleFor(p => p.CategoryId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.InitialQuantity).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Cost).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Price).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Discount).LessThanOrEqualTo(p => p.Price).WithMessage("Discount should be less than or equal to price");
        }

        
    }
}
