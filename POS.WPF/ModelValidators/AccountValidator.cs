using FluentValidation;
using POS.WPF.Models;

namespace POS.WPF.ModelValidators
{
    public class AccountValidator : AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(p => p.AccountTypeId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.CurrencyId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Name).NotEmpty().WithMessage("This field is required");
        }
    }
}
