using MassTransit;
using Microsoft.EntityFrameworkCore;
using orderService.Db;
using sharedModel;

namespace orderService
{
  public class CatalogResponseConsumer : IConsumer<CatalogResponse>
  {
    private readonly ILogger<CatalogResponse> _logger;
    private readonly OrderingDbContext _context;

    public CatalogResponseConsumer(ILogger<CatalogResponse> logger, OrderingDbContext context)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Consume(ConsumeContext<CatalogResponse> context)
    {
      var message = context.Message;

      await Console.Out.WriteLineAsync($"CatalogId: {message.CatalogId}, {message.OrderId}");
      _logger.LogInformation($"CatalogId: {message.CatalogId}, {message.OrderId}");

      if(!message.IsSuccess) {
        // TODO: if transaction is not successful, remove ordering item

        var orderItem = await _context.OrderItems.Where(o => o.ProductId == message.CatalogId && o.OrderId == message.OrderId).FirstOrDefaultAsync();
        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
      }
    }
  }
}
