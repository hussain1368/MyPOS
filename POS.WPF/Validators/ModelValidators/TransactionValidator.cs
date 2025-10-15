using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class TransactionValidator : AbstractValidator<TransactionEM>
    {
        public TransactionValidator()
        {
            //RuleFor(p => p.AccountTypeId).NotNull().WithMessage("This field is required");
            //RuleFor(p => p.CurrencyId).NotNull().WithMessage("This field is required");
            //RuleFor(p => p.Name).NotEmpty().WithMessage("This field is required");
        }
    }
}
