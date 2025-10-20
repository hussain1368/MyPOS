using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace POS.WPF.Views.Sections
{
    public partial class CurrencyRateTab : UserControl
    {
        public CurrencyRateTab()
        {
            InitializeComponent();
        }

        private void RateDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var clearBtn = FindDescendantByName<Button>(RateDatePicker, "PART_ClearButton");
            if (clearBtn != null)
            {
                clearBtn.Click -= ClearBtn_Click;
                clearBtn.Click += ClearBtn_Click;

                clearBtn.ClickMode = ClickMode.Release;
                clearBtn.Focusable = false;
                clearBtn.IsTabStop = false;
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                RateDatePicker.SelectedDate = null;

            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        public static T FindDescendantByName<T>(DependencyObject root, string name) where T : FrameworkElement
        {
            if (root == null) return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child is T fe && fe.Name == name) return fe;
                var found = FindDescendantByName<T>(child, name);
                if (found != null) return found;
            }
            return null;
        }
    }
}
