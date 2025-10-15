using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class CurrencyRateValidator : AbstractValidator<CurrencyRateEM>
    {
        public CurrencyRateValidator()
        {
            RuleFor(e => e.CurrencyId).NotNull().WithMessage("This field is mandatory");
            RuleFor(e => e.RateDate).NotNull().WithMessage("This field is mandatory");
            RuleFor(e => e.BaseValue).NotNull().WithMessage("This field is mandatory");
            RuleFor(e => e.Rate).NotNull().WithMessage("This field is mandatory");
        }
    }
}