﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using POS.DAL.DTO;
using POS.WPF.Commands;
using POS.WPF.Views.Sections;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Shared;
using POS.DAL.Repository.Abstraction;

namespace POS.WPF.Models.ViewModels
{
    public class AccountsVM : BaseBindable
    {
        public AccountsVM(IAccountRepository accountRepo, IOptionRepository optionRepo)
        {
            this.accountRepo = accountRepo;
            this.optionRepo = optionRepo;

            LoadOptionsCmd = new CommandAsync(LoadOptions);
            LoadListCmd = new CommandAsync(LoadList);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            SaveCmd = new CommandAsync(SaveForm);
            CancelCmd = new CommandSync(CancelForm);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteCmd = new CommandAsync(DeleteRows);
        }

        private readonly IAccountRepository accountRepo;
        private readonly IOptionRepository optionRepo;

        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandAsync LoadListCmd { get; set; }
        public CommandAsyncParam ShowFormCmd { get; set; }
        public CommandAsync SaveCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }
        public CommandAsync DeleteCmd { get; set; }

        public HeaderBarVM Header => new HeaderBarVM
        {
            HeaderText = "List of Accounts",
            IconKind = "Add",
            ButtonCmd = new CommandAsyncParam(ShowForm)
        };

        private IList<OptionValueDTO> _comboOptions;
        public IList<OptionValueDTO> ComboOptions
        {
            get { return _comboOptions; }
            set
            {
                _comboOptions = value;
                OnPropertyChanged(nameof(CurrencyList));
                OnPropertyChanged(nameof(AccountTypeList));
            }
        }

        public IList<OptionValueDTO> CurrencyList => ComboOptions?.Where(op => op.TypeCode == "CRC").ToList();
        public IList<OptionValueDTO> AccountTypeList => ComboOptions?.Where(op => op.TypeCode == "ATYP").ToList();

        private AccountEM _currentAccount = new AccountEM();
        public AccountEM CurrentAccount
        {
            get { return _currentAccount; }
            set { SetValue(ref _currentAccount, value); }
        }

        private ObservableCollection<AccountEM> _accountsList;
        public ObservableCollection<AccountEM> AccountsList
        {
            get { return _accountsList; }
            set { SetValue(ref _accountsList, value); }
        }

        private int? _accountTypeId;
        public int? AccountTypeId
        {
            get { return _accountTypeId; }
            set { SetValue(ref _accountTypeId, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetValue(ref _isLoading, value); }
        }

        private async Task LoadOptions()
        {
            ComboOptions = await optionRepo.OptionsAll();
        }

        private async Task LoadList()
        {
            await DialogHost.Show(new LoadingDialog(), "AccountsDH", async (sender, args) =>
            {
                await GetList();
                args.Session.Close(false);
            },
            null);
        }

        private async Task GetList()
        {
            var data = await accountRepo.GetList(AccountTypeId);
            var _data = data.Select(m => new AccountEM
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
            AccountsList = new ObservableCollection<AccountEM>(_data);
        }

        private async Task ShowForm(object id)
        {
            CurrentAccount = new AccountEM();
            await DialogHost.Show(new AccountForm(), "AccountsDH", async (sender, args)=>
            {
                if (id != null)
                {
                    IsLoading = true;
                    var obj = await accountRepo.GetById((int)id);
                    CurrentAccount = new AccountEM
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
                    IsLoading = false;
                }
            },
            null);
        }

        private void CancelForm()
        {
            CurrentAccount = new AccountEM();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task SaveForm()
        {
            CurrentAccount.ValidateModel();
            if (CurrentAccount.HasErrors) return;

            IsLoading = true;
            var data = new AccountDTO
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
                UpdatedBy = 1,
                UpdatedDate = DateTime.Now,
            };

            if (CurrentAccount.Id == 0)
            {
                await accountRepo.Create(data);
                CurrentAccount = new AccountEM();
            }
            else
            {
                await accountRepo.Update(data);
            }
            await GetList();
            IsLoading = false;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CheckAll(object isChecked)
        {
            bool _isChecked = (bool)isChecked;
            foreach (var obj in AccountsList) obj.IsChecked = _isChecked;
        }

        private async Task DeleteRows()
        {
            var ids = AccountsList.Where(m => m.IsChecked).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new ConfirmDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "AccountsDH", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await accountRepo.Delete(ids);
                await GetList();
                args.Session.Close(false);
            });
        }
    }
}
