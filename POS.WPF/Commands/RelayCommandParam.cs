using System;
using System.Windows.Input;

namespace POS.WPF.Commands
{
    public class RelayCommandParam : ICommand
    {
        private readonly Action<object> Work;

        public event EventHandler CanExecuteChanged;

        public RelayCommandParam(Action<object> work)
        {
            Work = work;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Work(parameter);
        }
    }
}
