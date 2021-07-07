using MaterialDesignThemes.Wpf;
using POS.WPF.Validators.ModelValidators;

namespace POS.WPF.Models.EntityModels
{
    public class OptionValueEM : BaseErrorBindable<OptionValueEM>
    {
        public OptionValueEM() : base(new OptionValueValidator()) { }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        private int? _typeId;
        public int? TypeId
        {
            get { return _typeId; }
            set { SetAndValidate(ref _typeId, value); }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetValue(ref _code, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetAndValidate(ref _name, value); }
        }

        public bool IsDefault { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsEditable => !IsReadOnly;

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetValue(ref _isChecked, value); }
        }

        public PackIconKind IsActiveIcon => IsDeleted ? PackIconKind.CloseThick : PackIconKind.CheckBold;
        public PackIconKind IsDefaultIcon => IsDefault ? PackIconKind.CheckBold : PackIconKind.CloseThick;
    }
}
