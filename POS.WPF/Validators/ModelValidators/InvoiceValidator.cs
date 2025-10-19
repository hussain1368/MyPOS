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
        }
    }
}
