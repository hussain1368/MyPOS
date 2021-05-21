using POS.WPF.Validators.ModelValidators;

namespace POS.WPF.Models.EntityModels
{
    public class LoginEM : BaseErrorBindable<LoginEM>
    {
        public LoginEM() : base(new LoginValidator()) { }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { SetAndValidate(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetAndValidate(ref _password, value); }
        }
    }
}
