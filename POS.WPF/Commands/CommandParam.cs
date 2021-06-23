using System;

namespace POS.WPF.Commands
{
    public class CommandParam : CommandBase
    {
        private readonly Action<object> _toExecute;

        public CommandParam(Action<object> toExecute)
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
