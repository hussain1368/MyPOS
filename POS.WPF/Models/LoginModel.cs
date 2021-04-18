using POS.WPF.ModelValidators;

namespace POS.WPF.Models
{
    public class LoginModel : BaseErrorBindable<LoginModel>
    {
        public LoginModel() : base(new LoginValidator()) { }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); ValidateField(); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); ValidateField(); }
        }
    }
}
