using POS.WPF.Enums;

namespace POS.WPF.Models.EntityModels
{
    public class SettingEM : BaseBindable
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        private string _appTitle;
        public string AppTitle
        {
            get { return _appTitle; }
            set { SetValue(ref _appTitle, value); }
        }

        private CalendarType _calendarType;
        public CalendarType CalendarType
        {
            get { return _calendarType; }
            set { SetValue(ref _calendarType, value); }
        }

        private string _language;
        public string Language
        {
            get { return _language; }
            set { SetValue(ref _language, value); }
        }
    }
}
