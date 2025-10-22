using FluentValidation;
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
            .WithMessage("Product code is duplicated!");

            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("This field is required");

            RuleFor(p => p.CategoryId)
                .NotNull()
                .WithMessage("This field is required");

            RuleFor(p => p.InitialQuantity)
                .NotEmpty()
                .WithMessage("This field is required")
                .Must(val => int.TryParse(val, out var _val) && _val >= 0)
                .WithMessage("Please enter a valid number");

            RuleFor(p => p.Cost)
                .NotEmpty()
                .WithMessage("This field is required")
                .Must(val => int.TryParse(val, out var _val) && _val >= 0)
                .WithMessage("Please enter a valid number");

            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage("This field is required")
                .Must(val => int.TryParse(val, out var _val) && _val >= 0)
                .WithMessage("Please enter a valid number");

            When(p => !string.IsNullOrEmpty(p.Discount), () =>
            {
                RuleFor(p => p.Discount)
                    .Must(val => int.TryParse(val, out var _val) && _val >= 0)
                    .WithMessage("Please enter a valid number")
                    .Must((obj, val) =>
                    {
                        if (int.TryParse(val, out int _val) && int.TryParse(obj.Price, out int _price))
                        {
                            return _val <= _price;
                        }
                        return true;
                    })
                    .WithMessage("Discount should be less than or equal to price");
            });

            When(p => !string.IsNullOrEmpty(p.AlertQuantity), () =>
            {
                RuleFor(p => p.AlertQuantity)
                    .Must(val => int.TryParse(val, out var _val) && _val >= 0)
                    .WithMessage("Please enter a valid number");
            });
        }
    }
}
