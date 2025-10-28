using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Sections;
using POS.WPF.Views.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class MainVM : BaseBindable, ICloseWindow
    {
        public MainVM(IServiceProvider services, AppState appState, IStringLocalizer<Labels> t)
        {
            _appState = appState;
            _services = services;
            _t = t;

            SetUpMenu();

            OpenPasswordFormCmd = new CommandAsync(OpenPasswordForm);
            LogoutCmd = new CommandAsync(Logout);
            ViewChangedCmd = new CommandSync(() =>
            {
                BodyContent.PageLeaving();
                BodyContent = MenuItems[SelectedIndex].ViewModel;
            });
        }

        private readonly IServiceProvider _services;

        private async Task OpenPasswordForm()
        {
            var passwordVM = _services.GetRequiredService<ChangePasswordVM>();
            var passwordWindow = new ChangePassword
            {
                DataContext = passwordVM
            };
            await DialogHost.Show(passwordWindow, "MainWindowDH");
        }

        private void SetUpMenu()
        {
            MenuItems = new ObservableCollection<MenuItemEM>()
            {
                new MenuItemEM
                {
                    Text = _t["Home"],
                    IconKind = PackIconKind.Home,
                    ViewModel = _services.GetRequiredService<HomeVM>()
                },
                new MenuItemEM
                {
                    Text = _t["SaleAndPurchase"],
                    IconKind = PackIconKind.Cart,
                    ViewModel = _services.GetRequiredService<InvoicesVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Transactions"],
                    IconKind = PackIconKind.CreditCardSyncOutline,
                    ViewModel = _services.GetRequiredService<TransactionsVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Products"],
                    IconKind = PackIconKind.PackageVariant,
                    ViewModel = _services.GetRequiredService<ProductsVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Partners"],
                    IconKind = PackIconKind.BusinessCardOutline,
                    ViewModel = _services.GetRequiredService<PartnersVM>()
                },
                //new MenuItemEM
                //{
                //    Text = _t["Reports"],
                //    IconKind = PackIconKind.Poll,
                //    ViewModel = _services.GetRequiredService<ReportsVM>()
                //},
                new MenuItemEM
                {
                    Text = _t["Users"],
                    IconKind = PackIconKind.Account,
                    ViewModel = _services.GetRequiredService<UsersVM>()
                },
                new MenuItemEM
                {
                    Text = _t["Settings"],
                    IconKind = PackIconKind.Settings,
                    ViewModel = _services.GetRequiredService<SettingsVM>()
                },
            };
            BodyContent = MenuItems[0].ViewModel;
        }

        private readonly AppState _appState;
        private readonly IStringLocalizer<Labels> _t;

        public CommandSync ViewChangedCmd { get; set; }
        public CommandAsync OpenPasswordFormCmd { get; set; }
        public CommandAsync LogoutCmd { get; set; }

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

        public string UserDisplayName => _appState.CurrentUser?.DisplayName;
        public string LayoutDirection => _appState.LayoutDirection;

        private async Task Logout()
        {
            var view = new ConfirmDialog(new MyDialogVM
            {
                Message = "Are you sure you want to log out?"
            });
            await DialogHost.Show(view, "MainWindowDH", null, (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;

                var loginWindow = _services.GetRequiredService<LoginWindow>();
                loginWindow.Show();

                _appState.Logout();
                Close?.Invoke();
            });
        }

        public Action Close { get; set; }
    }
}
