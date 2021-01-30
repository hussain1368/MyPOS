using POS.DAL.Query;
using POS.DAL.Models;
using POS.WPF.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using POS.WPF.Models;
using System.Threading.Tasks;
using System.Linq;
using POS.WPF.Controls;
using MaterialDesignThemes.Wpf;
using System;
using MaterialDesignThemes.Wpf.Transitions;

namespace POS.WPF.ViewModels
{
    public class ProductVM : BaseViewModel
    {
        private readonly ProductQuery productQuery;
        private readonly OptionQuery optionQuery;

        public RelayCommandAsync LoadListCmd { get; set; }
        public RelayCommandAsync LoadOptionsCmd { get; set; }
        public RelayCommandAsyncParam ShowFormCmd { get; set; }
        public RelayCommandAsync SaveCmd { get; set; }
        public RelayCommandAsync DeleteCmd { get; set; }
        public RelayCommandSyncVoid CancelCmd { get; set; }
        public RelayCommandSyncParam CheckAllCmd { get; set; }

        public ProductVM(ProductQuery productQuery, OptionQuery optionQuery)
        {
            this.productQuery = productQuery;
            this.optionQuery = optionQuery;
            CreateCommands();
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

        private IList<OptionValueDT> _categoryList;
        public IList<OptionValueDT> CategoryList
        {
            get { return _categoryList; }
            set { _categoryList = value; OnPropertyChanged(); }
        }

        private bool _isListLoading;
        public bool IsListLoading
        {
            get { return _isListLoading; }
            set { _isListLoading = value; OnPropertyChanged(); }
        }

        private void CreateCommands()
        {
            ShowFormCmd = new RelayCommandAsyncParam(async id =>
            {
                Transitioner.MoveNextCommand.Execute(null, null);

                if (id == null)
                {
                    CurrentProduct = new ProductModel();
                    return;
                }
            
                await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, eventArgs) =>
                {
                    int _id = (int)id;
                    var product = await productQuery.GetById(_id);
                    CurrentProduct = new ProductModel
                    {
                        Id = product.Id,
                        Code = product.Code,
                        CodeStatus = (Enums.CodeStatus)product.CodeStatus,
                        Name = product.Name,
                        InitialQuantity = product.InitialQuantity,
                        Cost = product.Cost,
                        Price = product.Price,
                        Discount = product.Discount,
                        CategoryId = product.CategoryId,
                    };
                    eventArgs.Session.Close(false);

                }, null);
            });

            LoadListCmd = new RelayCommandAsync(async () =>
            {
                IsListLoading = true;
                await LoadProducts();
                await Task.Delay(2000);
                IsListLoading = false;
            });

            LoadOptionsCmd = new RelayCommandAsync(async () =>
            {
                CategoryList = await optionQuery.OptionsByTypeId(1);
            });

            SaveCmd = new RelayCommandAsync(async () =>
            {
                CurrentProduct.ValidateModel();
                if (CurrentProduct.HasErrors) return;

                await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, eventArgs) =>
                {
                    var data = new ProductDT
                    {
                        Code = CurrentProduct.Code,
                        CodeStatus = (byte)CurrentProduct.CodeStatus,
                        Name = CurrentProduct.Name,
                        Cost = CurrentProduct.Cost.Value,
                        Profit = CurrentProduct.Profit.Value,
                        Price = CurrentProduct.Price.Value,
                        Discount = 0,
                        AlertQuantity = 0,
                        InitialQuantity = CurrentProduct.InitialQuantity.Value,
                        UnitId = null,
                        BrandId = null,
                        CategoryId = CurrentProduct.CategoryId,
                        CurrencyId = 2,
                        ExpiryDate = null,
                        Note = null,
                        InsertedBy = 1,
                        InsertedDate = DateTime.Now,
                        UpdatedBy = null,
                        UpdatedDate = null,
                        IsDeleted = false,
                    };

                    await productQuery.Create(data);
                    CurrentProduct = new ProductModel();
                    LoadListCmd.Execute(null);
                    eventArgs.Session.Close(false);
                }, null);
            });

            CancelCmd = new RelayCommandSyncVoid(() =>
            {
                CurrentProduct = new ProductModel();
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
                    await LoadProducts();
                    eventArgs.Session.Close(false);
                });
            });
        }

        private async Task LoadProducts()
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
            });
            ProductsList = new ObservableCollection<ProductModel>(_data);
        }
    }
}
