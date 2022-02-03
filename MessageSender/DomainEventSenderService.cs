using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Simitci.Order.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageSender
{
    public class DomainEventSenderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DomainEventSenderService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            using var scope = _scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
            dbContext.Database.EnsureCreated();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishDomainEvents(stoppingToken);
            }
        }

        private async Task PublishDomainEvents(CancellationToken stoppingToken)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "admin", Password = "123456" };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();

                while (!stoppingToken.IsCancellationRequested)
                {
                    {
                        using var scope = _scopeFactory.CreateScope();
                        using var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                        var events = dbContext.DomainEvents.OrderBy(o => o.Id).ToList();
                        foreach (var e in events)
                        {
                            var body = Encoding.UTF8.GetBytes(e.EventData);
                            channel.BasicPublish(exchange: "user",
                                                             routingKey: e.EventName,
                                                             basicProperties: null,
                                                             body: body);
                            Console.WriteLine("Published: " + e.EventName + " " + e.EventData);
                            dbContext.Remove(e);
                            dbContext.SaveChanges();
                        }
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
