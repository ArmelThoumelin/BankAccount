using System;
using System.Threading.Tasks;

namespace Application.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var operation = new BankOperation();
                await operation.Start();
            }
            catch (System.Exception)
            {
                Console.WriteLine(ConsoleResource.GeneralException);
            }
        }
    }
}