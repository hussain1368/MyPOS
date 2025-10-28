using AutoMapper;
using MaterialDesignThemes.Wpf;
using POS.DAL.DTO;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Common;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS.WPF.Models.ViewModels
{
    public class InvoiceFormVM : BaseBindable
    {
        public InvoiceFormVM(
            IProductRepository productRepo,
            IOptionRepository optionRepo,
            IPartnerRepository partnerRepo,
            IInvoiceRepository invoiceRepo,
            IMapper mapper,
            AppState appState)
        {
            _productRepo = productRepo;
            _optionRepo = optionRepo;
            _partnerRepo = partnerRepo;
            _invoiceRepo = invoiceRepo;
            _mapper = mapper;
            _appState = appState;

            LoadOptionsCmd = new CommandAsync(LoadOptions);
            FindByNameInputCmd = new CommandAsync(FindByName);
            FindByNameKeyUpCmd = new CommandAsyncParam(FindByNameKeyUp);
            AddInvoiceItemCmd = new CommandSync(() => AddInvoiceItem(SelectedProduct));
            DeleteInvoiceItemCmd = new CommandParam(DeleteInvoiceItem);
            IncrementInvoiceItemCmd = new CommandParam(IncrementInvoiceItem);
            DecrementInvoiceItemCmd = new CommandParam(DecrementInvoiceItem);
            SaveCmd = new CommandAsync(Save);
            CancelCmd = new CommandSync(Cancel);

            SetPartnerNameCmd = new CommandSync(() =>
            {
                if (CurrentInvoice.PartnerId != null)
                {
                    CurrentInvoice.PartnerName = PartnersList.FirstOrDefault(p => p.Id == CurrentInvoice.PartnerId)?.Name;
                }
                else
                {
                    CurrentInvoice.PartnerName = string.Empty;
                }
                OnPropertyChanged(nameof(PartnerNameReadOnly));
            });
        }

        private readonly AppState _appState;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;
        private readonly IOptionRepository _optionRepo;
        private readonly IPartnerRepository _partnerRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        public InvoicesVM ParentPage { get; set; }

        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandAsync FindByNameInputCmd { get; set; }
        public CommandAsyncParam FindByNameKeyUpCmd { get; set; }
        public CommandSync AddInvoiceItemCmd { get; set; }
        public CommandParam DeleteInvoiceItemCmd { get; set; }
        public CommandParam IncrementInvoiceItemCmd { get; set; }
        public CommandParam DecrementInvoiceItemCmd { get; set; }
        public CommandSync SetPartnerNameCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandAsync SaveCmd { get; set; }

        public bool PartnerNameReadOnly => CurrentInvoice.PartnerId != null;

        private InvoiceType _invoiceType = InvoiceType.Sale;
        public InvoiceType InvoiceType
        {
            get => _invoiceType;
            set
            {
                SetValue(ref _invoiceType, value);
                OnPropertyChanged(nameof(FormTitle));
            }
        }

        public string FormTitle => InvoiceType switch
        {
            InvoiceType.Sale => "New Sale Invoice",
            InvoiceType.Purchase => "New Purchase Invoice",
            InvoiceType.Return => "New Return Invoice",
            _ => null,
        };

        private IEnumerable<ProductItemDTO> _productsList = Enumerable.Empty<ProductItemDTO>();
        public IEnumerable<ProductItemDTO> ProductsList
        {
            get => _productsList;
            set => SetValue(ref _productsList, value);
        }

        private ObservableCollection<InvoiceItemEM> _invoiceItems = new ObservableCollection<InvoiceItemEM>();
        public ObservableCollection<InvoiceItemEM> InvoiceItems
        {
            get => _invoiceItems;
            set => SetValue(ref _invoiceItems, value);
        }

        public string InvoiceCurrencyCode => InvoiceItems.FirstOrDefault()?.CurrencyCode ?? "---";

        public int InvoiceTotalPrice => InvoiceItems.Sum(i => i.TotalPrice);

        private ProductItemDTO _selectedProduct;
        public ProductItemDTO SelectedProduct
        {
            get => _selectedProduct;
            set => SetValue(ref _selectedProduct, value);
        }

        private string _searchValue = string.Empty;
        public string SearchValue
        {
            get => _searchValue;
            set => SetValue(ref _searchValue, value);
        }

        private IEnumerable<WalletDTO> _walletsList;
        public IEnumerable<WalletDTO> WalletsList
        {
            get => _walletsList;
            set => SetValue(ref _walletsList, value);
        }

        private IEnumerable<WarehouseDTO> _warehousesList;
        public IEnumerable<WarehouseDTO> WarehousesList
        {
            get => _warehousesList;
            set => SetValue(ref _warehousesList, value);
        }
        private IEnumerable<PartnerDTO> _partnersList;
        public IEnumerable<PartnerDTO> PartnersList
        {
            get => _partnersList;
            set => SetValue(ref _partnersList, value);
        }

        private InvoiceEM _currentInvoice = new InvoiceEM();
        public InvoiceEM CurrentInvoice
        {
            get => _currentInvoice;
            set => SetValue(ref _currentInvoice, value);
        }

        private async Task LoadOptions()
        {
            WalletsList = await _optionRepo.GetWalletsList();
            WarehousesList = await _optionRepo.GetWarehousesList();
            PartnersList = await _partnerRepo.GetList();

            CurrentInvoice.Wallet = WalletsList.FirstOrDefault(t => t.IsDefault);
            CurrentInvoice.WarehouseId = WarehousesList.FirstOrDefault(t => t.IsDefault)?.Id;
        }

        private async Task FindByCode()
        {
            var product = await _productRepo.GetByCode(SearchValue);
            if (product != null) AddInvoiceItem(product);
        }

        private async Task FindByName()
        {
            if (!string.IsNullOrEmpty(SearchValue))
                ProductsList = await _productRepo.GetByName(SearchValue);
            else
                ProductsList = Enumerable.Empty<ProductItemDTO>();
        }

        private async Task FindByNameKeyUp(object e)
        {
            var _e = (KeyEventArgs)e;
            if (_e.Key == Key.Back || _e.Key == Key.Space) await FindByName();
            if (_e.Key == Key.Enter) await FindByCode();
        }

        private void AddInvoiceItem(ProductItemDTO product)
        {
            if (product == null) return;

            if (InvoiceItems.Count() != 0)
            {
                if (InvoiceItems.First().CurrencyId != product.CurrencyId)
                {
                    var message = "All products in the invoice must be of the same currency";
                    DialogHost.Show(new AlertDialog(new MyDialogVM { Message = message }), "FormDialogHost");
                    SearchValue = string.Empty;
                    ProductsList = Enumerable.Empty<ProductItemDTO>();
                    return;
                }
            }

            var productFound = InvoiceItems.FirstOrDefault(p => p.ProductId == product.Id);
            if (productFound == null)
            {
                InvoiceItems.Add(new InvoiceItemEM
                {
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    UnitDiscount = product.Discount,
                    CurrencyId = product.CurrencyId,
                    CurrencyCode = product.CurrencyCode,
                    Quantity = 1,
                });
                UpdateItemsIndexes();
            }
            else productFound.Quantity++;

            OnPropertyChanged(nameof(InvoiceTotalPrice));
            OnPropertyChanged(nameof(InvoiceCurrencyCode));
            SearchValue = string.Empty;
            ProductsList = Enumerable.Empty<ProductItemDTO>();
        }

        private void DeleteInvoiceItem(object index)
        {
            int _index = (int)index - 1;
            InvoiceItems.RemoveAt(_index);
            UpdateItemsIndexes();
            OnPropertyChanged(nameof(InvoiceTotalPrice));
            OnPropertyChanged(nameof(InvoiceCurrencyCode));
        }

        private void IncrementInvoiceItem(object index)
        {
            int _index = (int)index - 1;
            InvoiceItems.ElementAt(_index).Quantity++;
            OnPropertyChanged(nameof(InvoiceTotalPrice));
        }

        private void DecrementInvoiceItem(object index)
        {
            int _index = (int)index - 1;
            InvoiceItems.ElementAt(_index).Quantity--;
            OnPropertyChanged(nameof(InvoiceTotalPrice));
        }

        private void UpdateItemsIndexes()
        {
            foreach (var item in InvoiceItems) item.Index = InvoiceItems.IndexOf(item) + 1;
        }

        private async Task Save()
        {
            CurrentInvoice.ValidateModel();
            if (InvoiceItems.Count() == 0) return;
            if (CurrentInvoice.HasErrors) return;

            await DialogHost.Show(new LoadingDialog(), "FormDialogHost", async (send, args) =>
            {
                var data = _mapper.Map<InvoiceDTO>(CurrentInvoice);
                data.SerialNum = "INV00000345";
                data.InvoiceType = (byte)InvoiceType;
                data.UpdatedBy = _appState.CurrentUserId;
                data.UpdatedDate = DateTime.Now;
                data.CurrencyId = InvoiceItems.First().CurrencyId;

                data.Items = _mapper.Map<IList<InvoiceItemDTO>>(InvoiceItems);

                if (CurrentInvoice.Id == 0)
                {
                    await _invoiceRepo.Create(data);
                    Cancel();
                }
                else
                {
                    await _invoiceRepo.Update(data);
                    tempInvoiceData = data;
                }
                args.Session.Close(false);
            }, 
            null);
            await ParentPage.LoadList();
        }

        private void Cancel()
        {
            if (CurrentInvoice.Id == 0)
            {
                CurrentInvoice = new InvoiceEM
                {
                    Wallet = WalletsList.FirstOrDefault(t => t.IsDefault),
                    WarehouseId = WarehousesList.FirstOrDefault(t => t.IsDefault)?.Id
                };
                InvoiceItems.Clear();
            }
            else ResetInvoiceData();

            OnPropertyChanged(nameof(InvoiceTotalPrice));
            UpdateItemsIndexes();
        }

        public void ClearForm(InvoiceType invoiceType)
        {
            InvoiceType = invoiceType;
            CurrentInvoice.Id = 0;
            Cancel();
        }

        private InvoiceDTO tempInvoiceData;

        private void ResetInvoiceData()
        {
            CurrentInvoice = _mapper.Map<InvoiceEM>(tempInvoiceData);
            CurrentInvoice.Wallet = WalletsList.FirstOrDefault(t => t.Id == tempInvoiceData.WalletId);

            InvoiceType = (InvoiceType)tempInvoiceData.InvoiceType;

            var items = _mapper.Map<IList<InvoiceItemEM>>(tempInvoiceData.Items);
            InvoiceItems = new ObservableCollection<InvoiceItemEM>(items);
        }

        public async Task SetInvoiceData(int id)
        {
            await DialogHost.Show(new LoadingDialog(), "FormDialogHost", async (sender, args) =>
            {
                tempInvoiceData = await _invoiceRepo.GetById(id);
                ResetInvoiceData();
                args.Session.Close(false);
            },
            null);
            OnPropertyChanged(nameof(InvoiceTotalPrice));
            OnPropertyChanged(nameof(InvoiceCurrencyCode));
            UpdateItemsIndexes();
        }
    }
}
