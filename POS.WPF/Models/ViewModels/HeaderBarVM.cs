using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;

namespace POS.WPF.Models.ViewModels
{
    public class HeaderBarVM : BaseBindable
    {
        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetValue(ref _headerText, value);
        }

        private PackIconKind _iconKind;
        public PackIconKind IconKind
        {
            get => _iconKind;
            set => SetValue(ref _iconKind, value);
        }

        private bool _isButtonVisible = true;
        public bool IsButtonVisible
        {
            get => _isButtonVisible;
            set
            {
                SetValue(ref _isButtonVisible, value);
                OnPropertyChanged(nameof(ButtonVisibility));
            }
        }

        public Visibility ButtonVisibility => IsButtonVisible ? Visibility.Visible : Visibility.Hidden;

        public ICommand ButtonCmd { get; set; }
    }
}
