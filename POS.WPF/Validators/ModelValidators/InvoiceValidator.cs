using FluentValidation;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class InvoiceValidator : AbstractValidator<InvoiceEM>
    {
        public InvoiceValidator()
        {
            RuleFor(m => m.Wallet)
                .NotNull()
                .WithMessage("Wallet is required");

            RuleFor(m => m.WarehouseId)
                .NotNull()
                .WithMessage("Warehouse is required");

            RuleFor(m => m.IssueDate)
                .NotNull()
                .WithMessage("Issue date is required");

            RuleFor(m => m.CurrencyRate)
                .NotEmpty()
                .WithMessage("Currency rate is required")
                .Must(val => double.TryParse(val, out double _val) && _val > 0)
                .WithMessage("Please input a valid number");

            RuleFor(m => m.PaymentType)
                .NotEqual(PaymentType.None)
                .WithMessage("Payment type is required");

            RuleFor(m => m.AmountPaid)
                .NotEmpty()
                .WithMessage("This field is required")
                .Must(val => double.TryParse(val, out double _val) && _val >= 0)
                .WithMessage("Please input a valid number");

            When(p => !string.IsNullOrEmpty(p.OverallDiscount), () =>
            {
                RuleFor(p => p.OverallDiscount)
                    .Must(val => double.TryParse(val, out var _val) && _val >= 0)
                    .WithMessage("Please enter a valid number")
                    .Must((obj, val) =>
                    {
                        // Check if overall discount is not more than total price
                        return true;
                    })
                    .WithMessage("Discount should not be more total price");
            });
        }
    }
}
