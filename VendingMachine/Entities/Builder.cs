using System;
using VendingMachine.Abstractions;
using VendingMachine.Exceptions;

namespace VendingMachine.Entities
{
    public class Builder : IBuilder
    {
        #region Fields
        private readonly IMachine _machine;
        private readonly string _availableCommands =
            "Available commands: \r\ninv (Show inventory)\r\norder <amount> <item_number> <quantity> (Order)\r\n? (Available commands)\r\nx (Exit)";
        #endregion

        #region Constructors
        public Builder(IMachine machine)
        {
            _machine = machine;
        }

        #endregion

        #region IBuilder
        public string GetAvailableCommands() => _availableCommands;

        // Factory Method
        public Command BuildCommand(string cmd)
        {
            var tmps = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (tmps[0].ToLower())
            {
                case "inv":
                    if (tmps.Length != 1)
                        throw new ParseException($"Wrong inv format: {cmd}. Expected format: inv");

                    return new InventoryCommand(_machine);
                case "order":
                    if (tmps.Length != 4)
                        throw new ParseException("Wrong order format. Expected format: order <amount> <item_number> <quantity>");
                    if (!decimal.TryParse(tmps[1], out decimal amount))
                        throw new ParseException($"Wrong amount format: {tmps[1]}. Must be a number");
                    if (!int.TryParse(tmps[2], out int item))
                        throw new ParseException($"Wrong item_number format: {tmps[2]}. Must be an integer");
                    if (!int.TryParse(tmps[3], out int quantity))
                        throw new ParseException($"Wrong quantity format: {tmps[3]}. Must be an integer");

                    return new OrderCommand(_machine, amount, item, quantity);
                default:
                    throw new ParseException($"Can not understand the command {tmps[0]}.");
            }
        }
        
        #endregion

    }
}
