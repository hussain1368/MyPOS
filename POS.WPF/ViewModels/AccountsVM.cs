using MaterialDesignThemes.Wpf;
using POS.DAL.Models;
using POS.DAL.Query;
using POS.WPF.Commands;
using POS.WPF.Controls;
using POS.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.ViewModels
{
    public class AccountsVM : BaseVM
    {
        public AccountsVM(AccountQuery accountQuery, OptionQuery optionQuery)
        {
            this.accountQuery = accountQuery;
            this.optionQuery = optionQuery;

            LoadOptionsCmd = new RelayCommandAsync(doLoadOptions);
            LoadListCmd = new RelayCommandAsync(doLoadList);
            ShowFormCmd = new RelayCommandAsyncParam(doShowForm);
            SaveCmd = new RelayCommandAsync(doSave);
            CancelCmd = new RelayCommandSyncVoid(() =>
            {
                CurrentAccount = new AccountModel();
                DialogHost.CloseDialogCommand.Execute(null, null);
            });
            CheckAllCmd = new RelayCommandSyncParam(isChecked =>
            {
                bool _isChecked = (bool)isChecked;
                foreach (var obj in AccountsList) obj.IsChecked = _isChecked;
            });
        }

        private readonly AccountQuery accountQuery;
        private readonly OptionQuery optionQuery;

        public RelayCommandAsync LoadOptionsCmd { get; set; }
        public RelayCommandAsync LoadListCmd { get; set; }
        public RelayCommandAsyncParam ShowFormCmd { get; set; }
        public RelayCommandAsync SaveCmd { get; set; }
        public RelayCommandSyncVoid CancelCmd { get; set; }
        public RelayCommandSyncParam CheckAllCmd { get; set; }
        public RelayCommandAsync DeleteCmd { get; set; }

        public HeaderBarVM Header => new HeaderBarVM
        {
            HeaderText = "List of Accounts",
            IconKind = "Add",
            ButtonCommand = new RelayCommandAsyncParam(doShowForm)
        };

        private IList<OptionValueDTM> _comboOptions;
        public IList<OptionValueDTM> ComboOptions
        {
            get { return _comboOptions; }
            set
            {
                _comboOptions = value;
                OnPropertyChanged(nameof(CurrencyList));
                OnPropertyChanged(nameof(AccountTypeList));
            }
        }

        public IList<OptionValueDTM> CurrencyList => ComboOptions?.Where(op => op.TypeCode == "CRC").ToList();
        public IList<OptionValueDTM> AccountTypeList => ComboOptions?.Where(op => op.TypeCode == "ATYP").ToList();

        private AccountModel _currentAccount = new AccountModel();
        public AccountModel CurrentAccount
        {
            get { return _currentAccount; }
            set { _currentAccount = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AccountModel> _accountsList;
        public ObservableCollection<AccountModel> AccountsList
        {
            get { return _accountsList; }
            set { _accountsList = value; OnPropertyChanged(); }
        }

        private bool _isFormOpen;
        public bool IsFormOpen
        {
            get { return _isFormOpen; }
            set { _isFormOpen = value; OnPropertyChanged(); }
        }

        private async Task doLoadOptions()
        {
            ComboOptions = await optionQuery.OptionsAll();
        }

        private async Task doLoadList()
        {
            IsFormOpen = true;
            var data = await accountQuery.GetList();
            var _data = data.Select(m => new AccountModel
            {
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
                Address = m.Address,
                Note = m.Note,
                CurrencyId = m.CurrencyId,
                CurrentBalance = m.CurrentBalance,
                AccountTypeId = m.AccountTypeId,
                AccountTypeName = m.AccountTypeName,
                CurrencyName = m.CurrencyName,
                CurrencyCode = m.CurrencyCode,
            });
            AccountsList = new ObservableCollection<AccountModel>(_data);
            IsFormOpen = false;
        }

        private async Task doShowForm(object id)
        {
            if (id == null) CurrentAccount = new AccountModel();
            else
            {
                var obj = await accountQuery.GetById((int)id);
                CurrentAccount = new AccountModel
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Phone = obj.Phone,
                    Address = obj.Address,
                    Note = obj.Note,
                    CurrencyId = obj.CurrencyId,
                    CurrentBalance = obj.CurrentBalance,
                    AccountTypeId = obj.AccountTypeId,
                };
            }
            await DialogHost.Show(new AccountForm(), "MainDialogHost", null, null);
        }

        private async Task doSave()
        {
            CurrentAccount.ValidateModel();
            if (CurrentAccount.HasErrors) return;
            
            var data = new AccountDTM
            {
                Id = CurrentAccount.Id,
                Name = CurrentAccount.Name,
                Phone = CurrentAccount.Phone,
                Address = CurrentAccount.Address,
                Note = CurrentAccount.Note,
                CurrencyId = CurrentAccount.CurrencyId.Value,
                AccountTypeId = CurrentAccount.AccountTypeId.Value,
                CurrentBalance = CurrentAccount.CurrentBalance,
                IsDeleted = false,
            };

            if (CurrentAccount.Id == 0)
            {
                data.InsertedBy = 1;
                data.InsertedDate = DateTime.Now;
                await accountQuery.Create(data);
                CurrentAccount = new AccountModel();
            }
            else
            {
                data.UpdatedBy = 1;
                data.UpdatedDate = DateTime.Now;
                await accountQuery.Update(data);
            }

            LoadListCmd.Execute(null);
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
