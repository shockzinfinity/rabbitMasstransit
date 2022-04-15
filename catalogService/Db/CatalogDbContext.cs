using catalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace catalogService.Db
{
  public class CatalogDbContext : DbContext
  {
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    public DbSet<CatalogItem> CatalogItems { get; set; }
  }
}
