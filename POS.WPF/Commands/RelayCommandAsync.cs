using System;
using System.Threading.Tasks;

namespace POS.WPF.Commands
{
    public class RelayCommandAsync : RelayCommandBase
    {
        private readonly Func<Task> _toExecute;

        public RelayCommandAsync(Func<Task> toExecute)
        {
            _toExecute = toExecute;
        }

        public override async void Run(object param)
        {
            IsExecuting = true;
            await _toExecute();
            IsExecuting = false;
        }
    }
}
