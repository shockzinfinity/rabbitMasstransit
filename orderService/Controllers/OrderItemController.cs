using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orderService.Db;
using orderService.Models;
using sharedModel;

namespace orderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase
{
  private readonly OrderingDbContext _context;
  private readonly IPublishEndpoint _publishEndpoint;

  public OrderItemController(OrderingDbContext context, IPublishEndpoint publishEndpoint)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
  {
    return Ok(await _context.OrderItems.ToListAsync());
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
  {
    var orderItem = await _context.OrderItems.FindAsync(id);

    if (orderItem == null) return NotFound();

    return Ok(orderItem);
  }

  [HttpPost]
  public async Task PostOrderItem(OrderItem orderItem)
  {
    _context.OrderItems.Add(orderItem);
    await _context.SaveChangesAsync();

    int id = orderItem.Id;

    await _publishEndpoint.Publish(new OrderRequest
    {
      OrderId = orderItem.OrderId,
      CatalogId = orderItem.ProductId,
      Units = orderItem.Units,
      Name = orderItem.ProductName
    });
  }
}
