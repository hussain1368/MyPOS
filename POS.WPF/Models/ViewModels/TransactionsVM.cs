using AutoMapper;
using MaterialDesignThemes.Wpf;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Sections;
using POS.WPF.Views.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class TransactionsVM : BaseBindable
    {
        public TransactionsVM(ITransactionRepository transactionRepo, IPartnerRepository partnerRepo, IOptionRepository optionRepo, IMapper mapper, AppState appState)
        {
            _transactionRepo = transactionRepo;
            _partnerRepo = partnerRepo;
            _optionRepo = optionRepo;
            _mapper = mapper;
            _appState = appState;

            LoadOptionsCmd = new CommandAsync(LoadOptions);
            LoadListCmd = new CommandAsync(LoadList);
            SetPartnerNameCmd = new CommandSync(SetPartnerName);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            SaveCmd = new CommandAsync(SaveForm);
            CancelCmd = new CommandSync(CancelForm);
            CheckAllCmd = new CommandParam(CheckAll);
            DeleteCmd = new CommandAsync(DeleteRows);
        }

        private readonly AppState _appState;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IPartnerRepository _partnerRepo;
        private readonly IOptionRepository _optionRepo;
        private readonly IMapper _mapper;

        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandAsync LoadListCmd { get; set; }
        public CommandSync SetPartnerNameCmd { get; set; }
        public CommandAsyncParam ShowFormCmd { get; set; }
        public CommandAsync SaveCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }
        public CommandAsync DeleteCmd { get; set; }

        public HeaderBarVM Header => new HeaderBarVM
        {
            HeaderText = "Transactions List",
            IconKind = PackIconKind.Add,
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
                OnPropertyChanged(nameof(SourceList));
            }
        }

        public IList<OptionValueDTO> CurrencyList => ComboOptions?.Where(op => op.TypeCode == "CRC").ToList();
        public IList<OptionValueDTO> SourceList => ComboOptions?.Where(op => op.TypeCode == "TSR").ToList();

        private IEnumerable<PartnerDTO> _partnersList;
        public IEnumerable<PartnerDTO> PartnersList
        {
            get => _partnersList;
            set => SetValue(ref _partnersList, value);
        }

        private IEnumerable<WalletDTO> _walletsList;
        public IEnumerable<WalletDTO> WalletsList
        {
            get => _walletsList;
            set => SetValue(ref _walletsList, value);
        }

        private TransactionEM _currentTransaction = new TransactionEM();
        public TransactionEM CurrentTransaction
        {
            get { return _currentTransaction; }
            set { SetValue(ref _currentTransaction, value); }
        }

        private ObservableCollection<TransactionEM> _transactionList;
        public ObservableCollection<TransactionEM> TransactionList
        {
            get { return _transactionList; }
            set { SetValue(ref _transactionList, value); }
        }

        private int? _partnerId;
        public int? PartnerId
        {
            get { return _partnerId; }
            set { SetValue(ref _partnerId, value); }
        }

        private byte? _transactionType;
        public byte? TransactionType
        {
            get { return _transactionType; }
            set { SetValue(ref _transactionType, value); }
        }

        private int? _sourceId;
        public int? SourceId
        {
            get { return _sourceId; }
            set { SetValue(ref _sourceId, value); }
        }

        private DateTime? _searchDate;
        public DateTime? SearchDate
        {
            get { return _searchDate; }
            set { SetValue(ref _searchDate, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetValue(ref _isLoading, value); }
        }

        private async Task LoadOptions()
        {
            ComboOptions = await _optionRepo.OptionsAll();

            WalletsList = await _optionRepo.GetWalletsList();
            PartnersList = await _partnerRepo.GetList();
        }

        private async Task LoadList()
        {
            await DialogHost.Show(new LoadingDialog(), "TransactionsDH", async (sender, args) =>
            {
                await GetList();
                args.Session.Close(false);
            },
            null);
        }

        private async Task GetList()
        {
            var data = await _transactionRepo.GetList(TransactionType, PartnerId, SourceId, SearchDate, PageIndex);
            var _data = _mapper.Map<IEnumerable<TransactionEM>>(data.Transactions);
            TransactionList = new ObservableCollection<TransactionEM>(_data);
            PageCount = data.PageCount;
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => SetValue(ref _pageIndex, value);
        }

        private int _pageCount = 0;
        public int PageCount
        {
            get => _pageCount;
            set => SetValue(ref _pageCount, value);
        }

        private async Task ShowForm(object id)
        {
            CurrentTransaction = new TransactionEM();
            await DialogHost.Show(new TransactionForm(), "TransactionsDH", async (sender, args)=>
            {
                if (id != null)
                {
                    IsLoading = true;
                    var obj = await _transactionRepo.GetById((int)id);
                    CurrentTransaction = _mapper.Map<TransactionEM>(obj);
                    IsLoading = false;
                }
            },
            null);
        }

        private void CancelForm()
        {
            CurrentTransaction = new TransactionEM();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private async Task SaveForm()
        {
            CurrentTransaction.ValidateModel();
            if (CurrentTransaction.HasErrors) return;

            IsLoading = true;

            var data = _mapper.Map<TransactionDTO>(CurrentTransaction);

            data.IsDeleted = false;
            data.UpdatedBy = _appState.CurrentUserId;
            data.UpdatedDate = DateTime.Now;

            if (CurrentTransaction.Id == 0)
            {
                await _transactionRepo.Create(data);
                CurrentTransaction = new TransactionEM();
            }
            else
            {
                await _transactionRepo.Update(data);
            }
            await GetList();
            IsLoading = false;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CheckAll(object isChecked)
        {
            bool _isChecked = (bool)isChecked;
            foreach (var obj in TransactionList) obj.IsChecked = _isChecked;
        }

        private void SetPartnerName()
        {
            var id = CurrentTransaction.PartnerId;
            if (id.HasValue)
            {
                var partner = PartnersList.FirstOrDefault(a => a.Id == id);
                if (partner != null)
                {
                    CurrentTransaction.PartnerNameReadOnly = true;
                    CurrentTransaction.PartnerName = partner.Name;
                    return;
                }
            }
            CurrentTransaction.PartnerNameReadOnly = false;
            CurrentTransaction.PartnerName = string.Empty;
        }

        private async Task DeleteRows()
        {
            var ids = TransactionList.Where(m => m.IsChecked).Select(m => m.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new MyDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "TransactionsDH", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await _transactionRepo.Delete(ids);
                await GetList();
                args.Session.Close(false);
            });
        }
    }
}
