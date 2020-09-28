namespace VendingMachine.Abstractions
{
    public interface IMachine
    {
        string GetInventory();
        void Order(decimal amount, int id, int quantity);
    }
}
