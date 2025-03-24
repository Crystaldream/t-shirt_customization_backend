
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase {

    private readonly AppDbContext _context;

    public ItemsController(AppDbContext dbContext) {

        ArgumentNullException.ThrowIfNull(dbContext);
        _context = dbContext;

    }

    [HttpGet]
    public async Task<IActionResult> GetItems() {

        try {

            int count = await _context.Items.CountAsync();

            if (count == 0)
                return NotFound("Database is connected, but no items found.");

            var items = await _context.Items
                .Include(i => i.Colors)
                .Include(i => i.Fabrics)
                .Include(i => i.Images)
                .Select(i => new {

                    id = i.Id,
                    name = i.Name,
                    colorCount = i.Colors.Count,
                    FabricCount = i.Fabrics.Count,
                    ImageCount = i.Images.Count,
                    images = i.Images.Select(img => new {
                        id = img.Id, 
                        imageUrl = img.ImageUrl,
                        colorId = img.ColorId,
                        fabricId = img.FabricId
                    }).ToList()
                    
                }
                ).ToListAsync();

            if (!items.Any())
                return NotFound("No items found.");

            return Ok(items);

        } catch (Exception ex) {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemDetails(int id) {

        var item = await _context.Items
            .Include(i => i.Colors)
            .Include(i => i.Fabrics)
            .Include(i => i.Images)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return NotFound("Item not found.");

        var result = new {

            id = item.Id,
            name = item.Name,
            configurations = item.Colors.SelectMany(color => item.Fabrics, (color, fabric) => new {
                colorId = color.Id,
                colorName = color.Name,
                fabricId = fabric.Id,
                fabricName = fabric.Name,
                images = item.Images
                    .Where(img => img.ColorId == color.Id && img.FabricId == fabric.Id)
                    .Select(img => new { id = img.Id, imageUrl = img.ImageUrl })
                    .ToList()
                }).ToList()

        };

        return Ok(result);

    }

}
