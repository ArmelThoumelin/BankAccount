using Domain;
using Infrastructure.InMemoryDB;
using Infrastructure.SQLiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IBankRepository, SLBankRepository>(); 
                        //services.AddScoped<IBankRepository, IMBankRepository>(); 
                        services.AddScoped<IBank, Bank>(); 
                        services.AddScoped<BankOperation>(); 
                    })
                    .Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var bankOperation = services.GetService<BankOperation>();
                    await bankOperation.Start();
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine(ConsoleResource.GeneralException);
            }
        }
    }
}