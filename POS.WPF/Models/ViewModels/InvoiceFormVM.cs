using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using POS.DAL.DTO;
using POS.DAL.Query;
using POS.WPF.Commands;
using POS.WPF.Enums;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Layout;
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
            ProductQuery productQuery, 
            OptionQuery optionQuery, 
            AccountQuery accountQuery, 
            InvoiceQuery invoiceQuery)
        {
            this.productQuery = productQuery;
            this.optionQuery = optionQuery;
            this.accountQuery = accountQuery;
            this.invoiceQuery = invoiceQuery;
            ShowListCmd = new RelayCommandSyncVoid(() => { Transitioner.MovePreviousCommand.Execute(null, null); });
            LoadOptionsCmd = new RelayCommandAsync(LoadOptions);
            FindByNameInputCmd = new RelayCommandAsync(FindByName);
            FindByNameKeyUpCmd = new RelayCommandAsyncParam(FindByNameKeyUp);
            AddInvoiceItemCmd = new RelayCommandSyncVoid(() => AddInvoiceItem(SelectedProduct));
            DeleteInvoiceItemCmd = new RelayCommandSyncParam(DeleteInvoiceItem);
            IncrementInvoiceItemCmd = new RelayCommandSyncParam(IncrementInvoiceItem);
            DecrementInvoiceItemCmd = new RelayCommandSyncParam(DecrementInvoiceItem);
            SaveCmd = new RelayCommandAsync(Save);
            CancelCmd = new RelayCommandSyncVoid(Cancel);
        }
        private readonly ProductQuery productQuery;
        private readonly OptionQuery optionQuery;
        private readonly AccountQuery accountQuery;
        private readonly InvoiceQuery invoiceQuery;
        public InvoicesVM ParentPage { get; set; }

        public RelayCommandAsync LoadOptionsCmd { get; set; }
        public RelayCommandAsync FindByNameInputCmd { get; set; }
        public RelayCommandAsyncParam FindByNameKeyUpCmd { get; set; }
        public RelayCommandSyncVoid AddInvoiceItemCmd { get; set; }
        public RelayCommandSyncParam DeleteInvoiceItemCmd { get; set; }
        public RelayCommandSyncParam IncrementInvoiceItemCmd { get; set; }
        public RelayCommandSyncParam DecrementInvoiceItemCmd { get; set; }
        public RelayCommandSyncVoid ShowListCmd { get; set; }
        public RelayCommandSyncVoid CancelCmd { get; set; }
        public RelayCommandAsync SaveCmd { get; set; }

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

        private IEnumerable<TreasuryDTO> _treasuriesList;
        public IEnumerable<TreasuryDTO> TreasuriesList
        {
            get => _treasuriesList;
            set => SetValue(ref _treasuriesList, value);
        }

        private IEnumerable<WarehouseDTO> _warehousesList;
        public IEnumerable<WarehouseDTO> WarehousesList
        {
            get => _warehousesList;
            set => SetValue(ref _warehousesList, value);
        }
        private IEnumerable<AccountDTO> _accountsList;
        public IEnumerable<AccountDTO> AccountsList
        {
            get => _accountsList;
            set => SetValue(ref _accountsList, value);
        }

        private InvoiceEM _currentInvoice = new InvoiceEM();
        public InvoiceEM CurrentInvoice
        {
            get => _currentInvoice;
            set => SetValue(ref _currentInvoice, value);
        }

        private async Task LoadOptions()
        {
            TreasuriesList = await optionQuery.GetTreasuriesList();
            WarehousesList = await optionQuery.GetWarehousesList();
            AccountsList = await accountQuery.GetList();

            CurrentInvoice.Treasury = TreasuriesList.FirstOrDefault(t => t.IsDefault);
            CurrentInvoice.WarehouseId = WarehousesList.FirstOrDefault(t => t.IsDefault)?.Id;
        }

        private async Task FindByCode()
        {
            var product = await productQuery.GetByCode(SearchValue);
            if (product != null) AddInvoiceItem(product);
        }

        private async Task FindByName()
        {
            if (!string.IsNullOrEmpty(SearchValue))
                ProductsList = await productQuery.GetByName(SearchValue);
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
                    Quantity = 1,
                });
                UpdateItemsIndexes();
            }
            else productFound.Quantity++;

            OnPropertyChanged(nameof(InvoiceTotalPrice));
            SearchValue = string.Empty;
            ProductsList = Enumerable.Empty<ProductItemDTO>();
        }

        private void DeleteInvoiceItem(object index)
        {
            int _index = (int)index - 1;
            InvoiceItems.RemoveAt(_index);
            UpdateItemsIndexes();
            OnPropertyChanged(nameof(InvoiceTotalPrice));
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
                var data = new InvoiceDTO
                {
                    Id = CurrentInvoice.Id,
                    SerialNum = "INV00000345",
                    InvoiceType = (byte)InvoiceType,
                    WarehouseId = CurrentInvoice.WarehouseId.Value,
                    TreasuryId = CurrentInvoice.Treasury.Id,
                    AccountId = CurrentInvoice.AccountId,
                    CurrencyId = CurrentInvoice.Treasury.CurrencyId,
                    CurrencyRate = CurrentInvoice.CurrencyRate.Value,
                    IssueDate = CurrentInvoice.IssueDate.Value,
                    PaymentType = (byte)CurrentInvoice.PaymentType,
                    Note = CurrentInvoice.Note,
                    UpdatedBy = 1,
                    UpdatedDate = DateTime.Now,
                };

                data.Items = InvoiceItems.Select(i => new InvoiceItemDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductCode = i.ProductCode,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    Cost = 0,
                    Profit = 0,
                    UnitDiscount = i.UnitDiscount,
                    TotalDiscount = i.TotalDiscount,
                    Quantity = i.Quantity,
                })
                .ToList();

                if (CurrentInvoice.Id == 0)
                {
                    await invoiceQuery.Create(data);
                    Cancel();
                }
                else
                {
                    await invoiceQuery.Update(data);
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
                    Treasury = TreasuriesList.FirstOrDefault(t => t.IsDefault),
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
            CurrentInvoice = new InvoiceEM
            {
                Id = tempInvoiceData.Id,
                WarehouseId = tempInvoiceData.WarehouseId,
                Treasury = TreasuriesList.FirstOrDefault(t => t.Id == tempInvoiceData.TreasuryId),
                AccountId = tempInvoiceData.AccountId,
                CurrencyRate = tempInvoiceData.CurrencyRate,
                IssueDate = tempInvoiceData.IssueDate,
                PaymentType = (PaymentType)tempInvoiceData.PaymentType,
                Note = tempInvoiceData.Note,
            };
            InvoiceType = (InvoiceType)tempInvoiceData.InvoiceType;

            var items = tempInvoiceData.Items.Select(i => new InvoiceItemEM
            {
                ProductId = i.ProductId,
                ProductCode = i.ProductCode,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                UnitDiscount = i.UnitDiscount,
                Quantity = i.Quantity,
            })
            .ToList();
            InvoiceItems = new ObservableCollection<InvoiceItemEM>(items);
        }

        public async Task SetInvoiceData(int id)
        {
            await DialogHost.Show(new LoadingDialog(), "FormDialogHost", async (sender, args) =>
            {
                tempInvoiceData = await invoiceQuery.GetById(id);
                ResetInvoiceData();
                args.Session.Close(false);
            },
            null);
            OnPropertyChanged(nameof(InvoiceTotalPrice));
            UpdateItemsIndexes();
        }
    }
}
