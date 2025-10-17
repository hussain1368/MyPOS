namespace POS.DAL.DTO
{
    public class WalletDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        public int CurrentBalance { get; set; }
        public string Note { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}
