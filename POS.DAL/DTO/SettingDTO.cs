namespace POS.DAL.DTO
{
    public class SettingDTO
    {
        public int Id { get; set; }
        public string LayoutName { get; set; }
        public string Language { get; set; }
        public string AppTitle { get; set; }
        public byte CalendarType { get; set; }
        public bool IsActive { get; set; }
    }
}
