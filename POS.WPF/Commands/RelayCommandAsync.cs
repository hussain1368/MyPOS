using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS.WPF.Commands
{
    public class RelayCommandAsync : ICommand
    {
        private readonly Func<Task> Work;

        public event EventHandler CanExecuteChanged;

        public RelayCommandAsync(Func<Task> work)
        {
            Work = work;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Task.Run(async () => await Work());
        }
    }
}
