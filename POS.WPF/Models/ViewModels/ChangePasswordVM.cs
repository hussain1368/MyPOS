using MaterialDesignThemes.Wpf;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Validators.ModelValidators;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace POS.WPF.Models.ViewModels
{
    public class ChangePasswordVM : BaseBindable
    {
        public ChangePasswordVM(AppState appState, IUserRepository userRepo) 
        {
            _appState = appState;
            _userRepo = userRepo;

            SaveCmd = new CommandAsync(Save);
            CancelCmd = new CommandSync(Cancel);

            ToggleCurrentPassVisibleCmd = new CommandSync(() => IsCurrentPassVisible = !IsCurrentPassVisible);
            ToggleNewPassVisibleCmd = new CommandSync(() => IsNewPassVisible = !IsNewPassVisible);
        }

        private readonly AppState _appState;
        private readonly IUserRepository _userRepo;

        public CommandAsync SaveCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandSync ToggleCurrentPassVisibleCmd { get; set; }
        public CommandSync ToggleNewPassVisibleCmd { get; set; }

        private bool _isCurrentPassVisible;
        private bool IsCurrentPassVisible
        {
            get => _isCurrentPassVisible;
            set
            {
                SetValue(ref _isCurrentPassVisible, value);
                OnPropertyChanged(nameof(CurrentPassTextBoxVisible));
                OnPropertyChanged(nameof(CurrentPassPassBoxVisible));
                OnPropertyChanged(nameof(CurrentPassIcon));
            }
        }

        public Visibility CurrentPassTextBoxVisible => IsCurrentPassVisible ? Visibility.Visible : Visibility.Hidden;
        public Visibility CurrentPassPassBoxVisible => IsCurrentPassVisible ? Visibility.Hidden : Visibility.Visible;
        public PackIconKind CurrentPassIcon => IsCurrentPassVisible ? PackIconKind.EyeOff : PackIconKind.Eye;

        private bool _isNewPassVisible;
        private bool IsNewPassVisible
        {
            get => _isNewPassVisible;
            set
            {
                SetValue(ref _isNewPassVisible, value);

                OnPropertyChanged(nameof(NewPassTextBoxVisible));
                OnPropertyChanged(nameof(NewPassPassBoxVisible));
                OnPropertyChanged(nameof(NewPassIcon));
            }
        }

        public Visibility NewPassTextBoxVisible => IsNewPassVisible ? Visibility.Visible : Visibility.Hidden;
        public Visibility NewPassPassBoxVisible => IsNewPassVisible ? Visibility.Hidden : Visibility.Visible;
        public PackIconKind NewPassIcon => IsNewPassVisible ? PackIconKind.EyeOff : PackIconKind.Eye;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetValue(ref _isLoading, value);
        }

        private ChangePasswordEM _model = new ChangePasswordEM();
        public ChangePasswordEM Model
        {
            get => _model;
            set => SetValue(ref _model, value);
        }

        private async Task Save()
        {
            Model.ValidateModel();
            if (Model.HasErrors) return;

            IsLoading = true;
            try
            {
                await _userRepo.UpdatePassword(_appState.CurrentUserId, Model.CurrentPassword, Model.NewPassword);
            }
            catch (ApplicationException ex)
            {
                Model.AddManualError(nameof(Model.CurrentPassword), ex.Message);
                IsLoading = false;
                return;
            }
            DialogHost.CloseDialogCommand.Execute(null, null);
            Model = new ChangePasswordEM();
            IsCurrentPassVisible = false;
            IsNewPassVisible = false;
            IsLoading = false;
        }

        private void Cancel()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
            Model = new ChangePasswordEM();
            IsCurrentPassVisible = false;
            IsNewPassVisible = false;
        }
    }

    public class  ChangePasswordEM : BaseErrorBindable<ChangePasswordEM>
    {
        public ChangePasswordEM() : base(new ChangePasswordValidator()) { }

        private string _currentPassword;
        public string CurrentPassword
        {
            get { return _currentPassword; }
            set { SetAndValidate(ref _currentPassword, value); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetAndValidate(ref _newPassword, value); }
        }
    }
}
