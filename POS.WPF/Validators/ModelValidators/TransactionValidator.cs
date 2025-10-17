using FluentValidation;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class TransactionValidator : AbstractValidator<TransactionEM>
    {
        public TransactionValidator()
        {
            RuleFor(p => p.Amount).NotEmpty().WithMessage("This field is required")
                .Must(val => int.TryParse(val, out int _val) && _val > 0)
                .WithMessage("Please enter a valid number");
            RuleFor(p => p.Date).NotNull().WithMessage("This field is required");
            RuleFor(p => p.PartnerName).NotEmpty().WithMessage("This field is required");
            RuleFor(p => p.CurrencyId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.CurrencyRate).NotEmpty().WithMessage("This field is required")
                .Must(val => double.TryParse(val, out double _val))
                .WithMessage("Please enter a valid number");
            RuleFor(p => p.TransactionType).NotEqual(TransactionType.None).WithMessage("This field is required");
            RuleFor(p => p.SourceId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.WalletId).NotNull().WithMessage("This field is required");
        }
    }
}
