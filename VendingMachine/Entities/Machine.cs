using System.Text;
using VendingMachine.Abstractions;
using VendingMachine.Exceptions;

namespace VendingMachine.Entities
{
    public class Machine : IMachine
    {
        #region Fields
        private readonly IInventory[] _inventories;
        private readonly int _idSeed;

        #endregion

        #region Constructors
        public Machine()
        {
            _inventories = new IInventory[]
            {
                new Inventory(++_idSeed, "Coke", 10, 1.25m),
                new Inventory(++_idSeed, @"M&M's", 15, 1.89m),
                new Inventory(++_idSeed, "Water", 5, .89m),
                new Inventory(++_idSeed, "Snickers", 7, 2.05m)
            };
        }

        #endregion

        #region IMachine

        public string GetInventory()
        {
            if (_inventories.Length == 0)
                return "";

            StringBuilder sb = new StringBuilder();

            foreach (var i in _inventories)
                sb.AppendLine(i.Information());

            return sb.Remove(sb.Length - 2, 2).ToString();
        }

        public void Order(decimal amount, int id, int quantity)
        {
            if(id <= 0 || id > _inventories.Length)
                throw new OrderException($"Wrong inventory id {id}. Id must be between 1 and {_inventories.Length}");

            _inventories[id - 1].Order(amount, quantity);
        }        

        #endregion

    }
}
