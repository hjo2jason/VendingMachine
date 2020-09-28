namespace VendingMachine.Abstractions
{
    interface IInventory
    {
        void Order(decimal amount, int quantity);
        string Information();
    }
}
