namespace POS.DAL.DTO
{
    public class OptionTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsDeleted { get; set; }
    }
}
