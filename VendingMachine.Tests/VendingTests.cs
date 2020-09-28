using VendingMachine.Abstractions;
using VendingMachine.Entities;
using VendingMachine.Exceptions;
using Xunit;

namespace VendingMachine.Tests
{
    public class VendingTests
    {
        [Fact]
        public void BuildCommand_Exception_Tests()
        {
            // Arrange
            IMachine machine = new Machine();
            IBuilder builder = new Builder(machine);
            var client = new Client(builder);

            // Act & Assert
            var ex = Assert.Throws<ParseException>(() => client.ExecuteCommand("inventory"));
            Assert.Equal("Can not understand the command inventory.", ex.Message);

            ex = Assert.Throws<ParseException>(() => client.ExecuteCommand("inv test"));
            Assert.Equal("Wrong inv format: inv test. Expected format: inv", ex.Message);

            ex = Assert.Throws<ParseException>(() => client.ExecuteCommand("order 1 1"));
            Assert.Equal("Wrong order format. Expected format: order <amount> <item_number> <quantity>",
                ex.Message);

            ex = Assert.Throws<ParseException>(() => client.ExecuteCommand("order a 1 1"));
            Assert.Equal("Wrong amount format: a. Must be a number", ex.Message);

            ex = Assert.Throws<ParseException>(() => client.ExecuteCommand("order 1 a 1"));
            Assert.Equal("Wrong item_number format: a. Must be an integer", ex.Message);

            ex = Assert.Throws<ParseException>(() => client.ExecuteCommand("order 1 1 a"));
            Assert.Equal("Wrong quantity format: a. Must be an integer", ex.Message);
        }

        [Fact]
        public void Order_Exception_Tests()
        {
            // Arrange
            IMachine machine = new Machine();
            IBuilder builder = new Builder(machine);
            var client = new Client(builder);

            client.ExecuteCommand("order 2.5 1 2");

            // Act & Assert
            var ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 1 5 1"));
            Assert.Equal("Wrong inventory id 5. Id must be between 1 and 4", ex.Message);

            ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 1 0 1"));
            Assert.Equal("Wrong inventory id 0. Id must be between 1 and 4", ex.Message);

            // Act & Assert
            ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order -1.25 1 -1"));
            Assert.Equal("Must buy one or more items. Current Inventory for Coke[1]: 8", ex.Message);

            // Act & Assert
            ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 12.5 1 10"));
            Assert.Equal("Do not have 10 for Coke[1]. Current Inventory: 8", ex.Message);

            // Act & Assert
            ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 2.15 1 2"));
            Assert.Equal("Underpayment. Need exact change: $2.50 for 2 Coke[1]", ex.Message);

            // Act & Assert
            ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 3 1 2"));
            Assert.Equal("Overpayment. Need exact change: $2.50 for 2 Coke[1]", ex.Message);
        }

        [Fact]
        public void Client_Tests()
        {
            // Arrange
            IMachine machine = new Machine();
            IBuilder builder = new Builder(machine);
            var client = new Client(builder);

            // Act
            client.ExecuteCommand("order 5 1 4");
            client.ExecuteCommand("Order 1.78 3 2");
            client.ExecuteCommand("order   2.67 3 3");

            // Assert
            var invs = machine.GetInventory().Split("\r\n");
            Assert.Equal(4, invs.Length);
            Assert.Contains(@"1   Coke (6): $1.25", invs);
            Assert.Contains(@"2   M&M's (15): $1.89", invs);
            Assert.Contains(@"3   Water (0): $.89", invs);
            Assert.Contains(@"4   Snickers (7): $2.05", invs);

            // Act & Assert
            var ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 0 3 0"));
            Assert.Equal("Must buy one or more items. Current Inventory for Water[3]: 0", ex.Message);

            // Act & Assert
            ex = Assert.Throws<OrderException>(() => client.ExecuteCommand("order 12.5 1 10"));
            Assert.Equal("Do not have 10 for Coke[1]. Current Inventory: 6", ex.Message);

            // Assert
            invs = machine.GetInventory().Split("\r\n");
            Assert.Equal(4, invs.Length);
            Assert.Contains(@"1   Coke (6): $1.25", invs);
            Assert.Contains(@"2   M&M's (15): $1.89", invs);
            Assert.Contains(@"3   Water (0): $.89", invs);
            Assert.Contains(@"4   Snickers (7): $2.05", invs);

            // Act
            client.ExecuteCommand("order 2.05 4 1");
            client.ExecuteCommand("INV");

            // Act && Assert
            invs = machine.GetInventory().Split("\r\n");
            Assert.Equal(4, invs.Length);
            Assert.Contains(@"1   Coke (6): $1.25", invs);
            Assert.Contains(@"2   M&M's (15): $1.89", invs);
            Assert.Contains(@"3   Water (0): $.89", invs);
            Assert.Contains(@"4   Snickers (6): $2.05", invs);
        }
    }
}
