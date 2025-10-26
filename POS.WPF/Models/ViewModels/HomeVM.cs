using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace POS.WPF.Models.ViewModels
{
    public class HomeVM : BaseBindable
    {
        public HomeVM()
        {
            TotalSale = new SingleNumberCard
            {
                Title = "Total Sale",
                IconKind = PackIconKind.CashPlus,
                Number = 124_000,
            };

            TotalExpense = new SingleNumberCard
            {
                Title = "Total Expense",
                IconKind = PackIconKind.CashMinus,
                Number = 86_000,
            };

            CurrentBalance = new SingleNumberCard
            {
                Title = "Current Balance",
                IconKind = PackIconKind.Cash,
                Number = 52_000,
            };

            TotalProfit = new SingleNumberCard
            {
                Title = "Total Profit",
                IconKind = PackIconKind.Finance,
                Number = 78_600,
            };

            BestProducts = new TopFiveCardVM
            {
                Title = "Best Selling Products",
                IconKind = PackIconKind.BarcodeScan,
                Items = new ObservableCollection<TopTenItem>
                {
                    new TopTenItem { Name = "ASUS ROG Ally", Total = 150, Percent = 30 },
                    new TopTenItem { Name = "iPhone 17 Pro Max", Total = 120, Percent = 24 },
                    new TopTenItem { Name = "Samsung Galaxy S Ultra", Total = 100, Percent = 20 },
                    new TopTenItem { Name = "Lenovo Notebook", Total = 80, Percent = 16 },
                    new TopTenItem { Name = "Oneplus Nord CE", Total = 50, Percent = 10 },
                }
            };

            BestCustomers = new TopFiveCardVM
            {
                Title = "Best Customers",
                IconKind = PackIconKind.AccountMultiple,
                Items = new ObservableCollection<TopTenItem>
                {
                    new TopTenItem { Name = "Abdullah Haidari", Total = 150, Percent = 30 },
                    new TopTenItem { Name = "Aziz Saeidi", Total = 120, Percent = 24 },
                    new TopTenItem { Name = "Hashem Karimi", Total = 100, Percent = 20 },
                    new TopTenItem { Name = "Naser Poyan", Total = 80, Percent = 16 },
                    new TopTenItem { Name = "Elaha Naderi", Total = 50, Percent = 10 },
                }
            };
        }

        public SingleNumberCard TotalSale { get; set; }

        public SingleNumberCard TotalExpense { get; set; }

        public SingleNumberCard CurrentBalance { get; set; }

        public SingleNumberCard TotalProfit { get; set; }

        public TopFiveCardVM BestProducts { get; set; }
        public TopFiveCardVM BestCustomers { get; set; }
    }

    public class TopTenItem
    {
        public string Name { get; set; }
        public int Total { get; set; }
        public int Percent { get; set; }
    }

    public class TopFiveCardVM
    {
        public string Title { get; set; }
        public PackIconKind IconKind { get; set; }
        public ObservableCollection<TopTenItem> Items { get; set; }
    }
}
