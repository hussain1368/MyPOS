using FluentValidation;
using POS.WPF.Models;

namespace POS.WPF.ModelValidators
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(p => p.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
