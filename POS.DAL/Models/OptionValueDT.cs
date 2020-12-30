namespace POS.DAL.Models
{
    public class OptionValueDT
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Flag { get; set; }
        public bool IsDeleted { get; set; }
    }
}
