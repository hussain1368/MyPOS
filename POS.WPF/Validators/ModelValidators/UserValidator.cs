using FluentValidation;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Validators.ModelValidators
{
    public class UserValidator : AbstractValidator<UserEM>
    {
        public UserValidator()
        {
            RuleFor(m => m.UserRole)
                .NotNull()
                .WithMessage("This field is required");

            RuleFor(m => m.DisplayName)
                .NotNull()
                .WithMessage("This field is required");

            RuleFor(m => m.Username)
                .NotNull()
                .WithMessage("This field is required")
                .Matches("^[A-Za-z0-9_]{4,16}$")
                .WithMessage("Please enter a valid username");

            When(m => m.Id == 0, () =>
            {
                RuleFor(m => m.Password)
                    .NotEmpty()
                    .WithMessage("This field is mandatory");
            });

            When(m => !string.IsNullOrEmpty(m.Password), () =>
            {
                RuleFor(m => m.Password)
                    .Matches("^[!-~]{8,64}$")
                    .WithMessage("Please enter a valid password");
            });
        }
    }
}
