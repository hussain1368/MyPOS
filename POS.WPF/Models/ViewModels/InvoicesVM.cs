using POS.DAL.DTO;
using POS.DAL.Query;
using POS.WPF.Commands;
using POS.WPF.Models.EntityModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS.WPF.Models.ViewModels
{
    public class InvoicesVM : BaseBindable
    {
        public InvoicesVM(ProductQuery productQuery)
        {
            this.productQuery = productQuery;
            ButtonCommand = new RelayCommandSyncVoid(() => { });
            FindByNameInputCmd = new RelayCommandAsync(FindByName);
            FindByNameKeyUpCmd = new RelayCommandAsyncParam(FindByNameKeyUp);
            AddInvoiceItemCmd = new RelayCommandSyncVoid(() => AddInvoiceItem(SelectedProduct));
        }
        private readonly ProductQuery productQuery;

        public RelayCommandAsync FindByNameInputCmd { get; set; }
        public RelayCommandAsyncParam FindByNameKeyUpCmd { get; set; }
        public RelayCommandSyncVoid AddInvoiceItemCmd { get; set; }
        public RelayCommandSyncVoid ButtonCommand { get; set; }

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

        private async Task FindByCode()
        {
            var product = await productQuery.GetByCode(SearchValue);
            if (product != null) AddInvoiceItem(product);
        }

        private async Task FindByName()
        {
            if (SearchValue?.Length > 2)
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
                    SequenceNum = 0,
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Discount = product.Discount,
                    Quantity = 1,
                });
            }
            else productFound.Quantity++;

            SearchValue = string.Empty;
            ProductsList = Enumerable.Empty<ProductItemDTO>();
        }
    }
}
