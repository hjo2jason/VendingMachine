using VendingMachine.Abstractions;
using VendingMachine.Exceptions;

namespace VendingMachine.Entities
{
    class Inventory : IInventory
    {
        #region Properties
        public int Id { get; }
        public string Name { get;}
        public int Available { get; protected set; }
        public decimal Price { get;}

        #endregion

        #region Constructors
        public Inventory(int id, string name, int qty, decimal price)
        {
            Id = id;
            Name = name;
            Available = qty;
            Price = price;
        }
        #endregion

        #region Methods

        protected decimal RequiredPayment(int quantity) => Price * quantity;

        #endregion

        #region IInventory

        public void Order(decimal amount, int quantity)
        {
            if (quantity <= 0)
                throw new OrderException($"Must buy one or more items. Current Inventory for {Name}[{Id}]: {Available}");

            if (quantity > Available)
                throw new OrderException($"Do not have {quantity} for {Name}[{Id}]. Current Inventory: {Available}");

            var requiredPayment = RequiredPayment(quantity);
            if (amount < requiredPayment)
                throw new OrderException($"Underpayment. Need exact change: ${requiredPayment} for {quantity} {Name}[{Id}]");

            if (amount > requiredPayment)
                throw new OrderException($"Overpayment. Need exact change: ${requiredPayment} for {quantity} {Name}[{Id}]");

            Available -= quantity;
        }

        public string Information()
        {
            return $@"{Id}   {Name} ({Available}): ${Price:#.00}";
        }

        #endregion
    }
}
