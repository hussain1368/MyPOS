using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class CurrencyRateValidator : AbstractValidator<CurrencyRateEM>
    {
        public CurrencyRateValidator()
        {
            RuleFor(e => e.CurrencyId)
                .NotNull()
                .WithMessage("This field is mandatory");

            RuleFor(e => e.RateDate)
                .NotNull()
                .WithMessage("This field is mandatory");

            RuleFor(e => e.BaseValue)
                .NotEmpty()
                .WithMessage("This field is mandatory")
                .Must(val => double.TryParse(val, out double _val) && _val >= 0)
                .WithMessage("Please input a valid number");

            RuleFor(e => e.Rate)
                .NotEmpty()
                .WithMessage("This field is mandatory")
                .Must(val => double.TryParse(val, out double _val) && _val >= 0)
                .WithMessage("Please input a valid number");

            RuleFor(e => e.FinalRate)
                .NotNull()
                .WithMessage("This field is mandatory")
                .GreaterThan(0)
                .WithMessage("Final rate must be greater than zero");
        }
    }
}