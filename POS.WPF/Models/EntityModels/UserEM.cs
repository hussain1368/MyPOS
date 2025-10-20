using POS.WPF.Validators.ModelValidators;

namespace POS.WPF.Models.EntityModels
{
    public class UserEM : BaseErrorBindable<UserEM>
    {
        public UserEM() : base(new UserValidator()) { }

        private int _id;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetAndValidate(ref _displayName, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetAndValidate(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetAndValidate(ref _password, value);
        }

        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set => SetAndValidate(ref _userRole, value);
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => SetValue(ref _isChecked, value);
        }

        public bool IsDeleted { get; set; }
    }
}
