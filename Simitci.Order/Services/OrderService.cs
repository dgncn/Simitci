using Newtonsoft.Json;
using Simitci.Order.Data;
using Simitci.Order.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simitci.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderContext _orderContext;
        public OrderService(OrderContext orderContext)
        {
            _orderContext = orderContext;

        }
        public void CreateOrder()
        {
            //basitçe veritabanındaki Order tablosuna kayıt atacak.
            //console uygulamasıyla context-db bağlantısını kurmak için böyle bir yöntem kullandım.

            var applicationDbContextFactory = new OrderContextFactory();
            var s = new string[1];
            
            using (var dbContext = applicationDbContextFactory.CreateDbContext(s))
            {
                using var transaction = dbContext.Database.BeginTransaction();

                var insertedEntityEntry = dbContext.Orders.Add(new Orders()
                {
                    Name = $"Simit Siparişi: {DateTime.Now.Ticks} ",
                    CreateDate = DateTime.Now,
                });
                dbContext.SaveChanges();
                var integrationEventData = JsonConvert.SerializeObject(new
                {
                    orderId = insertedEntityEntry.Entity.Id,
                    orderName = insertedEntityEntry.Entity.Name,
                });
                dbContext.DomainEvents.Add(
                    new DomainEvent()
                    {
                        EventName = "order.create",
                        EventData = integrationEventData
                    }
                );

                dbContext.SaveChanges();
                transaction.Commit();
            }


        }
    }
}
