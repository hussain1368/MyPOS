using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class OptionValueValidator : AbstractValidator<OptionValueEM>
    {
        public OptionValueValidator()
        {
            RuleFor(p => p.TypeId)
                .NotNull()
                .WithMessage("This field is required");

            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("This field is required");
        }
    }
}
