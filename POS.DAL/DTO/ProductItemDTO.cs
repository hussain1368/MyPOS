namespace POS.DAL.DTO
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string NameAndPrice { get; set; }

        public override string ToString() => NameAndPrice;
    }
}
