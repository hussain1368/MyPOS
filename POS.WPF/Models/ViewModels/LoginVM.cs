using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Common.ControlHelper;
using POS.WPF.Models.EntityModels;

namespace POS.WPF.Models.ViewModels
{
    public class LoginVM : BaseBindable, ICloseWindow
    {
        public LoginVM(AppState appState, IServiceProvider services)
        {
            _appState = appState;
            _services = services;

            LoginCmd = new CommandAsync(Login);
            ResetCmd = new CommandSync(() =>
            {
                User = new LoginEM();
                ErrorMessage = null;
            });
        }

        private readonly AppState _appState;
        private readonly IServiceProvider _services;

        public CommandAsync LoginCmd { get; set; }
        public CommandSync ResetCmd { get; set; }

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
                await _appState.Login(User.Username, User.Password);

                var window = _services.GetRequiredService<MainWindow>();
                window.Show();

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
