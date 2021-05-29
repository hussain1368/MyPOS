using POS.WPF.Enums;
using System.Windows;

namespace POS.WPF.Common
{
    public class RadioBoxModel
    {
        public static PaymentType GetModel(DependencyObject obj)
        {
            return (PaymentType)obj.GetValue(ModelProperty);
        }

        public static void SetModel(DependencyObject obj, PaymentType value)
        {
            obj.SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.RegisterAttached("Model", typeof(PaymentType), typeof(RadioBoxModel), new PropertyMetadata(default(PaymentType)));
    }
}
