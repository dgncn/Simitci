using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simitci.Order.Data;
using Simitci.Order.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IOrderService, OrderService>();
                    services
                       .AddDbContext<OrderContext>(options => options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=simitci;User Id=postgres;Password=12345;"));

                    services.AddHostedService<DomainEventSenderService>();
                });
    }
}
