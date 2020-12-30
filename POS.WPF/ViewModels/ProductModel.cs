using System;

namespace POS.WPF.ViewModels
{
    public class ProductModel : BaseViewModel
    {
        private int id { get; set; }
        private string code { get; set; }
        private byte codeStatus { get; set; }
        private string name { get; set; }
        private int initialQuantity { get; set; }
        private int cost { get; set; }
        private int profit { get; set; }
        private int price { get; set; }
        private int? categoryId { get; set; }

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged(); }
        }
        public byte CodeStatus
        {
            get { return codeStatus; }
            set { codeStatus = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public int InitialQuantity
        {
            get { return initialQuantity; }
            set { initialQuantity = value; OnPropertyChanged(); }
        }
        public int Cost
        {
            get { return cost; }
            set { cost = value; OnPropertyChanged(); }
        }
        public int Profit
        {
            get { return profit; }
            set { profit = value; OnPropertyChanged(); }
        }
        public int Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); }
        }
        public int? CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; OnPropertyChanged(); }
        }
    }
}
