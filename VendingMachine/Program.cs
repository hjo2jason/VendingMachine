using System;
using VendingMachine.Abstractions;
using VendingMachine.Entities;
using VendingMachine.Exceptions;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            IMachine machine = new Machine();
            IBuilder builder = new Builder(machine);
            var client = new Client(builder);

            client.ExecuteCommand("?");
            Console.WriteLine("\r\nPlease input your command:\r\n");

            while (true)
            {
                string cmd = "";

                try
                {
                    cmd = Console.ReadLine()?.Trim();
                    if (!client.ExecuteCommand(cmd))
                        break;

                    Console.WriteLine($"Command succeeded[{cmd}].\r\n");
                }
                catch (ParseException e)
                {
                    Console.WriteLine($"Parse failed[{cmd}]. {e.Message}\r\n");
                }
                catch (OrderException e)
                {
                    Console.WriteLine($"Order failed[{cmd}]. {e.Message}\r\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Command failed[{cmd}]. {e.Message}\r\n");
                }
            }

            Console.WriteLine("Exit successfully");
        }
    }
}
