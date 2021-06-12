using System;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace POS.WPF.Models.ViewModels
{
    public class MessageVM : BaseBindable
    {
        public MessageVM()
        {
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
        }

        public ISnackbarMessageQueue MessageQueue { get; set; }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetValue(ref _isActive, value); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetValue(ref _backgroundColor, value); }
        }

        private PackIconKind _iconKind;
        public PackIconKind IconKind
        {
            get { return _iconKind; }
            set { SetValue(ref _iconKind, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }

        public async Task ShowSuccess(string message)
        {
            Message = message;
            IconKind = PackIconKind.CheckBold;
            BackgroundColor = "Green";
            IsActive = true;
            await Task.Delay(2000);
            IsActive = false;
        }

        public async Task ShowError(string message)
        {
            Message = message;
            IconKind = PackIconKind.Error;
            BackgroundColor = "Red";
            IsActive = true;
            await Task.Delay(2000);
            IsActive = false;
        }

        public object GetMsgContent(string message, bool success)
        {
            var panel = new System.Windows.Controls.WrapPanel();
            panel.Children.Add(new PackIcon
            {
                Kind = success ? PackIconKind.CheckBold : PackIconKind.Error
            });
            panel.Children.Add(new System.Windows.Controls.TextBlock
            {
                Margin = new System.Windows.Thickness(10, 0, 0, 0),
                Text = message
            });
            return panel;
        }
    }
}
