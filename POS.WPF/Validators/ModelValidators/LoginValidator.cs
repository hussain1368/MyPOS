using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class LoginValidator : AbstractValidator<LoginEM>
    {
        public LoginValidator()
        {
            RuleFor(p => p.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
