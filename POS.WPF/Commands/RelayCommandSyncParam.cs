using System;

namespace POS.WPF.Commands
{
    public class RelayCommandSyncParam : RelayCommandBase
    {
        private readonly Action<object> _toExecute;

        public RelayCommandSyncParam(Action<object> toExecute)
        {
            _toExecute = toExecute;
        }

        public override void Run(object param)
        {
            IsExecuting = true;
            _toExecute(param);
            IsExecuting = false;
        }
    }
}
