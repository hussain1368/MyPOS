using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using POS.DAL.Query;
using POS.DAL.Models;
using POS.WPF.Commands;
using POS.WPF.Models;
using POS.WPF.Controls;

namespace POS.WPF.ViewModels
{
    public class ProductsVM : BaseVM
    {
        public ProductsVM(ProductQuery productQuery, OptionQuery optionQuery)
        {
            this.productQuery = productQuery;
            this.optionQuery = optionQuery;
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
            CreateCommands();
        }

        private readonly ProductQuery productQuery;
        private readonly OptionQuery optionQuery;
        private ProductDTM theProduct;
        private OptionValueDTM defaultCurrency;

        public ISnackbarMessageQueue MessageQueue { get; set; }
        public RelayCommandAsync LoadListCmd { get; set; }
        public RelayCommandAsync LoadOptionsCmd { get; set; }
        public RelayCommandAsyncParam ShowFormCmd { get; set; }
        public RelayCommandAsync SaveCmd { get; set; }
        public RelayCommandAsync DeleteCmd { get; set; }
        public RelayCommandSyncVoid CancelCmd { get; set; }
        public RelayCommandSyncParam CheckAllCmd { get; set; }

        public HeaderBarVM FormHeader => new HeaderBarVM
        {
            HeaderText = "Product Details",
            IconKind = "ArrowBack",
            ButtonCommand = new RelayCommandSyncVoid(() =>
            {
                Transitioner.MovePreviousCommand.Execute(null, null);
            })
        };

        public HeaderBarVM GridHeader => new HeaderBarVM
        {
            HeaderText = "List of Products",
            IconKind = "Add",
            ButtonCommand = new RelayCommandAsyncParam(ShowForm),
        };

        private async Task ShowForm(object id)
        {
            Transitioner.MoveNextCommand.Execute(null, null);
            if (id == null)
            {
                CurrentProduct = new ProductModel();
                theProduct = null;
                return;
            }
            await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, eventArgs) =>
            {
                int _id = (int)id;
                theProduct = await productQuery.GetById(_id);
                MapCurrentProduct();
                eventArgs.Session.Close(false);
            }, null);
        }

        private ProductModel _currentProduct = new ProductModel();
        public ProductModel CurrentProduct
        {
            get { return _currentProduct; }
            set { _currentProduct = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ProductModel> _productsList;
        public ObservableCollection<ProductModel> ProductsList
        {
            get { return _productsList; }
            set { _productsList = value; OnPropertyChanged(); }
        }

        private IList<OptionValueDTM> _comboOptions;
        public IList<OptionValueDTM> ComboOptions
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

        public IList<OptionValueDTM> CategoryList => ComboOptions?.Where(op => op.TypeCode == "CAT").ToList();
        public IList<OptionValueDTM> CurrencyList => ComboOptions?.Where(op => op.TypeCode == "CRC").ToList();
        public IList<OptionValueDTM> UnitList => ComboOptions?.Where(op => op.TypeCode == "UNT").ToList();
        public IList<OptionValueDTM> BrandList => ComboOptions?.Where(op => op.TypeCode == "BRN").ToList();

        private bool _isListLoading;
        public bool IsListLoading
        {
            get { return _isListLoading; }
            set { _isListLoading = value; OnPropertyChanged(); }
        }

        private void CreateCommands()
        {
            LoadListCmd = new RelayCommandAsync(async () =>
            {
                await DialogHost.Show(new LoadingDialog(), "GridDialog", async (sender, eventArgs) =>
                {
                    await LoadProductsList();
                    eventArgs.Session.Close(false);
                }, null);
            });

            LoadOptionsCmd = new RelayCommandAsync(async () =>
            {
                ComboOptions = await optionQuery.OptionsAll();
                defaultCurrency = await optionQuery.OptionByCode("AFN");
            });

            ShowFormCmd = new RelayCommandAsyncParam(ShowForm);

            SaveCmd = new RelayCommandAsync(async () =>
            {
                CurrentProduct.ValidateModel();
                if (CurrentProduct.HasErrors) return;

                await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, eventArgs) =>
                {
                    var data = new ProductDTM
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

                        CurrencyId = CurrentProduct.CurrencyId ?? defaultCurrency.Id,
                        UnitId = CurrentProduct.UnitId,
                        BrandId = CurrentProduct.BrandId,
                        AlertQuantity = CurrentProduct.AlertQuantity ?? 0,
                        Discount = CurrentProduct.Discount ?? 0,
                        ExpiryDate = CurrentProduct.ExpiryDate,
                        Note = CurrentProduct.Note,

                        InsertedBy = 1,
                        InsertedDate = DateTime.Now,
                        UpdatedBy = 1,
                        UpdatedDate = DateTime.Now,
                        IsDeleted = false,
                    };

                    if (CurrentProduct.Id == 0)
                    {
                        await productQuery.Create(data);
                        CurrentProduct = new ProductModel();
                    }
                    else
                    {
                        await productQuery.Update(data);
                        theProduct = data;
                    }
                    eventArgs.Session.Close(false);
                    var content = getMsg("Product Saved!");
                    MessageQueue.Enqueue(content);
                    LoadListCmd.Execute(null);
                }, null);
            });

            CancelCmd = new RelayCommandSyncVoid(() =>
            {
                if (theProduct == null) CurrentProduct = new ProductModel();
                else MapCurrentProduct();
            });

            CheckAllCmd = new RelayCommandSyncParam(isChecked =>
            {
                bool _isChecked = (bool)isChecked;
                foreach (var obj in ProductsList) obj.IsChecked = _isChecked;
            });

            DeleteCmd = new RelayCommandAsync(async () =>
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
                    await LoadProductsList();
                    eventArgs.Session.Close(false);
                    var content = getMsg($"{ids.Length} Records Deleted!");
                    MessageQueue.Enqueue(content);
                });
            });
        }

        private void MapCurrentProduct()
        {
            if (theProduct == null) return;

            CurrentProduct = new ProductModel
            {
                Id = theProduct.Id,
                Code = theProduct.Code,
                CodeStatus = (Enums.CodeStatus)theProduct.CodeStatus,
                Name = theProduct.Name,
                InitialQuantity = theProduct.InitialQuantity,
                Cost = theProduct.Cost,
                Price = theProduct.Price,
                CategoryId = theProduct.CategoryId,

                CurrencyId = theProduct.CurrencyId,
                UnitId = theProduct.UnitId,
                BrandId = theProduct.BrandId,
                AlertQuantity = theProduct.AlertQuantity,
                Discount = theProduct.Discount,
                ExpiryDate = theProduct.ExpiryDate,
                Note = theProduct.Note,
            };
        }

        private async Task LoadProductsList()
        {
            var data = await productQuery.GetList();
            var _data = data.Select(p => new ProductModel
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
            ProductsList = new ObservableCollection<ProductModel>(_data);
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
