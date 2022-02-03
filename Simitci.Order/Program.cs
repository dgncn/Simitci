using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simitci.Order.Data;
using Simitci.Order.Services;
using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Order servisinin görevi client'ın siparişini oluşturmaktır. 
/// Projenin ana bağlamı olan outbox pattern demo uygulaması olduğundan veritabanı bağlantı ve kayıt işlemleri basit tutulmuştur.
/// </summary>
namespace Simitci.Order
{
    public class Program
    {

        static void Main(string[] args)
        {
            var provider = GetServiceProvider();
            var orderService = provider.GetService<IOrderService>();

            Console.WriteLine("2 saniyede bir otomatik sipariş oluşturur");
            string character;
            do
            {
                character = Console.ReadLine();
                if (character == "1")
                {
                    orderService.CreateOrder();
                }
            } while (character == "1");

            //while (true)
            //{
            //    Task.Run(() =>
            //    {
            //        orderService.CreateOrder();
            //    });
            //    Task.Delay(TimeSpan.FromSeconds(2)).Wait();
            //}
            //while (true)
            //{
            //    Console.WriteLine("Published: ");
            //    orderService.CreateOrder();
            //    Task.Delay(2000);
            //}
        }


        static ServiceProvider GetServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            var services = new ServiceCollection()
                .AddScoped<IOrderService, OrderService>()
                .AddSingleton<IConfigurationRoot>(configuration);

            services
                .AddDbContext<OrderContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            var provider = services.BuildServiceProvider();
            return provider;
        }
    }



}
