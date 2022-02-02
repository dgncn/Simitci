using Simitci.Order.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simitci.Order.Services
{
    public class OrderService : IOrderService
    {
        public void CreateOrder()
        {
            //basitçe veritabanındaki Order tablosuna kayıt atacak.

            using (var orderContext = new OrderContext())
            {
                orderContext.Orders.Add(new Entities.Orders()
                {
                    Name = $"Simit Siparişi: {DateTime.Now.Ticks} ",
                    CreateDate = DateTime.Now,
                });
            }
        }
    }
}
