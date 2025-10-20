using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POS.WPF.Models
{
    public abstract class BaseBindable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected void SetValue<F>(ref F prop, F value, [CallerMemberName] string propName = "")
        {
            prop = value;
            OnPropertyChanged(propName);
        }
    }
}
