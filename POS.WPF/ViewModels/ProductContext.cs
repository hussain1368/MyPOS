using POS.DAL.DataQuery;
using POS.WPF.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.ViewModels
{
    public class ProductContext : BaseVM
    {
        private readonly ProductQuery productQuery;
        public RelayCommandAsync LoadListCmd { get; set; }

        public ProductContext(ProductQuery productQuery)
        {
            this.productQuery = productQuery;
            LoadListCmd = new RelayCommandAsync(async () =>
            {
                var data = await productQuery.GetList();
                ProductsList = data.Select(x => new ProductVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    SalePrice = x.SalePrice,
                })
                .ToList();
            });
        }

        private int product;

        public int Product
        {
            get { return product; }
            set { product = value; OnPropertyChanged(nameof(Product)); }
        }

        //ObservableCollection

        private IList<ProductVM> productsList;

        public IList<ProductVM> ProductsList
        {
            get { return productsList; }
            set { productsList = value; OnPropertyChanged(nameof(ProductsList)); }
        }
    }
}
