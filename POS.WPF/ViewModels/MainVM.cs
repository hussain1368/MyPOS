using Microsoft.Extensions.DependencyInjection;
using POS.WPF.Commands;
using System;

namespace POS.WPF.ViewModels
{
    public class MainVM : BaseVM
    {
        public MainVM(IServiceProvider services)
        {
            Pages = new BaseVM[]
            {
                services.GetRequiredService<HomeVM>(),
                services.GetRequiredService<ProductsVM>(),
                services.GetRequiredService<AccountsVM>(),
            };

            BodyContent = Pages[0];
            ViewChangedCmd = new RelayCommandSyncVoid(() => BodyContent = Pages[SelectedIndex]);
        }

        private BaseVM[] Pages;
        public RelayCommandSyncVoid ViewChangedCmd { get; set; }

        private BaseVM _bodyContent;
        public BaseVM BodyContent
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
    }
}
