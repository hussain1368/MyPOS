using FluentValidation;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class InvoiceValidator : AbstractValidator<InvoiceEM>
    {
        public InvoiceValidator()
        {
            RuleFor(m => m.Wallet).NotNull().WithMessage("Treasury is required");
            RuleFor(m => m.WarehouseId).NotNull().WithMessage("Warehouse is required");
            RuleFor(m => m.IssueDate).NotNull().WithMessage("Issue date is required");
            RuleFor(m => m.CurrencyRate).NotNull().WithMessage("Currency rate is required");
            RuleFor(m => m.PaymentType).NotEqual(PaymentType.None).WithMessage("Payment type is required");
        }
    }
}
