
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SeventySevenDiamondsBackend.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImagesController(AppDbContext context, IWebHostEnvironment env) {
            _context = context;
            _env = env;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] int itemId, [FromForm] int colorId, [FromForm] int fabricId) {

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            
            var filename = Path.GetFileNameWithoutExtension(file.FileName);

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
            
            if (item == null)
                return NotFound("Item not found.");

            try {

                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(stream);
                }

                var image = new Image {

                    ItemId = itemId,
                    Name = filename,
                    ImageUrl = $"http://localhost:5047/uploads/{uniqueFileName}",
                    ColorId = colorId,
                    FabricId = fabricId,

                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                return Ok(new {

                    id = item.Id,
                    name = item.Name,
                    imageId = image.Id,
                    imageUrl = image.ImageUrl,
                    colorId = image.ColorId,
                    fabricId = image.FabricId

                });

            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id) {

            var image = await _context.Images.FindAsync(id);

            if (image == null)
                return NotFound("Image not found.");

            try {

                var filePath = Path.Combine(_env.WebRootPath, "uploads", Path.GetFileName(image.ImageUrl));

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();

                return NoContent();

            } catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

    }

}
