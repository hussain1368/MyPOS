using System;

namespace POS.WPF.Commands
{
    public class RelayCommandSyncVoid : RelayCommandBase
    {
        private readonly Action _toExecute;

        public RelayCommandSyncVoid(Action toExecute)
        {
            _toExecute = toExecute;
        }

        public override void Run(object param)
        {
            IsExecuting = true;
            _toExecute();
            IsExecuting = false;
        }
    }
}
