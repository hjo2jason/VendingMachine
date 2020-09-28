using System;
using VendingMachine.Abstractions;

namespace VendingMachine.Entities
{
    class InventoryCommand : Command
    {
        #region Constructors
        public InventoryCommand(IMachine machine) 
            : base(machine)
        {
        }
        #endregion

        #region Command Override

        public override void Execute()
        {
            Console.WriteLine(Machine.GetInventory());
        }        

        #endregion

    }
}
