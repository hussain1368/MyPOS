using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class PartnerValidator : AbstractValidator<PartnerEM>
    {
        public PartnerValidator()
        {
            RuleFor(p => p.PartnerTypeId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.CurrencyId).NotNull().WithMessage("This field is required");
            RuleFor(p => p.Name).NotEmpty().WithMessage("This field is required");
        }
    }
}
