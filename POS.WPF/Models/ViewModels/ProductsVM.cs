using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using POS.DAL.Query;
using POS.DAL.DTO;
using POS.WPF.Commands;
using Microsoft.Extensions.Localization;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Shared;

namespace POS.WPF.Models.ViewModels
{
    public class ProductsVM : BaseBindable
    {
        public ProductsVM(ProductQuery productQuery, OptionQuery optionQuery, IStringLocalizer<Labels> _t)
        {
            this.productQuery = productQuery;
            this.optionQuery = optionQuery;
            this._t = _t;
            MsgContext = new MessageVM();

            LoadListCmd = new RelayCommandAsync(async () =>
            {
                await DialogHost.Show(new LoadingDialog(), "GridDialog", async (sender, args) =>
                {
                    await LoadList();
                    args.Session.Close(false);
                }, null);
            });
            LoadOptionsCmd = new RelayCommandAsync(LoadOptions);
            ShowFormCmd = new RelayCommandAsyncParam(ShowForm);
            SaveCmd = new RelayCommandAsync(SaveForm);
            CancelCmd = new RelayCommandSyncVoid(() =>
            {
                if (tempProductData == null) CurrentProduct = new ProductEM();
                else ResetProductData();
            });
            CheckAllCmd = new RelayCommandSyncParam(isChecked =>
            {
                foreach (var obj in ProductsList) obj.IsChecked = (bool)isChecked;
            });
            DeleteCmd = new RelayCommandAsync(DeleteRows);

            HeaderContext = new HeaderBarVM
            {
                HeaderText = _t["ListOfProducts"],
                IconKind = "Add",
                ButtonCmd = new RelayCommandAsync(async () =>
                {
                    if (TransitionerIndex == 0) await ShowForm(null);
                    else
                    {
                        HeaderContext.HeaderText = _t["ListOfProducts"];
                        HeaderContext.IconKind = "Add";
                        TransitionerIndex--;
                    }
                })
            };
        }

        private readonly ProductQuery productQuery;
        private readonly OptionQuery optionQuery;
        private readonly IStringLocalizer<Labels> _t;
        private ProductDTO tempProductData;

        public RelayCommandAsync LoadListCmd { get; set; }
        public RelayCommandAsync LoadOptionsCmd { get; set; }
        public RelayCommandAsyncParam ShowFormCmd { get; set; }
        public RelayCommandAsync SaveCmd { get; set; }
        public RelayCommandAsync DeleteCmd { get; set; }
        public RelayCommandSyncVoid CancelCmd { get; set; }
        public RelayCommandSyncParam CheckAllCmd { get; set; }

        private HeaderBarVM _headerContext;
        public HeaderBarVM HeaderContext
        {
            get => _headerContext;
            set => SetValue(ref _headerContext, value);
        }

        private MessageVM _msgContext;
        public MessageVM MsgContext
        {
            get { return _msgContext; }
            set { SetValue(ref _msgContext, value); }
        }

        public bool IsEditMode => CurrentProduct.Id != 0;

        private int _transitionerIndex = 0;
        public int TransitionerIndex
        {
            get => _transitionerIndex;
            set => SetValue(ref _transitionerIndex, value);
        }

        private ProductEM _currentProduct = new ProductEM();
        public ProductEM CurrentProduct
        {
            get { return _currentProduct; }
            set
            {
                SetValue(ref _currentProduct, value);
                OnPropertyChanged(nameof(IsEditMode));
            }
        }

        private int? _categoryId;
        public int? CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductEM> _productsList;
        public ObservableCollection<ProductEM> ProductsList
        {
            get { return _productsList; }
            set { _productsList = value; OnPropertyChanged(); }
        }

        private IEnumerable<OptionValueDTO> _comboOptions = Enumerable.Empty<OptionValueDTO>();
        public IEnumerable<OptionValueDTO> ComboOptions
        {
            get { return _comboOptions; }
            set {
                _comboOptions = value;
                OnPropertyChanged(nameof(CategoryList));
                OnPropertyChanged(nameof(CurrencyList));
                OnPropertyChanged(nameof(UnitList));
                OnPropertyChanged(nameof(BrandList));
            }
        }

        public IList<OptionValueDTO> CategoryList => ComboOptions?.Where(op => op.TypeCode == "CAT").ToList();
        public IList<OptionValueDTO> CurrencyList => ComboOptions?.Where(op => op.TypeCode == "CRC").ToList();
        public IList<OptionValueDTO> UnitList => ComboOptions?.Where(op => op.TypeCode == "UNT").ToList();
        public IList<OptionValueDTO> BrandList => ComboOptions?.Where(op => op.TypeCode == "BRN").ToList();
        private OptionValueDTO DefaultCurrency => ComboOptions?.SingleOrDefault(op => op.TypeCode == "CRC" && op.IsDefault == true);

