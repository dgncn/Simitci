using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simitci.Order.Data;
using Simitci.Order.Services;
using System;
using System.IO;


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

            Console.WriteLine("Sipariş oluşturmak için 1'e basın.");
            string character;
            do
            {
                character = Console.ReadLine();
                if (character == "1")
                {
                    orderService.CreateOrder();
                }
            } while (character == "1");
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
