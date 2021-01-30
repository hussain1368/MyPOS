using System;
using System.Threading.Tasks;

namespace POS.WPF.Commands
{
    public class RelayCommandAsyncParam : RelayCommandBase
    {
        private readonly Func<object, Task> _toExecute;

        public RelayCommandAsyncParam(Func<object, Task> toExecute)
        {
            _toExecute = toExecute;
        }

        public override async void Run(object param)
        {
            IsExecuting = true;
            await _toExecute(param);
            IsExecuting = false;
        }
    }
}
