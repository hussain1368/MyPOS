using System;
using System.Threading.Tasks;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.ViewModels
{
    public class LoginVM : BaseBindable, ICloseWindow
    {
        public LoginVM(AppState appState, MainVM mainVM)
        {
            this.appState = appState;
            this.mainVM = mainVM;
            LoginCmd = new CommandAsync(Login);
        }

        private readonly AppState appState;
        private readonly MainVM mainVM;

        public CommandAsync LoginCmd { get; set; }

        private LoginEM _user = new LoginEM();
        public LoginEM User
        {
            get { return _user; }
            set { SetValue(ref _user, value); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetValue(ref _errorMessage, value); }
        }

        private async Task Login()
        {
            try
            {
                ErrorMessage = null;
                User.ValidateModel();
                if (User.HasErrors) return;
                await appState.Login(User.Username, User.Password);
                var mainWindow = new MainWindow(mainVM);
                mainWindow.Show();
                Close?.Invoke();
            }
            catch(ApplicationException e)
            {
                ErrorMessage = e.Message;
            }
        }

        public Action Close { get; set; }
    }
}
