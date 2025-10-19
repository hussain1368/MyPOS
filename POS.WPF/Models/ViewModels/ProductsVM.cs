using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Localization;
using POS.DAL.DTO;
using POS.WPF.Commands;
using POS.WPF.Models.EntityModels;
using POS.WPF.Views.Shared;
using POS.DAL.Repository.Abstraction;
using AutoMapper;

namespace POS.WPF.Models.ViewModels
{
    public class ProductsVM : BaseBindable
    {
        public ProductsVM(IMapper mapper, IProductRepository productRepo, IOptionRepository optionRepo, IStringLocalizer<Labels> _t)
        {
            this.mapper = mapper;
            this.productRepo = productRepo;
            this.optionRepo = optionRepo;
            this._t = _t;
            MsgContext = new MessageVM();

            LoadListCmd = new CommandAsync(async () =>
            {
                await DialogHost.Show(new LoadingDialog(), "GridDialog", async (sender, args) =>
                {
                    await LoadList();
                    args.Session.Close(false);
                }, null);
            });
            LoadOptionsCmd = new CommandAsync(LoadOptions);
            ShowFormCmd = new CommandAsyncParam(ShowForm);
            SaveCmd = new CommandAsync(SaveForm);
            CancelCmd = new CommandSync(() =>
            {
                if (tempProductData == null) CurrentProduct = new ProductEM();
                else ResetProductData();
            });
            CheckAllCmd = new CommandParam(isChecked =>
            {
                foreach (var obj in ProductsList) obj.IsChecked = (bool)isChecked;
            });
            DeleteCmd = new CommandAsync(DeleteRows);

            HeaderContext = new HeaderBarVM
            {
                HeaderText = _t["ListOfProducts"],
                IconKind = "Add",
                ButtonCmd = new CommandAsync(async () =>
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

        private readonly IMapper mapper;
        private readonly IProductRepository productRepo;
        private readonly IOptionRepository optionRepo;
        private readonly IStringLocalizer<Labels> _t;
        private ProductDTO tempProductData;

        public CommandAsync LoadListCmd { get; set; }
        public CommandAsync LoadOptionsCmd { get; set; }
        public CommandAsyncParam ShowFormCmd { get; set; }
        public CommandAsync SaveCmd { get; set; }
        public CommandAsync DeleteCmd { get; set; }
        public CommandSync CancelCmd { get; set; }
        public CommandParam CheckAllCmd { get; set; }

        private HeaderBarVM _headerContext;
        public HeaderBarVM HeaderContext
        {
            get => _headerContext;
            set => SetValue(ref _headerContext, value);
        }

        private MessageVM _msgContext;
        public MessageVM MsgContext
        {
            get => _msgContext;
            set => SetValue(ref _msgContext, value);
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

            ComboOptions = await optionRepo.OptionsAll();

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
                tempProductData = await productRepo.GetById((int)id);
                ResetProductData();
                args.Session.Close(false);
            }, null);
        }

        private async Task SaveForm()
        {
            await CurrentProduct.ValidateModelAsync();
            //CurrentProduct.ValidateModel();
            if (CurrentProduct.HasErrors) return;

            if (string.IsNullOrWhiteSpace(CurrentProduct.Code))
            {
                var message = "Product code is blank. Do you want to generate a new code?";
                await DialogHost.Show(new ConfirmDialog(new MyDialogVM { Message = message }), "FormDialog", null, async (sender, args) =>
                {
                    if (args.Parameter is bool param && param == false) return;
                    args.Cancel();
                    args.Session.UpdateContent(new LoadingDialog());
                    await SaveToDatabase();
                    await LoadList();
                    args.Session.Close(false);
                    await MsgContext.ShowSuccess("Product saved successfully!");
                });
            }
            else
            {
                await DialogHost.Show(new LoadingDialog(), "FormDialog", async (sender, args) =>
                {
                    await SaveToDatabase();
                    args.Session.Close(false);
                    await MsgContext.ShowSuccess("Product saved successfully!");
                }, null);
            }
        }

        private async Task SaveToDatabase()
        {
            var data = mapper.Map<ProductDTO>(CurrentProduct);
            data.CurrencyId = CurrentProduct.CurrencyId ?? DefaultCurrency.Id;
            data.UpdatedDate = DateTime.Now;
            data.UpdatedBy = 1;
            data.IsDeleted = false;

            if (CurrentProduct.Id == 0)
            {
                await productRepo.Create(data);
                CurrentProduct = new ProductEM();
            }
            else
            {
                await productRepo.Update(data);
                tempProductData = data;
            }
            await LoadList();
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
            var view = new ConfirmDialog(new MyDialogVM { Message = message });
            var obj = await DialogHost.Show(view, "GridDialog", null, async (sender, args) =>
            {
                if (args.Parameter is bool param && param == false) return;
                args.Cancel();
                args.Session.UpdateContent(new LoadingDialog());
                await productRepo.Delete(ids);
                await LoadList();
                args.Session.Close(false);
                await MsgContext.ShowSuccess($"{ids.Length} records deleted successfully!");
            });
        }

        private void ResetProductData()
        {
            if (tempProductData == null) return;
            CurrentProduct = mapper.Map<ProductEM>(tempProductData);
        }

        private async Task LoadList()
        {
            var result = await productRepo.GetList(CategoryId, PageIndex);
            var _data = mapper.Map<IEnumerable<ProductEM>>(result.Products);
            ProductsList = new ObservableCollection<ProductEM>(_data);
            PageCount = result.PageCount;
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
    }
}
