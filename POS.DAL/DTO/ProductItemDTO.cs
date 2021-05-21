namespace POS.DAL.DTO
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }

        public override string ToString() => Name;
    }
}
