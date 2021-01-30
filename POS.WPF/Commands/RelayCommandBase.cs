using System;
using System.Windows.Input;

namespace POS.WPF.Commands
{
    public abstract class RelayCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { _isExecuting = value; CanExecuteChanged?.Invoke(this, new EventArgs()); }
        }

        public bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public void Execute(object parameter)
        {
            Run(parameter);
        }

        public abstract void Run(object param);
    }
}
