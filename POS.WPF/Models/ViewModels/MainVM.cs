using Microsoft.Extensions.DependencyInjection;
using POS.WPF.Commands;
using System;

namespace POS.WPF.Models.ViewModels
{
    public class MainVM : BaseBindable
    {
        public MainVM(IServiceProvider services, AppState appState)
        {
            Pages = new BaseBindable[]
            {
                services.GetRequiredService<HomeVM>(),
                services.GetRequiredService<ProductsVM>(),
                services.GetRequiredService<AccountsVM>(),
                services.GetRequiredService<InvoicesVM>(),
            };

            BodyContent = Pages[0];
            ViewChangedCmd = new RelayCommandSyncVoid(() => BodyContent = Pages[SelectedIndex]);
            this.appState = appState;
        }

        private readonly AppState appState;
        private readonly BaseBindable[] Pages;
        public RelayCommandSyncVoid ViewChangedCmd { get; set; }

        private BaseBindable _bodyContent;
        public BaseBindable BodyContent
        {
            get { return _bodyContent; }
            set { _bodyContent = value; OnPropertyChanged(); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; OnPropertyChanged(); }
        }

        public string UserDisplayName => appState.CurrentUser?.DisplayName;
    }
}
