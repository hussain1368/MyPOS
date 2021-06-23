using System;

namespace POS.WPF.Commands
{
    public class CommandSync : CommandBase
    {
        private readonly Action _toExecute;

        public CommandSync(Action toExecute)
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
