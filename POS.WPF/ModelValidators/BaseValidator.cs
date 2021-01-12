using FluentValidation;
using FluentValidation.Results;
using POS.WPF.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace POS.WPF.ModelValidators
{
    public class BaseValidator<BaseViewModel>
    {
        private ValidationResult results = new ValidationResult();
        private readonly BaseViewModel model;
        private readonly AbstractValidator<BaseViewModel> validator;

        public BaseValidator(BaseViewModel model, AbstractValidator<BaseViewModel> validator)
        {
            this.model = model;
            this.validator = validator;
        }

        public IList<string> GetErrors(string propertyName)
        {
            return results.Errors.Where(er => er.PropertyName == propertyName).Select(er => er.ErrorMessage).ToList();
        }

        public void ValidateProp([CallerMemberName] string propName = "")
        {
            results = validator.Validate(model, options => options.IncludeProperties(propName));
            //OnErrorsChanged(propName);
            //OnPropertyChanged(nameof(IsValid));
        }

        public string[] ValidateAll()
        {
            results = validator.Validate(model);
            return results.Errors.Select(er => er.PropertyName).ToArray();
            //foreach (var prop in results.Errors.Select(er => er.PropertyName))
            //{
            //    OnErrorsChanged(prop);
            //}
            //OnPropertyChanged(nameof(IsValid));
        }

        public bool AnyErrors => results.IsValid;
    }
}
