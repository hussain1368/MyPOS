using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using System;
using System.Collections.ObjectModel;

namespace POS.WPF.Models.ViewModels
{
    public class MainVM : BaseBindable
    {
        public MainVM(IServiceProvider services, AppState appState, IStringLocalizer<Labels> t)
        {
            ViewChangedCmd = new CommandSync(() => BodyContent = MenuItems[SelectedIndex].ViewModel);
            this.appState = appState;
            _t = t;
            MenuItems = new ObservableCollection<MenuItemEM>()
            {
                new MenuItemEM
                {
                    Text = _t["Home"],
                    IconKind = PackIconKind.Home,
                    ViewModel = services.GetRequiredService<HomeVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Products"],
                    IconKind = PackIconKind.PackageVariant,
                    ViewModel = services.GetRequiredService<ProductsVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Accounts"],
                    IconKind = PackIconKind.BusinessCardOutline,
                    ViewModel = services.GetRequiredService<AccountsVM>()
                },
                new MenuItemEM
                {
                    Text = _t["SaleAndPurchase"],
                    IconKind = PackIconKind.Cart,
                    ViewModel = services.GetRequiredService<InvoicesVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Transactions"],
                    IconKind = PackIconKind.CashMultiple,
                    ViewModel = null
                },
                new MenuItemEM
                {
                    Text = _t["Reports"],
                    IconKind = PackIconKind.Poll,
                    ViewModel = null
                },
                new MenuItemEM
                {
                    Text = _t["Users"],
                    IconKind = PackIconKind.Account,
                    ViewModel = null
                },
                new MenuItemEM
                {
                    Text = _t["Settings"],
                    IconKind = PackIconKind.Settings,
                    ViewModel = null
                },
            };
            BodyContent = MenuItems[0].ViewModel;
            
        }

        private readonly AppState appState;
        private readonly IStringLocalizer<Labels> _t;

        public CommandSync ViewChangedCmd { get; set; }

        private BaseBindable _bodyContent;
        public BaseBindable BodyContent
        {
            get { return _bodyContent; }
            set { SetValue(ref _bodyContent, value); }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetValue(ref _selectedIndex, value); }
        }

        private ObservableCollection<MenuItemEM> _menuItems;
        public ObservableCollection<MenuItemEM> MenuItems
        {
            get { return _menuItems; }
            set { SetValue(ref _menuItems, value); }
        }

        public string UserDisplayName => appState.CurrentUser?.DisplayName;
    }
}
