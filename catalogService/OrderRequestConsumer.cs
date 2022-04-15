using catalogService.Db;
using MassTransit;
using sharedModel;

namespace catalogService;

public class OrderRequestConsumer : IConsumer<OrderRequest>
{
  private readonly ILogger<OrderRequestConsumer> _logger;
  private readonly IPublishEndpoint _publishEndpoint;
  private readonly CatalogDbContext _context;

  public OrderRequestConsumer(ILogger<OrderRequestConsumer> logger, IPublishEndpoint publishEndpoint, CatalogDbContext context)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }

  public async Task Consume(ConsumeContext<OrderRequest> context)
  {
    var message = context.Message;

    await Console.Out.WriteLineAsync($"Name: {message.Name}, {message.OrderId}, {message.Units}, {message.CatalogId}");
    _logger.LogInformation($"Name: {message.Name}, {message.OrderId}, {message.Units}, {message.CatalogId}");

    try {
      var catalogItem = await _context.CatalogItems.FindAsync(message.CatalogId);
      if (catalogItem == null || catalogItem.AvailableStock < message.Units) {
        throw new Exception();
      }
      catalogItem.AvailableStock = catalogItem.AvailableStock - message.Units;
      _context.Entry(catalogItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
      await _context.SaveChangesAsync();

      await _publishEndpoint.Publish(new CatalogResponse
      {
        OrderId = message.OrderId,
        CatalogId = message.CatalogId,
        IsSuccess = true
      });

    } catch (Exception) {
      await _publishEndpoint.Publish(new CatalogResponse
      {
        OrderId = message.OrderId,
        CatalogId = message.CatalogId,
        IsSuccess = false
      });
    }
  }
}
