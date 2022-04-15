using Microsoft.EntityFrameworkCore;
using orderService.Models;

namespace orderService.Db
{
  public class OrderingDbContext : DbContext
  {
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options) { }

    public DbSet<OrderItem> OrderItems { get; set; }
  }
}
