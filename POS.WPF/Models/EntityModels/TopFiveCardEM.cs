using MaterialDesignThemes.Wpf;
using POS.WPF.Models.ViewModels;
using System.Collections.ObjectModel;

namespace POS.WPF.Models.EntityModels
{
    public class TopFiveCardEM : BaseBindable
    {
        public string Title { get; set; }
        public PackIconKind IconKind { get; set; }

        private ObservableCollection<TopFiveItem> _items;
        public ObservableCollection<TopFiveItem> Items
        {
            get => _items;
            set => SetValue(ref _items, value);
        }
    }

    public class TopFiveItem
    {
        public string Name { get; set; }
        public int Total { get; set; }
        public int Percent { get; set; }
    }
}
