using Microsoft.EntityFrameworkCore;
using Simitci.Order.Entities;

namespace Simitci.Order.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<DomainEvent> DomainEvents { get; set; }
    }
}
