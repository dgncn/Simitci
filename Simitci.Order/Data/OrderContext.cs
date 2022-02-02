using Microsoft.EntityFrameworkCore;
using Simitci.Order.Entities;

namespace Simitci.Order.Data
{
    public class OrderContext : DbContext
    {
        public DbSet<Orders> Orders { get; set; }
    }
}
