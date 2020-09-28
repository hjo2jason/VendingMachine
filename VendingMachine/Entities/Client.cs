using System;
using VendingMachine.Abstractions;

namespace VendingMachine.Entities
{
    public class Client
    {
        #region Fields
        private readonly IBuilder _parser;
        private Command _cmd;

        #endregion

        #region Constructors
        public Client(IBuilder parser)
        {
            _parser = parser;
        }

        #endregion
        
        #region Methods
        protected void SetCommand(Command cmd) => _cmd = cmd;

        protected void Execute()
        {
            _cmd?.Execute();
        }

        public bool ExecuteCommand(string cmd)
        {
            if (0 == string.Compare(cmd, "x", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (0 == string.CompareOrdinal(cmd, "?"))
            {
                Console.WriteLine(_parser.GetAvailableCommands());
            }
            else
            {
                var command = _parser.BuildCommand(cmd);
                SetCommand(command);
                Execute();
            }

            return true;
        }

        #endregion
    }
}
