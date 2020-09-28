using System;

namespace VendingMachine.Exceptions
{
    public class OrderException : Exception
    {
        public OrderException(string message)
            : base(message)
        {

        }
    }
}
