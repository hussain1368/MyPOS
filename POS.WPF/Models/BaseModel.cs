﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;
using POS.WPF.ViewModels;

namespace POS.WPF.Models
{
    public class BaseModel<T> : BaseViewModel, INotifyDataErrorInfo where T : BaseViewModel
    {
        private readonly T model;
        private readonly AbstractValidator<T> validator;
        private ValidationResult valResult = new ValidationResult();
        public bool HasErrors => !valResult.IsValid;
        public bool IsValid => valResult.IsValid;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public BaseModel(AbstractValidator<T> validator)
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
            OnPropertyChanged(nameof(IsValid));
        }

        public void ValidateModel()
        {
            valResult = validator.Validate(model);
            foreach (var prop in valResult.Errors.Select(err => err.PropertyName))
            {
                OnErrorsChanged(prop);
            }
            OnPropertyChanged(nameof(IsValid));
        }

        public void OnErrorsChanged(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
        }
    }
}
