using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simitci.Order.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContextFactory()
        {

        }
        public OrderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=simitci;User Id=postgres;Password=12345;");

            return new OrderContext(optionsBuilder.Options);
        }
    }
}
