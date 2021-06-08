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
using POS.WPF.Views.Layout;

namespace POS.WPF.Models.ViewModels
{
    public class ProductsVM : BaseBindable
    {
        public ProductsVM(ProductQuery productQuery, OptionQuery optionQuery, IStringLocalizer<Labels> _t)
        {
            this.productQuery = productQuery;
            this._t = _t;
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

            LoadListCmd = new RelayCommandAsync(async () =>
            {
                await DialogHost.Show(new LoadingDialog(), "GridDialog", async (sender, eventArgs) =>
                {
                    await LoadList();
                    eventArgs.Session.Close(false);
                }, null);
            });
            LoadOptionsCmd = new RelayCommandAsync(async () =>
            {
                ComboOptions = await optionQuery.OptionsAll();
            });
            ShowFormCmd = new RelayCommandAsyncParam(ShowForm);
            SaveCmd = new RelayCommandAsync(SaveForm);
            CancelCmd = new RelayCommandSyncVoid(() =>
            {
                if (tempProductData == null) CurrentProduct = new ProductEM();
                else MapProduct();
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
        private readonly IStringLocalizer<Labels> _t;
        private ProductDTO tempProductData;

        public ISnackbarMessageQueue MessageQueue { get; set; }
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
                _currentProduct = value; 
                OnPropertyChanged();
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

        private IList<OptionValueDTO> _comboOptions;
        public IList<OptionValueDTO> ComboOptions
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
        private OptionValueDTO DefaultCurrency => ComboOptions?.SingleOrDefault(op => op.Code == "AFN");

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
            await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, eventArgs) =>
            {
                tempProductData = await productQuery.GetById((int)id);
                MapProduct();
                eventArgs.Session.Close(false);
            }, null);
        }

        private async Task SaveForm()
        {
            CurrentProduct.ValidateModel();
            if (CurrentProduct.HasErrors) return;

            await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, eventArgs) =>
            {
                var data = new ProductDTO
                {
                    Id = CurrentProduct.Id,
                    Code = CurrentProduct.Code,
                    CodeStatus = (byte)CurrentProduct.CodeStatus,
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
                eventArgs.Session.Close(false);
                var content = getMsg("Product Saved!");
                MessageQueue.Enqueue(content);
            }, null);
        }

        private async Task DeleteRows()
        {
            var ids = ProductsList.Where(p => p.IsChecked).Select(p => p.Id).ToArray();
            if (ids.Length == 0) return;
            string message = $"Are you sure to delete ({ids.Length}) records?";
            var view = new ConfirmDialog(new ConfirmDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "GridDialog", null, async (sender, eventArgs) =>
            {
                if (eventArgs.Parameter is bool param && param == false) return;
                eventArgs.Cancel();
                eventArgs.Session.UpdateContent(new LoadingDialog());
                await productQuery.Delete(ids);
                await LoadList();
                eventArgs.Session.Close(false);
                var content = getMsg($"{ids.Length} Records Deleted!");
                MessageQueue.Enqueue(content);
            });
        }

        private void MapProduct()
        {
            if (tempProductData == null) return;

            CurrentProduct = new ProductEM
            {
                Id = tempProductData.Id,
                Code = tempProductData.Code,
                CodeStatus = (Enums.CodeStatus)tempProductData.CodeStatus,
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

        private object getMsg(string msg)
        {
            var panel = new System.Windows.Controls.StackPanel();
            panel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            var icon = new PackIcon();
            icon.Kind = PackIconKind.CheckBold;
            var text = new System.Windows.Controls.TextBlock();
            text.Margin = new System.Windows.Thickness(10, 0, 0, 0);
            text.Text = msg;
            panel.Children.Add(icon);
            panel.Children.Add(text);
            return panel;
        }
    }
}
