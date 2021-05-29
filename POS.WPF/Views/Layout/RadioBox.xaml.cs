using System.Windows;
using System.Windows.Controls;

namespace POS.WPF.Views.Layout
{
    public partial class RadioBox : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(RadioBox), new PropertyMetadata(default(string)));

        public RadioBox()
        {
            InitializeComponent();
        }
    }
}
