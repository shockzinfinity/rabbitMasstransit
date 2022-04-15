using catalogService.Db;
using catalogService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace catalogService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CatalogItemController : ControllerBase
{
  private readonly CatalogDbContext _context;
  private readonly ILogger<CatalogItemController> _logger;

  public CatalogItemController(CatalogDbContext context, ILogger<CatalogItemController> logger)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<CatalogItem>>> GetCatalogItems()
  {
    return Ok(await _context.CatalogItems.ToListAsync());
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<CatalogItem>> GetCatalogItem(int id)
  {
    var catalogItem = await _context.CatalogItems.FindAsync(id);

    if (catalogItem == null) return NotFound();

    return Ok(catalogItem);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> PutCatalogItem(int id, CatalogItem catalogItem)
  {
    if (id != catalogItem.Id) return BadRequest();

    _context.Entry(catalogItem).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
    } catch (DbUpdateConcurrencyException) {
      if (!CatalogItemExists(id)) return NotFound();
      else throw;
    }

    return NoContent();
  }

  [HttpPost]
  public async Task<IActionResult> PostCatalogItem(CatalogItem catalogItem)
  {
    _context.CatalogItems.Add(catalogItem);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetCatalogItem", new { id = catalogItem.Id }, catalogItem);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCatalogItem(int id)
  {
    var catalogItem = await _context.CatalogItems.FindAsync(id);

    if (catalogItem == null) return NotFound();

    _context.CatalogItems.Remove(catalogItem);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool CatalogItemExists(int id)
  {
    return _context.CatalogItems.Any(e => e.Id == id);
  }
}
