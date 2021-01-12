using POS.WPF.Enums;
using POS.WPF.ModelValidators;

namespace POS.WPF.Models
{
    public class ProductModel : BaseModel<ProductModel>
    {
        private int id { get; set; }
        private string code { get; set; }
        private CodeStatus codeStatus { get; set; }
        private string name { get; set; }
        private int? initialQuantity { get; set; }
        private int? cost { get; set; }
        private int? price { get; set; }
        private int? categoryId { get; set; }

        public ProductModel() : base(new ProductValidator()) { }

        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                ValidateField();
                OnPropertyChanged();
            }
        }
        public CodeStatus CodeStatus
        {
            get { return codeStatus; }
            set { codeStatus = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; ValidateField(); OnPropertyChanged(); }
        }
        public int? InitialQuantity
        {
            get { return initialQuantity; }
            set { initialQuantity = value; OnPropertyChanged(); }
        }
        public int? Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Profit));
            }
        }
        public int? Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Profit));
            }
        }
        public int? Profit
        {
            get { return (Price ?? 0) - (Cost ?? 0); }
        }
        public int? CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; OnPropertyChanged(); }
        }
    }
}
