using MaterialDesignThemes.Wpf;
using POS.DAL.Repository.Abstraction;
using POS.WPF.Commands;
using POS.WPF.Models.EntityModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class HomeVM : BaseBindable
    {
        public HomeVM(IReportsRepository reportsRepo)
        {
            _reportsRepo = reportsRepo;

            SetUpTheCards();

            LoadNumbersCmd = new CommandAsync(LoadNumbers);
        }

        private readonly IReportsRepository _reportsRepo;

        public CommandAsync LoadNumbersCmd { get; set; }

        private async Task LoadNumbers()
        {
            var totals = await _reportsRepo.GetTotals();

            TotalSale.Number = totals.TotalSales;
            TotalExpense.Number = totals.TotalExpenses;
            CurrentBalance.Number = totals.CurrentBalance;

            var bestProducts = totals.BestProducts.Select(r => new TopFiveItem
            {
                Name = r.ProductName,
                Total = r.TotalQuantitySold,
                Percent = r.PercentageOfTotalSales,
            });

            var bestCustomers = totals.BestCustomers.Select(r => new TopFiveItem
            {
                Name = r.PartnerName,
                Total = r.TotalPurchases,
                Percent = r.PercentageOfTotalPurchases,
            });

            BestProducts.Items = new ObservableCollection<TopFiveItem>(bestProducts);
            BestCustomers.Items = new ObservableCollection<TopFiveItem>(bestCustomers);
        }

        private void SetUpTheCards()
        {
            TotalSale = new SingleNumberCardEM
            {
                Title = "Total Sale",
                IconKind = PackIconKind.CashPlus,
            };

            TotalExpense = new SingleNumberCardEM
            {
                Title = "Total Expense",
                IconKind = PackIconKind.CashMinus,
            };

            CurrentBalance = new SingleNumberCardEM
            {
                Title = "Current Balance",
                IconKind = PackIconKind.Cash,
            };

            BestProducts = new TopFiveCardEM
            {
                Title = "Best Selling Products",
                IconKind = PackIconKind.BarcodeScan,
            };

            BestCustomers = new TopFiveCardEM
            {
                Title = "Best Customers",
                IconKind = PackIconKind.AccountMultiple,
            };
        }

        public SingleNumberCardEM TotalSale { get; set; }
        public SingleNumberCardEM TotalExpense { get; set; }
        public SingleNumberCardEM CurrentBalance { get; set; }

        public TopFiveCardEM BestProducts { get; set; }
        public TopFiveCardEM BestCustomers { get; set; }
    }
}
