﻿using POS.WPF.Commands;
using POS.WPF.Models;
using System;
using System.Threading.Tasks;

namespace POS.WPF.ViewModels
{
    public class LoginVM : BaseBindable, ICloseWindow
    {
        public LoginVM(AppState appState, MainVM mainVM)
        {
            this.appState = appState;
            this.mainVM = mainVM;
            LoginCmd = new RelayCommandAsync(Login);
        }

        private readonly AppState appState;
        private readonly MainVM mainVM;

        public RelayCommandAsync LoginCmd { get; set; }

        private LoginModel _user = new LoginModel();
        public LoginModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
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
