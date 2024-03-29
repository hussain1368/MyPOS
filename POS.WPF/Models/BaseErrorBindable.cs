﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;

namespace POS.WPF.Models
{
    public class BaseErrorBindable<T> : BaseBindable, INotifyDataErrorInfo where T : BaseBindable
    {
        private readonly T model;
        private readonly AbstractValidator<T> validator;
        private ValidationResult valResult = new ValidationResult();

        public bool HasErrors => !valResult.IsValid;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public BaseErrorBindable(AbstractValidator<T> validator)
        {
            model = this as T;
            this.validator = validator;
        }

        public IEnumerable GetErrors(string propName)
        {
            return valResult.Errors.Where(er => er.PropertyName == propName).Select(err => err.ErrorMessage).ToList();
        }

        public void ValidateField([CallerMemberName] string propName = "")
        {
            valResult = validator.Validate(model, options => options.IncludeProperties(propName));
            OnErrorsChanged(propName);
            OnPropertyChanged(nameof(HasErrors));
        }

        protected void SetAndValidate<F>(ref F prop, F value, [CallerMemberName] string propName = "")
        {
            SetValue(ref prop, value, propName);
            ValidateField(propName);
        }

        public void ValidateModel()
        {
            valResult = validator.Validate(model);
            foreach (var prop in valResult.Errors.Select(err => err.PropertyName))
            {
                OnErrorsChanged(prop);
            }
            OnPropertyChanged(nameof(HasErrors));
        }

        public void OnErrorsChanged(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
        }
    }
}
