namespace VendingMachine.Abstractions
{
    public interface IBuilder
    {
        Command BuildCommand(string cmd);
        string GetAvailableCommands();
    }
}
