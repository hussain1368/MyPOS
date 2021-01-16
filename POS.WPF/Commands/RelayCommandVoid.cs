using System;
using System.Windows.Input;

namespace POS.WPF.Commands
{
    public class RelayCommandVoid : ICommand
    {
        private readonly Action Work;

        public event EventHandler CanExecuteChanged;

        public RelayCommandVoid(Action work)
        {
            Work = work;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Work();
        }
    }
}
