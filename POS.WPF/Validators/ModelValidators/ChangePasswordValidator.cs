using FluentValidation;
using POS.WPF.Models.ViewModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordEM>
    {
        public ChangePasswordValidator()
        {
            RuleFor(p => p.CurrentPassword)
                .NotEmpty()
                .WithMessage("This field is required");

            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .WithMessage("This field is required")
                .Matches("^[!-~]{8,64}$")
                .WithMessage("Please enter a valid password");
        }
    }
}
