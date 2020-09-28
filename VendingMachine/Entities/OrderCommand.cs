using VendingMachine.Abstractions;

namespace VendingMachine.Entities
{
    class OrderCommand : Command
    {
        #region Fields
        private readonly decimal _amount;
        private readonly int _id;
        private readonly int _quantity;        

        #endregion
        
        #region Constructors
        public OrderCommand(IMachine machine, decimal amount, int id, int quantity) 
            : base(machine)
        {
            _amount = amount;
            _id = id;
            _quantity = quantity;
        }

        #endregion


        #region Command Override
        public override void Execute()
        {
            Machine.Order(_amount, _id, _quantity);
        }
        
        #endregion

    }
}