        private async Task LoadOptions()
        {
            var categoryId = CurrentProduct.CategoryId;
            var currencyId = CurrentProduct.CurrencyId;
            var unitId = CurrentProduct.UnitId;
            var brandId = CurrentProduct.BrandId;

            ComboOptions = await optionQuery.OptionsAll();

            CurrentProduct.CategoryId = categoryId;
            CurrentProduct.CurrencyId = currencyId;
            CurrentProduct.UnitId = unitId;
            CurrentProduct.BrandId = brandId;
        }
        
        private async Task ShowForm(object id)
        {
            TransitionerIndex++;
            HeaderContext.HeaderText = _t["ProductDetails"];
            HeaderContext.IconKind = "ArrowBack";

            if (id == null)
            {
                CurrentProduct = new ProductEM();
                tempProductData = null;
                return;
            }
            await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, args) =>
            {
                tempProductData = await productQuery.GetById((int)id);
                ResetProductData();
                args.Session.Close(false);
            }, null);
        }

        private async Task SaveForm()
        {
            CurrentProduct.ValidateModel();
            if (CurrentProduct.HasErrors) return;

            await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, args) =>
            {
                var data = new ProductDTO
                {
                    Id = CurrentProduct.Id,
                    Code = CurrentProduct.Code,
                    CategoryId = CurrentProduct.CategoryId,
                    Name = CurrentProduct.Name,
                    Cost = CurrentProduct.Cost.Value,
                    Profit = CurrentProduct.Profit.Value,
                    Price = CurrentProduct.Price.Value,
                    InitialQuantity = CurrentProduct.InitialQuantity.Value,
                    CurrencyId = CurrentProduct.CurrencyId ?? DefaultCurrency.Id,
                    UnitId = CurrentProduct.UnitId,
                    BrandId = CurrentProduct.BrandId,
                    AlertQuantity = CurrentProduct.AlertQuantity ?? 0,
                    Discount = CurrentProduct.Discount ?? 0,
                    ExpiryDate = CurrentProduct.ExpiryDate,
                    Note = CurrentProduct.Note,
                    IsDeleted = false,
                    UpdatedBy = 1,
                    UpdatedDate = DateTime.Now,
                };

                if (CurrentProduct.Id == 0)
                {
                    await productQuery.Create(data);
                    CurrentProduct = new ProductEM();
                }
                else
                {
                    await productQuery.Update(data);
                    tempProductData = data;
                }
                await LoadList();
                args.Session.Close(false);
                await MsgContext.ShowSuccess("Product saved successfully!");
            }, null);
        }

        private async Task DeleteRows()
        {
            var ids = ProductsList.Where(p => p.IsChecked).Select(p => p.Id).ToArray();
            if (ids.Length == 0)
            {
                await MsgContext.ShowError("Please select at least one record!");
                return;
            }
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new ConfirmDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "GridDialog", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await productQuery.Delete(ids);
                await LoadList();
                args.Session.Close(false);
                await MsgContext.ShowSuccess($"{ids.Length} records deleted successfully!");
            });
        }

        private void ResetProductData()
        {
            if (tempProductData == null) return;
            CurrentProduct = new ProductEM
            {
                Id = tempProductData.Id,
                Code = tempProductData.Code,
                Name = tempProductData.Name,
                InitialQuantity = tempProductData.InitialQuantity,
                Cost = tempProductData.Cost,
                Price = tempProductData.Price,
                CategoryId = tempProductData.CategoryId,
                CurrencyId = tempProductData.CurrencyId,
                UnitId = tempProductData.UnitId,
                BrandId = tempProductData.BrandId,
                AlertQuantity = tempProductData.AlertQuantity,
                Discount = tempProductData.Discount,
                ExpiryDate = tempProductData.ExpiryDate,
                Note = tempProductData.Note,
            };
        }

        private async Task LoadList()
        {
            var data = await productQuery.GetList(CategoryId);
            var _data = data.Select(p => new ProductEM
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Cost = p.Cost,
                Price = p.Price,
                Discount = p.Discount,
                UnitName = p.UnitName,
                BrandName = p.BrandName,
                CategoryName = p.CategoryName,
                CurrencyName = p.CurrencyName,
                CurrencyCode = p.CurrencyCode,
            });
            ProductsList = new ObservableCollection<ProductEM>(_data);
        }
    }
}
