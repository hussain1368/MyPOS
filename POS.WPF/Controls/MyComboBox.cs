using System.Windows.Controls;
using System.Windows.Input;

namespace POS.WPF.Controls
{
    public class MyComboBox : ComboBox
    {
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            IsDropDownOpen = true;
            if (e.Key == Key.Down || e.Key == Key.Up) return;
            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}
